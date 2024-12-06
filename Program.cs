using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleOlimpic
{
    public class Country
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Continent { get; set; }
        public int GoldMedals { get; set; }
        public int SilverMedals { get; set; }
        public int BronzeMedals { get; set; }
        public int TotalMedals { get; set; }
        public int Rank { get; set; }
        public int RankTotalMedals { get; set; }
    }

    public class OlympicDataResponse
    {
        public List<Country> Data { get; set; }
        // ... outros campos da resposta
    }

    class Program
    {
        static async Task Main()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://apis.codante.io/olympic-games/countries");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var olympicData = JsonSerializer.Deserialize<OlympicDataResponse>(content);

            // Ordenar os países por total de medalhas
            var orderedCountries = olympicData.Data.OrderByDescending(c => c.TotalMedals).ToList();

            // Agrupar por continente e calcular o total de medalhas
            var continents = orderedCountries
                .GroupBy(c => c.Continent)
                .Select(g => new
                {
                    Continent = g.Key,
                    TotalGold = g.Sum(c => c.GoldMedals),
                    TotalSilver = g.Sum(c => c.SilverMedals),
                    TotalBronze = g.Sum(c => c.BronzeMedals),
                    TotalMedals = g.Sum(c => c.TotalMedals)
                })
                .OrderByDescending(c => c.TotalMedals)
                .ToList();

            // Imprimir o resultado (ajuste conforme necessário)
            foreach (var continent in continents)
            {
                Console.WriteLine($"Continente: {continent.Continent}");
                Console.WriteLine($"Total de Ouro: {continent.TotalGold}");
                Console.WriteLine($"Total de Prata: {continent.TotalSilver}");
                Console.WriteLine($"Total de Bronze: {continent.TotalBronze}");
                Console.WriteLine($"Total de Medalhas: {continent.TotalMedals}");
                Console.WriteLine();
            }
        }
    }
}