using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using DutchTreat.Data.Entities;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    // Inserts seed data to db if empty
    public class DutchSeeder
    {
        readonly DutchContext _context;
        readonly IHostingEnvironment _host;
        readonly ILogger<DutchSeeder> _logger;
        readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext context, IHostingEnvironment host, ILogger<DutchSeeder> logger, UserManager<StoreUser> userManager) 
        {
            _context = context;
            _host = host;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            var user = await _userManager.FindByEmailAsync("pedro@gmail.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Pedro",
                    LastName = "Test",
                    Email = "pedro@gmail.com",
                    UserName = "pedro"
                };

                var saveResult = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (saveResult != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Cannot create user in seeder!");
                }
            }

            if (!_context.Products.Any())
            {
                _logger.LogWarning("No products detected, seeding products");
                var seedDataPath = Path.Combine(_host.ContentRootPath, "Data/art.json");
                var rawData = File.ReadAllText(seedDataPath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(rawData);
                _context.Products.AddRange(products);

                var order = _context.Orders.Where(o => o.Id == 1).FirstOrDefault();
                // Originally the course had the seed order created on DutchContext
                // This led to the order not being inserted for whatever reason
                // this code is a workaround
                if (order == null)
                {
                    _logger.LogWarning("No orders detected, seeding orders");
                    order = new Order()
                    {
                        OrderDate = DateTime.UtcNow,
                        OrderNumber = "12345",
                        User = user
                    };

                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Quantity = 5,
                            UnitPrice = products.First().Price,
                            Product = products.First()
                        }
                    };
                    _context.Orders.Add(order);
                }

                var changes = _context.SaveChanges();
                _logger.LogWarning($"{changes} have been saved to database");
            }
        }

    }
}
