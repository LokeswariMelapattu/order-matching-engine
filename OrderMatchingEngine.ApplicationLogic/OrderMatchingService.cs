using OrderMatchingEngine.DataAccess;
using OrderMatchingEngine.Domain;
using OrderMatchingEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMatchingEngine.ApplicationLogic
{
    public class OrderMatchingService : IOrderMatchingService
    {
        private readonly object _lock = new object(); // For thread safety
        private IOrderRepository _orderRepository;

        public OrderMatchingService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public Task<int> PlaceOrder(Order order)
        {
            try
            {
                lock (_lock)  // Ensure thread safety
                {
                  var result =  _orderRepository.AddOrder(order); //add to order book
                    var oppositeSideOrders = _orderRepository.GetOppositeSideOrders(order.Type, order.Price).Result;

                    foreach (var oppositeOrder in oppositeSideOrders)
                    {
                        if (CanMatch(order, oppositeOrder))
                        {
                            var trade = MatchOrder(order, oppositeOrder);
                            _orderRepository.AddTrade(trade);
                            Console.WriteLine($"Trade: {trade.BuyOrderId} matches {trade.SellOrderId}, Quantity: {trade.Quantity}");
                        }
                        if (oppositeOrder.RemainingQuantity == 0)
                            _orderRepository.RemoveOrder(oppositeOrder.OrderId); //delete opposite order when its remining quantity becomes 0
                        if (order.RemainingQuantity == 0) break;
                    }
                    if (order.RemainingQuantity == 0) //Remove from order book if its remaining quantity becomes 0
                        _orderRepository.RemoveOrder(result.Result);
                    // Console.WriteLine($"Order {order.OrderId} placed.");
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while placing an order:");
                Console.WriteLine(ex.Message);
                throw;
            } 
        }

        public Task<bool> CancelOrder(int orderId)
        {
            try
            {
                lock (_lock)
                {
                    var order = _orderRepository.GetOrder(orderId).Result;
                    if (order != null && (order.Status == OrderStatus.New || order.Status == OrderStatus.PartiallyFilled))
                    {
                        order.Status = OrderStatus.Cancelled;
                        var result = _orderRepository.CancelOrder(orderId).Result;
                        // Console.WriteLine($"Order {orderId} has been cancelled.");
                        return Task.FromResult(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while cancelling an order:");
                Console.WriteLine(ex.Message);
                throw;
            }
            return Task.FromResult(false);
        }
        public async Task<IEnumerable<Order>> GetActiveOrders()
        {
            try
            {
                return await _orderRepository.GetActiveOrders();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while fetching active orders:");
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            try
            {
                return await _orderRepository.GetAllOrders();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while fetching all orders:");
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<Order?> GetOrder(int orderId)
        {
            try
            {
                return await _orderRepository.GetOrder(orderId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while fetching all orders:");
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<IEnumerable<Trade>> GetTradeHistory()
        {
            try
            {
                return await _orderRepository.GetTradeHistory();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while fetching trade histories:");
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        private bool CanMatch(Order order, Order oppositeOrder)
        {
            return (order.Type == OrderType.Buy && oppositeOrder.Type == OrderType.Sell && order.Price >= oppositeOrder.Price) ||
                   (order.Type == OrderType.Sell && oppositeOrder.Type == OrderType.Buy && order.Price <= oppositeOrder.Price);
        }

        private Trade MatchOrder(Order order, Order oppositOrder)
        {
            var tradeQuantity = Math.Min(order.RemainingQuantity, oppositOrder.RemainingQuantity);
            order.UpdateRemainingQuantity(tradeQuantity);
            oppositOrder.UpdateRemainingQuantity(tradeQuantity);
            return new Trade((order.Type == OrderType.Buy ? order.OrderId : oppositOrder.OrderId),
                (order.Type == OrderType.Buy ? oppositOrder.OrderId : order.OrderId), order.Price, tradeQuantity);

        }

    }
}

