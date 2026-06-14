namespace CheckoutLogic.Models;

public class Offer(string SKU, int Count, decimal Price)
{
    public string SKU { get; } = SKU;
    public int Count { get; } = Count;
    public decimal Price { get; } = Price;
}
