using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TreasuryToolkit.Core.Contracts;

namespace TreasuryToolkit.Infra.Services
{
    public class JsonCompanyService : ICompanyService
    {
        private readonly List<string> _companies;

        public JsonCompanyService()
        {
            try
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "companies.json");

                if (File.Exists(jsonPath))
                {
                    string jsonContent = File.ReadAllText(jsonPath);
                    _companies = JsonSerializer.Deserialize<List<string>>(jsonContent) ?? [];
                }
                else
                {
                    _companies = ["Error: companies.json missing"];
                }
            }
            catch
            {
                _companies = ["Error loading companies"];
            }
        }

        public IReadOnlyList<string> GetCompanyNames() => _companies;
    }
}
