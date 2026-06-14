using Repository.Models;

namespace Repository.ProductsRepository;

public class ProductRepository : IProductRepository
{
    public void AddProduct(Product product)
    {
        bool isProductAdded = Data.Products.TryAdd(product.SKU, product);

        if (!isProductAdded)
        {
            throw new InvalidOperationException("Product with the same SKU already exists.");
        }
    }

    public Dictionary<string, Product> GetProducts()
    {
        return Data.Products;
    }

    internal void ClearProducts()
    {
        Data.Products.Clear();
    }
}
