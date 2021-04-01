using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace PersonalCrm
{
    public interface IReminderService
    {
        Task<Reminder> Create(string title, string schedule, string ownerId);
        Task<List<Reminder>> GetByOwner(string id);
    }

    public class ReminderService : IReminderService
    {
        private readonly IMongoCollection<User> userCollection;
        private readonly IMongoCollection<Reminder> reminderCollection;
        private readonly JwtSettings jwtSettings;

        public ReminderService(IDatabase db, IOptions<JwtSettings> jwtSettings)
        {
            this.jwtSettings = jwtSettings.Value;
            userCollection = db.GetDatabase().GetCollection<User>("User");
            reminderCollection = db.GetDatabase().GetCollection<Reminder>("Reminder");
        }

        public async Task<Reminder> Create(string title, string schedule, string ownerId)
        {

            Reminder reminder = new()
            {
                Title = title,
                Schedule = schedule,
                OwnerId = ownerId
            };

            await reminderCollection.InsertOneAsync(reminder);

            return reminder;
        }

        public async Task<List<Reminder>> GetByOwner(string ownerId)
        {
            var query = from rem in reminderCollection.AsQueryable()
                        join user in userCollection.AsQueryable()
                        on rem.OwnerId equals user.Id
                        where rem.OwnerId.Equals(ownerId)
                        select new Reminder()
                        {
                            Id = rem.Id,
                            Schedule = rem.Id,
                            Title = rem.Title,
                            Owner = user,
                            OwnerId = rem.Id,
                        };


            return await query.ToListAsync();
        }

    }

}