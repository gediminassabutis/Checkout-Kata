using Repository.Models;

namespace Repository.ProductsRepository;

public interface IProductRepository
{
    Dictionary<string, Product> GetProducts();
    void AddProduct(Product product);
}
