using Microsoft.Extensions.Configuration;

namespace PoseidonPool.Persistance
{
    static class Configuration
    {
        static public string ConncetString
        {
            get
            {
                ConfigurationManager configuration = new();
                configuration.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/PoseidonPool.API"));
                configuration.AddJsonFile("appsettings.json");

                return configuration.GetConnectionString("DefaultConnection");

            }
        }
    }
}
