using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    // Inserts seed data to db if empty
    public class DutchSeeder
    {
        DutchContext _context;
        IHostingEnvironment _host;

        public DutchSeeder(DutchContext context, IHostingEnvironment host) 
        {
            _context = context;
            _host = host;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            if (!_context.Products.Any())
            {
                var seedDataPath = Path.Combine(_host.ContentRootPath, "Data/art.json");
                var rawData = File.ReadAllText(seedDataPath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(rawData);
                _context.Products.AddRange(products);

                var order = _context.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if (order != null)
                {
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Id = 1,
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };
                }

                _context.SaveChanges();
            }
        }

    }
}
