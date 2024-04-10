namespace Main.Settings.Database
{
    public class MongoSettings
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }

        public string GetConnectionStringWithDatabaseName()
        {
            return this.ConnectionString + "/" + this.DatabaseName;

        }
    }
}

