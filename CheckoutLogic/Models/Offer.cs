namespace CheckoutLogic.Models;

public class Offer
{
    public required string SKU { get; set; }
    public int Count { get; set; }
    public double Price { get; set; }
}
