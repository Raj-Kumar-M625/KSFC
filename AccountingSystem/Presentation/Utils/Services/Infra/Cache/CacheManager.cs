using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Services.Infra.Cache
{
    public class CacheManager : ICacheManager
    {
        private readonly ICacheStorage cacheStorage;

        // group type, Tuple<lock object, cache keys[]>
        private static readonly ConcurrentDictionary<Type, Tuple<object, HashSet<string>>> cgroups
            = new ConcurrentDictionary<Type, Tuple<object, HashSet<string>>>();

       // private static readonly IDictionary<Type, Type[]> typeDeps;

        private static readonly IDictionary<Type, Type[]> typeDeps = new Dictionary<Type, Type[]>();

        static CacheManager()
        {
            //typeDeps = new Dictionary<Type, Type[]>
            //{
            //    {typeof(Chef), new[] {typeof(Dinner)}},
            //    {typeof(Country), new[] {typeof(Dinner), typeof(Chef)}}
            //};
        }

        public CacheManager(ICacheStorage cacheStorage)
        {
            this.cacheStorage = cacheStorage;
        }

        public void RemoveGroup(Type group)
        {
            Tuple<object, HashSet<string>> tuple;

            if (cgroups.ContainsKey(group) && cgroups.TryGetValue(group, out tuple))
            {
                lock (tuple.Item1)
                {
                    foreach (var key in tuple.Item2)
                    {
                        cacheStorage.Remove(key);
                    }
                }
            }
        }

        public void Remove(string key)
        {
            cacheStorage.Remove(key);
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> getFunc, Type group)
        {
            var val = cacheStorage.Get(key);

            if (val != null)
            {
                return (T)val;
            }

            checkGroup<T>(key, group);

            val = await getFunc();

            cacheStorage.Insert(key, val);

            return (T)val;
        }

        public T Get<T>(string key, Func<T> getFunc, Type group = null)
        {
            var val = cacheStorage.Get(key);

            if (val != null)
            {
                return (T)val;
            }

            checkGroup<T>(key, group);

            val = getFunc();

            cacheStorage.Insert(key, val);

            return (T)val;
        }

        public void ChangeAction<T>(ChangeType chtype)
        {
            var currentType = typeof(T);

            RemoveGroup(currentType);

            if (!typeDeps.ContainsKey(currentType)) return;

            var deps = typeDeps[currentType];

            if (chtype != ChangeType.Edit || deps == null) return;

            foreach (var dep in deps)
            {
                RemoveGroup(dep);
            }
        }

        private static void checkGroup<T>(string key, Type group)
        {
            if (group != null)
            {
                cgroups.AddOrUpdate(
                    group,
                    g => new Tuple<object, HashSet<string>>(new object(), new HashSet<string> {key}), // create new group
                    (gname, tuple) =>
                    {
                        // add new key to existing group
                        lock (tuple.Item1)
                        {
                            tuple.Item2.Add(key);
                        }

                        return tuple;
                    });
            }
        }
    }
}