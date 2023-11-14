using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Security
{
    public static class SMSSecurity
    {
        //Method to Generate hash code
        public static string HashData(string content, string hashType)//Defines types of hash string outputs available
        {
            using (var sha1 = HashAlgorithm.Create(hashType))//Setup data variable to hold hashed value
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(content));//Hashing algorithm for hashing a Data instance
                var hashBldr = new StringBuilder(hash.Length * 2);
                foreach (var bt in hash)
                {
                    hashBldr.Append(bt.ToString("x2"));
                }
                return hashBldr.ToString();
            }
        }
    }
}
