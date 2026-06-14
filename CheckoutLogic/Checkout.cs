using CheckoutLogic.Models;
using CheckoutLogic.Repository;

namespace CheckoutLogic;

public class Checkout : ICheckout
{
    internal List<BasketItem> _basket = [];

    public decimal GetTotalPrice()
    {
        _basket = [.. _basket.OrderBy(b => b.Product.SKU)];

        List<string> uniqueProductsInBasket = [.. _basket.GroupBy(p => p.Product.SKU).Select(g => g.Key)];

        List<Offer> availableOffers = [.. Offers.ProductOffers.Where(o => uniqueProductsInBasket.Contains(o.SKU))];

        decimal totalPrice = 0;

        for (int i = 0; i < availableOffers.Count; i++)
        {
            Offer offer = availableOffers[i];

            while (true)
            {
                //Find multiple offers for the same product in the basket and apply them until no more can be applied
                if (!TryApplyOffer(offer))
                {
                    break;
                }

                totalPrice += offer.Price;
            }
        }

        totalPrice += _basket.Where(b => !b.IsPartOfOffer).Sum(b => b.Product.UnitPrice);

        return totalPrice;
    }

    public void Scan(string item)
    {
        if (!Products.ProductPrices.TryGetValue(item, out Product? value))
        {
            throw new ArgumentException($"Unknown SKU: {item}");
        }

        _basket.Add(new BasketItem(_basket.Count + 1, value));
    }

    private bool TryApplyOffer(Offer offer)
    {
        List<BasketItem> items = [.. _basket.Where(b => b.Product.SKU == offer.SKU && !b.IsPartOfOffer)];

        if (offer.Count <= items.Count)
        {
            for (int i = 0; i < offer.Count; i++)
            {
                items[i].IsPartOfOffer = true;
            }
            return true;
        }
        return false;
    }
}