using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace PersonalCrm
{
    [ExtendObjectType(Name = "Mutation")]
    public class ReminderMutation
    {

        private readonly ILogger<ReminderMutation> logger;
        private readonly IReminderService reminderService;
        private readonly IHttpContextAccessor ctx;

        public ReminderMutation(ILogger<ReminderMutation> logger, IReminderService reminderService, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.reminderService = reminderService;
            this.ctx = httpContextAccessor;
        }

        [Authorize]
        public async Task<Reminder> CreateReminder(string title, string schedule)
        {
            var id = Context.GetUserId(ctx);
            return await reminderService.Create(title, schedule, id);
        }
    }
}
