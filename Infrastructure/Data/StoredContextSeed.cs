using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoredContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {
                var brandData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                brands.ForEach(e =>
                {
                    context.ProductBrands.Add(e);
                });
                await context.SaveChangesAsync();
            }

            if (!context.ProductTypes.Any())
            {
                var productTypeData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                var productTypes = JsonSerializer.Deserialize<List<ProductType>>(productTypeData);
                productTypes.ForEach(e =>
                {
                    context.ProductTypes.Add(e);
                });
                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                var productData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);
                products.ForEach(e =>
                {
                    context.Products.Add(e);
                });
                await context.SaveChangesAsync();
            }
        }
    }
}