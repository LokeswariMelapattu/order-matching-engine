
using System;
using System.Threading.Tasks;
using OrderMatchingEngine.Domain;
using OrderMatchingEngine.Domain.Enums;
using OrderMatchingEngine.DataAccess;
using OrderMatchingEngine.ApplicationLogic;
using System.Diagnostics;

namespace OrderMatchingEngine.ConsoleApp
{
    public class OrderMatchingApp
    {
        private readonly OrderMatchingService _orderMatchingService;

        public OrderMatchingApp()
        {
            var repository = new OrderRepository();
            _orderMatchingService = new OrderMatchingService(repository);
        }

        public async Task Run()
        {
            Console.WriteLine("Welcome to the Order Matching Engine!");
            while (true)
            {
                Console.WriteLine("\n Choose an action:");
                Console.WriteLine("1. Place Order");
                Console.WriteLine("2. Cancel Order");
                Console.WriteLine("3. View Active Orders");
                Console.WriteLine("4. View Trade History");
                Console.WriteLine("5. Exit");
                Console.Write("\nAction: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await PlaceOrder();
                        break;
                    case "2":
                        await CancelOrder();
                        break;
                    case "3":
                        await ViewActiveOrders();
                        break;
                    case "4":
                        await ViewTradeHistory();
                        break;
                    case "5":
                        Console.WriteLine("Exiting... bye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        private async Task PlaceOrder()
        {
            try
            {
                //Console.Write("Enter Order ID: ");
                //int orderId = int.Parse(Console.ReadLine());

                Console.Write("Enter Type (Buy/Sell): ");
                string typeInput = Console.ReadLine();
                if (!typeInput.Equals("Buy", StringComparison.OrdinalIgnoreCase) && !typeInput.Equals("Sell", StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Invalid Order Type");
                OrderType type = typeInput.Equals("Buy", StringComparison.OrdinalIgnoreCase)
                        ? OrderType.Buy
                        : OrderType.Sell;


                Console.Write("Enter Price: ");
                decimal price = decimal.Parse(Console.ReadLine());

                Console.Write("Enter Quantity: ");
                int quantity = int.Parse(Console.ReadLine());

                var order = new Order(type, price, quantity);
               var orderId= await _orderMatchingService.PlaceOrder(order);
                Console.WriteLine($"Order {orderId} placed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to place order: {ex.Message}");
            }
        }

        private async Task CancelOrder()
        {
            try
            {
                Console.Write("Enter Order ID to cancel: ");
                int orderId = int.Parse(Console.ReadLine());

                var result = await _orderMatchingService.CancelOrder(orderId);
                if (result)
                    Console.WriteLine($"Order {orderId} canceled successfully.");
                else
                    Console.WriteLine($"Order {orderId} not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to cancel order: {ex.Message}");
            }
        }

        private async Task ViewActiveOrders()
        {
            var orders = await _orderMatchingService.GetAllOrders();
            if (!orders.Any())
            {
                Console.WriteLine("No active orders.");
                return;
            }

            Console.WriteLine("Active Orders:");
            Console.WriteLine("\n===================================================================================");
            Console.WriteLine("| Order ID | Type  | Price   | Quantity | Remaining Quantity | Status              |");
            Console.WriteLine("====================================================================================");

            foreach (var order in orders)
            {
                Console.WriteLine($"| {order.OrderId,-8} | {order.Type,-5} | {order.Price,-7:C} | {order.Quantity,-8} | {order.RemainingQuantity,-18} | {order.Status,-19} |");
            }

            Console.WriteLine("====================================================================================");
        }

        private async Task ViewTradeHistory()
        {
            var trades = await _orderMatchingService.GetTradeHistory();
            if (!trades.Any())
            {
                Console.WriteLine("No active trades.");
                return;
            }

            Console.WriteLine("Trade History:");
            Console.WriteLine("\n========================================================");
            Console.WriteLine("| Buyer Order ID | Seller Order ID | Price    | Quantity |");
            Console.WriteLine("==========================================================");

            foreach (var trade in trades)
            {
                Console.WriteLine($"| {trade.BuyOrderId,-14} | {trade.SellOrderId,-15} | {trade.Price,-8:C} | {trade.Quantity,-8} |");
            }
            Console.WriteLine("==========================================================");
        }
    }
}

