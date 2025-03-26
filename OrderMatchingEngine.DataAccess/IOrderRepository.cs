
using OrderMatchingEngine.Domain;
using OrderMatchingEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMatchingEngine.DataAccess
{
    public interface IOrderRepository
    {
        Task<int> AddOrder(Order order);
        Task RemoveOrder(int orderId);
        Task<bool> CancelOrder(int orderId);
        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<Order>> GetActiveOrders();
        Task<Order?> GetOrder(int orderId);
        Task<IEnumerable<Order>> GetOppositeSideOrders(OrderType type, decimal price);
        Task AddTrade(Trade trade);
        Task<IEnumerable<Trade>> GetTradeHistory();

    }
}
