using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PersonalCrm
{
    [ExtendObjectType(Name = "Query")]
    public class UserQuery
    {

        private readonly ILogger<UserQuery> logger;
        private readonly IUserService userService;
        private readonly IHttpContextAccessor ctx;

        public UserQuery(ILogger<UserQuery> logger, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.userService = userService;
            this.ctx = httpContextAccessor;
        }

        [Authorize]
        public async Task<User> Me()
        {
            var identity = ctx.HttpContext.User.Identity as ClaimsIdentity;
            var id = identity.FindFirst("Id").Value;
            var user = await userService.GetById(id);

            return user;
        }

    }
}
