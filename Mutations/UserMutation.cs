using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace PersonalCrm
{
    [ExtendObjectType(Name = "Mutation")]
    public class UserMutation
    {

        private readonly ILogger<UserMutation> logger;
        private readonly IUserService userService;

        public UserMutation(ILogger<UserMutation> logger, IUserService userService)
        {
            this.logger = logger;
            this.userService = userService;
        }

        public async Task<User> Register(string name, string email, string password)
        {
            return await userService.Create(name, email, password);
        }

        public async Task<UserTokenResult> Login(string email, string password)
        {
            return await userService.IssueToken(email, password);
        }

    }
}
