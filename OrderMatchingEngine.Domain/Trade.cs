using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMatchingEngine.Domain
{
    public class Trade
    {
        public int TradeId { get; set; }
        public int BuyOrderId { get; }
        public int SellOrderId { get; }
        public decimal Price { get; }
        public int Quantity { get; }
        public DateTime TradeTime { get; }

        public Trade(int buyOrderId, int sellOrderId, decimal price, int quantity)
        {
            BuyOrderId = buyOrderId;
            SellOrderId = sellOrderId;
            Price = price;
            Quantity = quantity;
            TradeTime = DateTime.UtcNow;
        }
    }
}