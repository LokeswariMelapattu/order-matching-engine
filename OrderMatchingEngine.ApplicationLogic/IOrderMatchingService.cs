using OrderMatchingEngine.Domain;

namespace OrderMatchingEngine.ApplicationLogic
{
    public interface IOrderMatchingService
    {
        Task<int> PlaceOrder(Order order);
        Task<bool> CancelOrder(int orderId);
        Task<IEnumerable<Order>> GetActiveOrders();
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order?> GetOrder(int orderId);
        Task<IEnumerable<Trade>> GetTradeHistory();

    }
}
