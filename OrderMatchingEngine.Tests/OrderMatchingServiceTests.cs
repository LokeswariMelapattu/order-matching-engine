using OrderMatchingEngine.ApplicationLogic;
using OrderMatchingEngine.DataAccess;
using OrderMatchingEngine.Domain;
using OrderMatchingEngine.Domain.Enums;
using Xunit;
using FluentAssertions;
namespace OrderMatchingEngine.Tests
{
    public class OrderMatchingServiceTests
    {
        private readonly OrderMatchingService _orderMatchingService;

        public OrderMatchingServiceTests()
        {
            var repository = new OrderRepository();
            _orderMatchingService = new OrderMatchingService(repository);
        }

        /// <summary>
        /// Single order placement (no match)
        /// </summary>
        [Fact(DisplayName= "Single order placement (no match)")] 
        public async Task PlaceOrder_SingleOrder_NoMatch()
        {
            var order = new Order(OrderType.Buy, 50.0m, 100);
            await _orderMatchingService.PlaceOrder(order);

            var orders = await _orderMatchingService.GetAllOrders();
            orders.Should().ContainSingle()
                  .Which.Should().BeEquivalentTo(order);
        }

        /// <summary>
        /// Fully matched order
        /// </summary>
        [Fact(DisplayName = "Fully matched order")]
        public async Task PlaceOrder_FullyMatched()
        {
            var buyOrder = new Order(OrderType.Buy, 50.0m, 100);
            var sellOrder = new Order(OrderType.Sell, 50.0m, 100);

            await _orderMatchingService.PlaceOrder(buyOrder);
            await _orderMatchingService.PlaceOrder(sellOrder);

            var orders = await _orderMatchingService.GetAllOrders();
            orders.Should().BeEmpty(); // Both should be removed after full match

            var trades = await _orderMatchingService.GetTradeHistory();
            trades.Should().ContainSingle();
            trades.First().Quantity.Should().Be(100);
        }

        /// <summary>
        /// Partially matched order
        /// </summary>
        [Fact(DisplayName = "Partially matched order")]
        public async Task PlaceOrder_PartiallyMatched()
        {
            var buyOrder = new Order(OrderType.Buy, 50.0m, 150);
            var sellOrder = new Order(OrderType.Sell, 50.0m, 100);

            await _orderMatchingService.PlaceOrder(buyOrder);
            await _orderMatchingService.PlaceOrder(sellOrder);

            var remainingBuyOrder = await _orderMatchingService.GetOrder(1);
            remainingBuyOrder?.Status.Should().Be(OrderStatus.PartiallyFilled);
            remainingBuyOrder?.RemainingQuantity.Should().Be(50); // 100 matched, 50 remains
        }

        /// <summary>
        /// Canceling an order before match
        /// </summary>
        [Fact(DisplayName = "Canceling an order before match")]
        public async Task CancelOrder_OrderCancelled()
        {
            var order = new Order(OrderType.Buy, 50.0m, 100);
            await _orderMatchingService.PlaceOrder(order);
            await _orderMatchingService.CancelOrder(1);

            var orders = await _orderMatchingService.GetAllOrders();
            orders.Should().BeEmpty();
        }

        /// <summary>
        /// Complex scenario with multiple orders
        /// </summary>
        [Fact(DisplayName = "Complex scenario with multiple orders")]
        public async Task PlaceOrder_ComplexMatchingScenario()
        {
            await _orderMatchingService.PlaceOrder(new Order(OrderType.Buy, 50.0m, 100));
            await _orderMatchingService.PlaceOrder(new Order(OrderType.Buy, 55.0m, 200));
            await _orderMatchingService.PlaceOrder(new Order(OrderType.Sell, 49.0m, 150));
            await _orderMatchingService.PlaceOrder(new Order(OrderType.Sell, 50.0m, 100));
            await _orderMatchingService.PlaceOrder(new Order(OrderType.Buy, 48.0m, 50));

            var trades = await _orderMatchingService.GetTradeHistory();
            trades.Count().Should().Be(3);

            var remainingOrders = await _orderMatchingService.GetAllOrders();
            remainingOrders.Count().Should().Be(2); // One order should remain partially filled
        }
    }
}
