﻿using EstanteMania.MessageBus;

namespace EstanteMania.API.Messages
{
    public class PaymentDTO : BaseMessage
    {
        public long OrderId { get; set; }
        public string Name { get; set; } = null!;
        public string CardNumber { get; set; } = null!;
        public string CVV { get; set; } = null!;
        public string ExpireMonthYear { get; set; } = null!;
        public decimal PurchaseAmount { get; set; }
        public string Email { get; set; } = null!;
    }
}
