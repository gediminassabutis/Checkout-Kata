namespace CheckoutLogic;

internal interface ICheckout
{
    public void Scan(string item);
    public decimal GetTotalPrice();
}
