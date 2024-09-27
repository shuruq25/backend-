using Microsoft.EntityFrameworkCore;
using src.Database;
using src.Entity;

namespace src.Repository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> AddOrderAsync(Order newOrder);
        Task<bool> UpdateOrderAsync(int id, Order order);
        Task<bool> DeleteOrderAsync(int id);
    }

    public class OrderRepository : IOrderRepository
    {
        protected readonly DatabaseContext _db;
        private DbSet<Order> _orders => _db.Order;
        public OrderRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<Order>> GetAllOrdersAsync() => await _orders.ToListAsync();

        public async Task<Order> AddOrderAsync(Order newOrder)
        {
            var result = await _orders.AddAsync(newOrder);
            await _db.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> UpdateOrderAsync(int id, Order updatedOrder)
        {
            var existingOrder = await _orders.FirstOrDefaultAsync(o => o.ID == id);
            if (existingOrder == null)
            {
                return false;
            }
            _orders.Entry(existingOrder).CurrentValues.SetValues(updatedOrder);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var existingOrder = await _orders.FirstOrDefaultAsync(o => o.ID == id);
            if (existingOrder == null)
            {
                return false;
            }
            _orders.Remove(existingOrder);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}