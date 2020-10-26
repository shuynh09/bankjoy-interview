using BankingApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BankingApi.Repositories
{
    public class Repository : IRepository
    {
        private readonly ILogger _logger;
        private const string fileName = "database.json";

        public Repository(ILogger<Repository> logger)
        {
            _logger = logger;
        }

        public async Task<InstitutionModel> ReadDataAsync()
        {
            try
            {
                using (FileStream fs = File.OpenRead(fileName))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var model = await JsonSerializer.DeserializeAsync<InstitutionModel>(fs, options);
                    return model;
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async void SaveDataAsync(InstitutionModel model)
        {
            try {
                using (FileStream fs = File.Create(fileName))
                {
                    await JsonSerializer.SerializeAsync(fs, model);
                }
            }
            catch (Exception ex)
            {
                Thread.Sleep(5);

                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
