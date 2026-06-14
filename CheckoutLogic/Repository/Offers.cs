using CheckoutLogic.Models;

namespace CheckoutLogic.Repository;

internal static class Offers
{
    internal static readonly Dictionary<string, Offer> ProductOffers = new()
    {
        { "A", new Offer("A", 3, 130) }, // Buy 3 A's for 130
        { "B", new Offer("B", 2, 45) }    // Buy 2 B's for 45
    };
}
