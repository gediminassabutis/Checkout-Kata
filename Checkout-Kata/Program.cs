using CheckoutLogic;
using Repository.Models;
using Repository.OffersRepository;
using Repository.ProductsRepository;
using System.Globalization;

ProductRepository productRepo = new();
OfferRepository offerRepo = new();

Checkout checkout = new(offerRepo, productRepo);

Console.WriteLine("Checkout Commands:");
Console.WriteLine("  scan <SKU> count           - Scan/add one units of the product with the specified SKU into the current basket.");
Console.WriteLine("  total                      - Calculate and display the basket total (applies any configured offers/discounts).");
Console.WriteLine("  clear                      - Remove all items from the current basket.");

Console.WriteLine("Product Commands:");
Console.WriteLine("  addProduct <SKU> <price>   - Register a product with SKU and unit price (price as decimal, e.g. 1.99).");

Console.WriteLine("Offer Commands:");
Console.WriteLine("  addOffer <SKU> <count> <price> - Add a bulk offer: when buying <count> units of SKU the total price becomes <price>.");

Console.WriteLine("Other Commands:");
Console.WriteLine("  exit                       - Exit the application.");

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) continue;

    var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var cmd = parts[0].ToLowerInvariant();

    if (cmd == "exit") break;

    if (cmd == "scan")
    {
        var sku = parts[1].ToUpperInvariant();
        var count = parts[2].ToUpperInvariant();
        try
        {
            for (int i = 0; i < int.Parse(count); i++)
            {
                checkout.Scan(sku);
            }

            Console.WriteLine($"Scanned {count} {sku}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        continue;
    }

    if (cmd == "total")
    {
        Console.WriteLine($"Total: {checkout.GetTotalPrice()}");
        continue;
    }

    if (cmd == "clear")
    {
        checkout.ClearBasket();
        Console.WriteLine("Basket cleared.");
        continue;
    }

    // Handle add product commands:
    if (cmd == "addproduct")
    {
        if (parts.Length == 3)
        {
            // Product: addProduct <SKU> <price>
            string sku = parts[1].ToUpperInvariant();

            if (!decimal.TryParse(parts[2], NumberStyles.Number, CultureInfo.InvariantCulture, out var price))
            {
                Console.WriteLine("Invalid price format.");
                continue;
            }

            productRepo.AddProduct(new Product(sku, price));

            Console.WriteLine($"Product {sku} added at {price.ToString(CultureInfo.InvariantCulture)}");
            continue;
        }

        Console.WriteLine("Unknown add command format.");
        continue;
    }

    // Handle add offer commands:
    if (cmd == "addoffer")
    {
        if (parts.Length == 4)
        {
            // Offer: addOffer <SKU> <count> <price>
            string sku = parts[1].ToUpperInvariant();
            if (!int.TryParse(parts[2], out var count))
            {
                Console.WriteLine("Invalid count format.");
                continue;
            }
            if (!decimal.TryParse(parts[3], NumberStyles.Number, CultureInfo.InvariantCulture, out var price))
            {
                Console.WriteLine("Invalid price format.");
                continue;
            }
            offerRepo.AddOffer(new Offer(sku, count, price));
            Console.WriteLine($"Offer for {sku} added: {count} for {price.ToString(CultureInfo.InvariantCulture)}");
            continue;
        }
        Console.WriteLine("Unknown add command format.");
        continue;
    }

    Console.WriteLine("Unknown command");
}
