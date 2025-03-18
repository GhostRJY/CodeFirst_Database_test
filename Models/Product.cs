using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Contexts;


namespace CodeFirst.Models
{
    
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

        //реализую связь многие к одному (1 категория много товаров)
        //внешние ключи
        public int CategoryId { get; set; }        

        //навигационные свойства
        public Category? Category { get; set; }

        public ICollection<ProductToOrder>? ProductToOrders { get; set; }
    }

    public class ProductRepository
    {
        public void GetAllProducts()
        {
            using var context = new MarketPlaceDBContext();
            var products = context.Products?.ToList();
            foreach(var product in products!)
            {
                Console.WriteLine($"{product.Name} {product.Count} {product.Price} {product.Category?.Name}");
            }
        }
        public void GetProductById(int id)
        {
            using var context = new MarketPlaceDBContext();
            var product = context.Products?.FirstOrDefault(p => p.Id == id);
            if(product == null)
            {
                Console.WriteLine($"Товар с id {id} не найден.");
                return;
            }
            Console.WriteLine($"{product.Name} {product.Count} {product.Price} {product.Category?.Name}");
        }
        public void AddProduct(in Product newProduct)
        {
            using var context = new MarketPlaceDBContext();
            var product = newProduct;
            context.Products?.Add(product);
            context.SaveChanges();
            Console.WriteLine($"Товар {product.Name} добавлен!");
        }

        public void UpdateProduct(string? productName, in Product newProduct)
        {
            using var context = new MarketPlaceDBContext();
            var product = context.Products?.FirstOrDefault(p => p.Name == productName);
            if(product == null)
            {
                Console.WriteLine($"Товар {productName} не найден.");
                return;
            }
            product.Name = newProduct.Name;
            product.Count = newProduct.Count;
            product.Price = newProduct.Price;

            product.CategoryId = newProduct.CategoryId;
            context.SaveChanges();
            Console.WriteLine($"Товар {productName} обновлен!");
        }

        public void DeleteProduct(int id)
        {
            using var context = new MarketPlaceDBContext();
            var product = context.Products?.FirstOrDefault(p => p.Id == id);
            if(product == null)
            {
                Console.WriteLine($"Товар с id {id} не найден.");
                return;
            }
            context.Products?.Remove(product);
            context.SaveChanges();
            Console.WriteLine($"Товар {product.Name} удален!");
        }
    }
}
