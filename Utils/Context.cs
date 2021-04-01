using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PersonalCrm
{
    public class Context
    {
        public static string GetUserId(IHttpContextAccessor ctx)
        {
            var identity = ctx.HttpContext.User.Identity as ClaimsIdentity;
            var id = identity.FindFirst("Id").Value;
            return id;
        }
    }
}