using CitizensWebApi.Models;

namespace CitizensWebApi.Services
{
    public class FileService
    {
        private readonly string _filePath;
        private readonly ILogger<FileService> _logger;

        public FileService(IConfiguration configuration, ILogger<FileService> logger)
        {
            _filePath = configuration.GetValue<string>("FileStorage:Path") ?? "citizens.csv";
            _logger = logger;
        }

        public async Task<List<Citizen>> ReadCitizensAsync()
        {
            var citizens = new List<Citizen>();
            if (!File.Exists(_filePath)) return citizens;

            try
            {
                var lines = await File.ReadAllLinesAsync(_filePath);
                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    if (data.Length >= 5)
                    {
                        citizens.Add(new Citizen
                        {
                            FirstName = data[0],
                            LastName = data[1],
                            CI = data[2],
                            BloodGroup = data[3],
                            PersonalAsset = data[4]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading citizens file");
            }

            return citizens;
        }

        public async Task WriteCitizensAsync(List<Citizen> citizens)
        {
            try
            {
                var lines = citizens.Select(c => string.Join(",", c.FirstName, c.LastName, c.CI, c.BloodGroup, c.PersonalAsset));
                await File.WriteAllLinesAsync(_filePath, lines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing citizens file");
            }
        }
    }
}