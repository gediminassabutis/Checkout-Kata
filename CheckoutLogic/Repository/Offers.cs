namespace CheckoutLogic.Repository;

internal static class Offers
{
    internal static readonly Dictionary<string, (int Quantity, int Price)> ProductOffers = new()
    {
        { "A", (3, 130) }, // Buy 3 A's for 130
        { "B", (2, 45) }   // Buy 2 B's for 45
    };
}
