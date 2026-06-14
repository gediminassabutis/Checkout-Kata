using Repository.Models;

namespace Repository.OffersRepository;

public class OfferRepository : IOfferRepository
{
    public void AddOffer(Offer offer)
    {
        // Check if the offer already exists in the list
        if (Data.Offers.Any(o => o.SKU == offer.SKU && o.Count == offer.Count && o.Price == offer.Price))
        {
            throw new InvalidOperationException("Offer already exists.");
        }

        Data.Offers.Add(offer);
    }

    public List<Offer> GetOffers()
    {
        return Data.Offers;
    }

    internal void ClearOffers()
    {
        Data.Offers.Clear();
    }
}
