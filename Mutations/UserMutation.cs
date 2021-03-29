using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace PersonalCrm
{
    public class UserMutation
    {

        private readonly ILogger<UserMutation> logger;
        private readonly IUserService userService;

        public UserMutation(ILogger<UserMutation> logger, IUserService userService)
        {
            this.logger = logger;
            this.userService = userService;
        }

        public User Register(string name, string email, string password)
        {
            var user = userService.Create(name, email, password);
            return user;
        }

        public async Task<UserTokenResult> Login(string email, string password)
        {
            var token = await userService.IssueToken(email, password);
            return token;
        }

    }
}
