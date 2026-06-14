using Repository.Models;

namespace Repository.OffersRepository;

public interface IOfferRepository
{
    List<Offer> GetOffers();
    void AddOffer(Offer offer);
}
