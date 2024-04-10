namespace Main.Settings.Database
{
    public class RedisSettings
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }

        public string GetConnectionString()
        {
            if(string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new InvalidOperationException("Connection string not found");
            }
            return this.ConnectionString;
        }
    }
}

