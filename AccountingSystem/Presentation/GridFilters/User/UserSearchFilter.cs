using Domain.User;
using Presentation.Utils;
using System.Linq;

namespace Presentation.GridFilters.User
{

    public class UserSearchFilter
    {
        public FilterBuilder<UsersList> GetUserCriteria(IQueryable<UsersList> UserList,UserFilter userfilters)
        {
            userfilters.forder = userfilters.forder ?? new string[] { };
           // IQueryable<UsersList> Query = UserList.AsQueryable();
            //var frow = new UserFilterRow();
            var filterBuilder = new FilterBuilder<UsersList>();
            filterBuilder
                .Add("userfilters.UserName",q => userfilters.UserName != null ? q.Where(o => o.UserName.Contains(userfilters.UserName)) : q)
                .Add("userfilters.EmailId",q => userfilters.EmailId != null ? q.Where(o => o.EmailId.Contains(userfilters.EmailId)) : q)
                .Add("userfilters.Role",q => userfilters.Role != null ? q.Where(o => o.Role.Contains(userfilters.Role)) : q)
                .Add("userfilters.MobileNumber",q => userfilters.MobileNumber != null ? q.Where(o => o.MobileNumber.Contains(userfilters.MobileNumber)) : q);

            return filterBuilder;


        }
    }
}
