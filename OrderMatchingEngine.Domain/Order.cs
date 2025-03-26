using OrderMatchingEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMatchingEngine.Domain
{
    public class Order
    {
        public int OrderId { get; set; }
        public OrderType Type { get; }
        public decimal Price { get; }
        public int Quantity { get; }
        public int RemainingQuantity { get; private set; }
        public DateTime Timestamp { get; }
        public OrderStatus Status { get; set; }

        public Order(  OrderType type, decimal price, int quantity)
        { 
            Type = type;
            Price = price;
            Quantity = quantity;
            RemainingQuantity = quantity;
            Timestamp = DateTime.UtcNow;
            Status = OrderStatus.New;
        }

        public void UpdateRemainingQuantity(int filledQuantity)
        {
            RemainingQuantity -= filledQuantity;
            Status = RemainingQuantity == 0 ? OrderStatus.Filled : OrderStatus.PartiallyFilled;
        }
         

        public void Cancel()
        {
            Status = OrderStatus.Cancelled;
            RemainingQuantity = 0;
        }
    }
}

