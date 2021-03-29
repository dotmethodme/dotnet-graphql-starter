namespace PersonalCrm
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public class JwtSettings
    {
        public string Secret { get; init; }
        public string Issuer { get; init; }
        public string Audience { get; init; }
    }
}