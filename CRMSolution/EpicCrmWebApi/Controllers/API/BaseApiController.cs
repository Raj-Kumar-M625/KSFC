using System.Configuration;
using System.Web.Http;

namespace EpicCrmWebApi
{
    public class BaseApiController : ApiController
    {
        protected string CurrentUserStaffCode => User.Identity.Name;
        protected bool IsSuperAdmin => User.IsInRole("Admin") && User.IsInRole("Manager");
    }
}
