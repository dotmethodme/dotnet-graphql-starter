using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalCrm
{
    [ExtendObjectType(Name = "Query")]
    public class ReminderQuery
    {

        private readonly ILogger<ReminderQuery> logger;
        private readonly IReminderService reminderService;
        private readonly IHttpContextAccessor ctx;

        public ReminderQuery(ILogger<ReminderQuery> logger, IReminderService reminderService, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.reminderService = reminderService;
            this.ctx = httpContextAccessor;
        }

        [Authorize]
        public async Task<List<Reminder>> GetReminders()
        {
            var id = Context.GetUserId(ctx);
            var reminders = await reminderService.GetByOwner(id);

            return reminders;
        }

    }
}
