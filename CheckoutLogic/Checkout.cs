using CheckoutLogic.Models;
using Repository.Models;
using Repository.OffersRepository;
using Repository.ProductsRepository;

namespace CheckoutLogic;

public class Checkout(IOfferRepository offersRepository, IProductRepository productRepository) : ICheckout
{
    internal List<BasketItem> _basket = [];

    public decimal GetTotalPrice()
    {
        //reset IsPartOfOffer flag.
        _basket.ForEach(b => b.IsPartOfOffer = false);

        _basket = [.. _basket.OrderBy(b => b.Product.SKU)];

        List<Offer> offers = offersRepository.GetOffers();

        List<string> uniqueProductsInBasket = [.. _basket.GroupBy(p => p.Product.SKU).Select(g => g.Key)];

        List<Offer> availableOffers = [.. offers.Where(o => uniqueProductsInBasket.Contains(o.SKU))];

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
        Dictionary<string, Product> products = productRepository.GetProducts();

        if (!products.TryGetValue(item, out Product? value))
        {
            throw new ArgumentException($"Unknown SKU: {item}");
        }

        _basket.Add(new BasketItem(_basket.Count + 1, value));
    }

    public void ClearBasket()
    {
        _basket.Clear();
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