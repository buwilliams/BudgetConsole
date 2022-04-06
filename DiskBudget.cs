using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BudgetConsole
{
    public class DiskBudget : Budget
    {
        const string FILENAME_FORMAT = "{0}-{1}.json";

        public bool Saved = false;

        public DiskBudget() : base()
        {
        }

        protected string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize<Budget>(this, options);
            return jsonString;
        }

        public Budget FromJson(string json)
        {
            Budget? budget = JsonSerializer.Deserialize<Budget>(json);
            return budget == null ? new Budget() : budget;
        }

        public async Task<Budget> Load(int month, int year)
        {
            string text = await File.ReadAllTextAsync(string.Format(FILENAME_FORMAT, year, month));
            Saved = true;
            return FromJson(text);
        }

        public async Task Save(string json, int month, int year)
        {
            Saved = true;
            await File.WriteAllTextAsync(string.Format(FILENAME_FORMAT, year, month), json);
        }
    }
}
