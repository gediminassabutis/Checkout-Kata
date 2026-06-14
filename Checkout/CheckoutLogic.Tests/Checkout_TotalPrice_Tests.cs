using Moq;
using Repository.Models;
using Repository.OffersRepository;
using Repository.ProductsRepository;

namespace CheckoutLogic.Tests;

public class Checkout_TotalPrice_Tests
{
    private readonly Checkout _checkout;

    public Checkout_TotalPrice_Tests()
    {
        Mock<IProductRepository> productRepositoryMock = new();
        Mock<IOfferRepository> offersRepositoryMock = new();

        productRepositoryMock
            .Setup(p => p.GetProducts())
            .Returns(new Dictionary<string, Product>
            {
            { "A", new Product("A", 50) },
            { "B", new Product("B", 30) },
            { "C", new Product("C", 20) },
            { "D", new Product("D", 15) }
            });

        offersRepositoryMock
            .Setup(o => o.GetOffers())
            .Returns(
            [
                new Offer("A", 3, 130),
                new Offer("B", 2, 45)
            ]);

        _checkout = new Checkout(offersRepositoryMock.Object, productRepositoryMock.Object);
    }

    // Verifies incremental scanning updates the running total and that quantity offers are applied (A: 3 for 130, B: 2 for 45).
    [Fact]
    public void Scan_MultipleItems_ReturnsSumOfPrices()
    {
        _checkout.Scan("A");

        Assert.Equal(50, _checkout.GetTotalPrice());

        _checkout.Scan("B");
        Assert.Equal(80, _checkout.GetTotalPrice());

        _checkout.Scan("A");
        _checkout.Scan("A");

        Assert.Equal(160, _checkout.GetTotalPrice());
    }
}
