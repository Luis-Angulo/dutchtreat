using System.Collections.Generic;
using System.Linq;
using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DutchTreat.Data
{
    // ctrl + . brings up fixes
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _context;
        public DutchRepository(DutchContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            var data = includeItems ?
                _context.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .ToList() :
                _context.Orders
                    .ToList();
            return data;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.OrderBy(p => p.Title).ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _context.Products
                .Where(p => p.Category == category)
                .OrderBy(p => p.Title)
                .ToList();
        }
        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public Order GetOrderById(string userName, int id)
        {
            return _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.Id == id && o.User.UserName == userName)
                .FirstOrDefault();
        }

        public void AddEntity(object data)
        {
            _context.Add(data);
        }

        public IEnumerable<Order> GetAllOrdersByUser(string userName, bool includeItems)
        {
            var data = includeItems ?
                _context.Orders
                    .Where(o => o.User.UserName == userName)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .ToList() :
                _context.Orders
                    .Where(o => o.User.UserName == userName)
                    .ToList();
            return data;
        }
    }
}
