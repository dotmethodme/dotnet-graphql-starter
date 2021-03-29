using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace PersonalCrm
{
    public interface IUserService
    {
        User Create(string name, string email, string password);
        Task<UserTokenResult> IssueToken(string email, string password);
        Task<User> GetByEmail(string email);
        Task<User> GetById(string id);
    }

    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> col;
        private readonly JwtSettings jwtSettings;

        public UserService(IDatabase db, IOptions<JwtSettings> jwtSettings)
        {
            this.jwtSettings = jwtSettings.Value;

            col = db.GetDatabase().GetCollection<User>("User");
            ensureIndex();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await col.Find(d => d.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> GetById(string id)
        {
            return await col.Find(d => d.Id == id).FirstOrDefaultAsync();
        }

        public User Create(string name, string email, string password)
        {
            var salt = CryptoUtil.GenerateSalt();
            var hash = CryptoUtil.GetSaltedPasswordHash(password, salt);
            var user = new User
            {
                Name = name,
                Email = email,
                Salt = salt,
                Hash = hash,
            };

            col.InsertOne(user);
            return user;
        }

        public async Task<UserTokenResult> IssueToken(string email, string password)
        {
            var user = await GetByEmail(email);
            if (user == null)
            {
                throw new Exception("No such user");
            }

            var hash = CryptoUtil.GetSaltedPasswordHash(password, user.Salt);
            if (hash != user.Hash)
            {
                throw new Exception("Password is incorrect");
            }

            UserToken token = new(user.Id, user.Name, user.Email);

            var jwt = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(jwtSettings.Secret)
                .AddClaim("Id", user.Id)
                .AddClaim("Name", user.Name)
                .AddClaim("Email", user.Email)
                .ExpirationTime(DateTime.Now.AddDays(1))
                .Audience("personalcrm")
                .Issuer("personalcrm")
                .Encode();


            return new UserTokenResult(token, jwt);
        }

        private void ensureIndex()
        {
            BsonDocument index = new()
            {
                { "Email", 1 }
            };
            var indexModel = new CreateIndexModel<User>(index, new CreateIndexOptions { Unique = true });
            col.Indexes.CreateOne(indexModel);
        }
    }

}