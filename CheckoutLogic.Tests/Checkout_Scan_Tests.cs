using Moq;
using Repository.Models;
using Repository.OffersRepository;
using Repository.ProductsRepository;

namespace CheckoutLogic.Tests;

public class Checkout_Scan_Tests : IDisposable
{
    private readonly Checkout _checkout;

    public Checkout_Scan_Tests()
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

    [Fact]
    public void Scan_SingleItem_ReturnsItemPrice()
    {
        _checkout.Scan("A");
        Assert.Equal(50, _checkout.GetTotalPrice());
    }

    [Theory]
    [InlineData(new string[] { "A", "B" }, 80)]
    [InlineData(new string[] { "A", "B", "C" }, 100)]
    [InlineData(new string[] { "A", "B", "C", "D" }, 115)]
    public void Scan_MultipleDifferentItems_ReturnsSumOfPrices(string[] items, decimal expectedTotal)
    {
        for (int i = 0; i < items.Length; i++)
        {
            _checkout.Scan(items[i]);
        }

        Assert.Equal(expectedTotal, _checkout.GetTotalPrice());
    }

    [Fact]
    public void Scan_EmptyCheckout_TotalIsZero()
    {
        Assert.Equal(0, _checkout.GetTotalPrice());
    }

    [Fact]
    public void Scan_UnknownSKU_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _checkout.Scan("Z"));
    }

    [Fact]
    public void BulkDiscount_AppliesWhenThresholdReached()
    {
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");

        Assert.Equal(130, _checkout.GetTotalPrice());
    }

    [Fact]
    public void BulkDiscount_PartialGroupChargedAtUnitPrice()
    {
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");

        Assert.Equal(180, _checkout.GetTotalPrice());
    }

    [Fact]
    public void Promotion_TwoOfferItems()
    {
        _checkout.Scan("B");
        _checkout.Scan("B");

        Assert.Equal(45, _checkout.GetTotalPrice());
    }

    [Fact]
    public void Promotion_OddCountBilling()
    {
        _checkout.Scan("B");
        _checkout.Scan("B");
        _checkout.Scan("B");

        Assert.Equal(75, _checkout.GetTotalPrice());
    }

    [Fact]
    public void Combine_Promotions_AppliedCorrectly()
    {
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("B");
        _checkout.Scan("B");

        Assert.Equal(175, _checkout.GetTotalPrice());
    }

    [Fact]
    public void BulkDiscount_Multiple_SameProduct_Offers()
    {
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");
        _checkout.Scan("A");

        Assert.Equal(260, _checkout.GetTotalPrice());
    }

    public void Dispose()
    {
        //clear the basket after each test to ensure isolation
        _checkout.ClearBasket();
    }
}
