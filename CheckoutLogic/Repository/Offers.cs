using CheckoutLogic.Models;

namespace CheckoutLogic.Repository;

internal static class Offers
{
    internal static readonly List<Offer> ProductOffers = new()
    {
        { new Offer("A", 3, 130) }, // Buy 3 A's for 130
        { new Offer("B", 2, 45) }    // Buy 2 B's for 45
    };
}
