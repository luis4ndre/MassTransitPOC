﻿namespace Events
{
    public interface ILoanEvent
    {
        public Guid OrderId { get; }
        public string? Client { get; }
        public decimal Amount { get; }
        public decimal Limit { get; }
    }
}
