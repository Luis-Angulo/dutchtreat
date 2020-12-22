using System.Collections.Generic;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {

        IEnumerable<Product> GetAllProducts();
        IEnumerable<Order> GetAllOrders(bool includeItems);
        IEnumerable<Product> GetProductsByCategory(string category);
        Order GetOrderById(int id);
        bool SaveAll();
        void AddEntity(object data);
    }
}