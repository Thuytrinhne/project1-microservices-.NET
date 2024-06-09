namespace User.API.Settings
{
    public class MongoDbConfig
    {
        public string Name { get; init; }
        public string Host { get; init; }
        public int Port { get; init; }
        public string EmailVerificationCollectionName { get; set; }
        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}
