using Repository.Models;
using Repository.ProductsRepository;

namespace Repository.Tests
{
    public class ProductRepositoryTests : IDisposable
    {
        private readonly ProductRepository _productRepository;

        public ProductRepositoryTests()
        {
            _productRepository = new ProductRepository();
        }

        [Fact]
        public void GetProducts_InitiallyEmpty()
        {
            Assert.Empty(_productRepository.GetProducts());
        }

        [Fact]
        public void AddProduct_SingleItem_ReturnsItemPrice()
        {
            _productRepository.AddProduct(new Product("A", 50));

            var product = _productRepository.GetProducts().Single();
            Assert.Equal("A", product.Value.SKU);
            Assert.Equal(50, product.Value.UnitPrice);
        }

        [Fact]
        public void AddProduct_MultipleItems_ReturnsAllItems()
        {
            _productRepository.AddProduct(new Product("A", 50));
            _productRepository.AddProduct(new Product("B", 30));

            var products = _productRepository.GetProducts().ToList();

            Assert.Equal(2, products.Count);

            Assert.Contains(products, p => p.Value.SKU == "A");
            Assert.Contains(products, p => p.Value.SKU == "B");
        }

        [Fact]
        public void AddProduct_DuplicateProduct_ThrowsInvalidOperationException()
        {
            _productRepository.AddProduct(new Product("A", 50));

            Assert.Throws<InvalidOperationException>(() => _productRepository.AddProduct(new Product("A", 50)));
        }

        public void Dispose()
        {
            _productRepository.ClearProducts();
        }
    }
}
