using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Persistence
{
    public class ApplicationDbContextInitializer
    {
        public static void Initialize(
           ApplicationDBContext dbContext,
           IServiceProvider serviceProvider)
        {
            var initializer = new ApplicationDbContextInitializer();
            initializer.Seed(dbContext, serviceProvider);
        }

        private void Seed(
            ApplicationDBContext dbContext,
            IServiceProvider serviceProvider)
        {
            dbContext.Database.EnsureCreated();
            SeedProducts(dbContext);
        }
        private void SeedProducts(ApplicationDBContext dbContext)
        {
            var products = dbContext.Products.ToList();
            if (!products.Any())
            {
                products = JsonConvert.DeserializeObject<List<Product>>(ReadJson("products.json"));
                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();
            }
        }
        private static string ReadJson(string fileName)
        {
            var assembly = typeof(ApplicationDBContext).Assembly;
            var resources = assembly.GetManifestResourceNames();

            using Stream stream = assembly.GetManifestResourceStream(resources.First(x => x.Contains(fileName)));
            using StreamReader reader = new StreamReader(stream);
            var result = reader.ReadToEnd();
            return result;
        }
    }
}
