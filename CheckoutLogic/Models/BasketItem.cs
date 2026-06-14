using Repository.Models;

namespace CheckoutLogic.Models;

internal class BasketItem(int Id, Product product)
{
    public int Id { get; } = Id;
    public Product Product { get; } = product;
    public bool IsPartOfOffer { get; set; } = false;
}
