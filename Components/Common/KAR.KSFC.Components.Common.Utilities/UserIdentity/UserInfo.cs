using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Utilities
{
    public class UserInfo
    {
        
        /// <summary>
        /// The DisplayName
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The UserId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The ROles
        /// </summary>
        public List<string> Role { get; set; }

        /// <summary>
        /// The PhoneNumber
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The Pan
        /// </summary>
        public string Pan { get; set; }
        /// <summary>
        /// Ip Address
        /// </summary>
        public string IPAddress { get; set; }
    }
}
