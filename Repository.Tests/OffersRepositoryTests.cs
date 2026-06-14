using Repository.Models;
using Repository.OffersRepository;

namespace Repository.Tests;

public class OffersRepositoryTests : IDisposable
{
    private readonly OfferRepository offerRepository;

    public OffersRepositoryTests()
    {
        offerRepository = new OfferRepository();
    }

    [Fact]
    public void GetOffers_InitiallyEmpty()
    {
        Assert.Empty(offerRepository.GetOffers());
    }

    [Fact]
    public void AddOffer_AddsOfferToRepository()
    {
        Offer offer = new("A", 3, 130);

        offerRepository.AddOffer(offer);

        List<Offer> offers = offerRepository.GetOffers();

        Assert.Single(offers);
        Assert.Equal(offer, offers[0]);
    }

    [Fact]
    public void AddOffer_ThrowsInvalidOperationException_WhenOfferAlreadyExists()
    {
        Offer offer = new("A", 3, 130);
        offerRepository.AddOffer(offer);

        Assert.Throws<InvalidOperationException>(() => offerRepository.AddOffer(offer));
    }

    public void Dispose()
    {
        offerRepository.ClearOffers();
    }
}
