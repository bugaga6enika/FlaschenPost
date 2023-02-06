using System;
namespace FlaschenPost.Drinks
{
    public record Price
    {
        private Price()
        {
        }

        public decimal Amount { get; init; }
        public string Currency { get; init; }

        public static Price CreateFromDecimal(decimal price, string currency = "€")
        {
            return new Price { Currency = currency, Amount = price };
        }
    }
}

