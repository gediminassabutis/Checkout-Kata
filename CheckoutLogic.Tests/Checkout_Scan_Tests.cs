namespace CheckoutLogic.Tests;

public class Checkout_Scan_Tests
{
    [Fact]
    public void Scan_SingleItem_ReturnsItemPrice()
    {
        Checkout co = CreateCheckout();
        co.Scan("A");
        Assert.Equal(50, co.GetTotalPrice());
    }

    [Theory]
    [InlineData(new string[] { "A", "B" }, 80)]
    [InlineData(new string[] { "A", "B", "C" }, 100)]
    [InlineData(new string[] { "A", "B", "C", "D" }, 115)]
    public void Scan_MultipleDifferentItems_ReturnsSumOfPrices(string[] items, decimal expectedTotal)
    {
        Checkout co = CreateCheckout();

        for (int i = 0; i < items.Length; i++)
        {
            co.Scan(items[i]);
        }

        Assert.Equal(expectedTotal, co.GetTotalPrice());
    }

    [Fact]
    public void Scan_EmptyCheckout_TotalIsZero()
    {
        Checkout co = CreateCheckout();
        Assert.Equal(0, co.GetTotalPrice());
    }

    [Fact]
    public void Scan_UnknownSKU_ThrowsArgumentException()
    {
        Checkout co = CreateCheckout();

        Assert.Throws<ArgumentException>(() => co.Scan("Z"));
    }

    [Fact]
    public void BulkDiscount_AppliesWhenThresholdReached()
    {
        Checkout co = CreateCheckout();

        co.Scan("A");
        co.Scan("A");
        co.Scan("A");

        Assert.Equal(130, co.GetTotalPrice());
    }

    [Fact]
    public void BulkDiscount_PartialGroupChargedAtUnitPrice()
    {
        Checkout co = CreateCheckout();

        co.Scan("A");
        co.Scan("A");
        co.Scan("A");
        co.Scan("A");

        Assert.Equal(180, co.GetTotalPrice());
    }

    [Fact]
    public void Promotion_TwoOfferItems()
    {
        Checkout co = CreateCheckout();

        co.Scan("B");
        co.Scan("B");

        Assert.Equal(45, co.GetTotalPrice());
    }

    [Fact]
    public void Promotion_OddCountBilling()
    {
        Checkout co = CreateCheckout();

        co.Scan("B");
        co.Scan("B");
        co.Scan("B");

        Assert.Equal(75, co.GetTotalPrice());
    }

    [Fact]
    public void Combine_Promotions_AppliedCorrectly()
    {
        Checkout co = CreateCheckout();

        co.Scan("A");
        co.Scan("A");
        co.Scan("A");
        co.Scan("B");
        co.Scan("B");

        Assert.Equal(175, co.GetTotalPrice());
    }

    [Fact]
    public void BulkDiscount_Multiple_SameProduct_Offers()
    {
        Checkout co = CreateCheckout();

        co.Scan("A");
        co.Scan("A");
        co.Scan("A");
        co.Scan("A");
        co.Scan("A");
        co.Scan("A");

        Assert.Equal(260, co.GetTotalPrice());
    }

    private static Checkout CreateCheckout()
    {
        return new Checkout();
    }
}
