using OrderMatchingEngine.Domain.Enums;
using OrderMatchingEngine.Domain;

namespace OrderMatchingEngine.DataAccess
{
    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();
        private readonly List<Trade> _tradeHistory = new();


        public Task<int> AddOrder(Order order)
        {
            order.OrderId = _orders.Count == 0 ? 1 : _orders.Max(x => x.OrderId) + 1;
            _orders.Add(order);
            return Task.FromResult(order.OrderId);
        }
        public Task<bool> CancelOrder(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                order.Status = OrderStatus.Cancelled;
                _orders.Remove(order);
            }
            else
                return Task.FromResult(false);
            return Task.FromResult(true);
        }
        public Task RemoveOrder(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
                _orders.Remove(order);
            else
                return Task.FromResult(false);
            return Task.FromResult(true);
        }
        public Task<Order?> GetOrder(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            return Task.FromResult(order);
        }
        public Task<IEnumerable<Order>> GetAllOrders()
        {
            return Task.FromResult<IEnumerable<Order>>(_orders.OrderBy(o => o.OrderId));
        }
        public Task<IEnumerable<Order>> GetActiveOrders()
        {
            var orders = _orders.Where(o => o.Status == OrderStatus.New || o.Status == OrderStatus.PartiallyFilled)
                                .OrderBy(o => o.OrderId);
            return Task.FromResult<IEnumerable<Order>>(orders);
        }
        public Task<IEnumerable<Order>> GetOppositeSideOrders(OrderType type, decimal price)
        {
            var orders = _orders
                .Where(o => o.Type == (type == OrderType.Buy ? OrderType.Sell : OrderType.Buy)
                        && (o.Status == OrderStatus.New || o.Status == OrderStatus.PartiallyFilled) && o.Price >= price)
                .OrderByDescending(o => o.Price)
                .ThenBy(o => o.OrderId); 
            return Task.FromResult<IEnumerable<Order>>(orders);
        }
        public Task AddTrade(Trade trade)
        {
            trade.TradeId = _tradeHistory.Count == 0 ? 1 : _tradeHistory.Max(x => x.TradeId) + 1; 
            _tradeHistory.Add(trade);
            return Task.CompletedTask;
        }
        public Task<IEnumerable<Trade>> GetTradeHistory()
        {
            return Task.FromResult<IEnumerable<Trade>>(_tradeHistory);
        }
    }
}
