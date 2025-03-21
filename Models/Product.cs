
using CodeFirst.Contexts;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Models
{
    
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано имя товара!")]
        public string? Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Недопустимое значение кол-ва товара!")]
        public int Count { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Недопустимое значение цены товара!")]
        public decimal Price { get; set; }

        //реализую связь многие к одному (1 категория много товаров)
        //внешние ключи
        public int CategoryId { get; set; }        

        //навигационные свойства
        public Category? Category { get; set; }

        public ICollection<ProductToOrder>? ProductToOrders { get; set; }
    }

    //класс для хранимой процедуры (Названия полей должны совпадать с названиями в ХП) 
    public class CategoriesOfProduct
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CategoryName { get; set; }
    }

    public class ProductRepository
    {
      

        public void ExecStoredProcedure(in string procedureName)
        {
            using var context = new MarketPlaceDBContext();
            var caterories = context.Set<CategoriesOfProduct>()
                .FromSqlRaw($"EXEC {procedureName}")  // вызов хранимой процедуры
                .ToList();

            caterories.ForEach(record =>
            {
                Console.WriteLine($"{record.Name} категория: {record.CategoryName}");
            });



        }

       
        public void GetAllProducts()
        {
            var context = new MarketPlaceDBContext();
            var products = context.Products?.Include(c => c.Category);

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
        public void AddProduct(in string name, in int count, in double price, in int categoryId )
        {
            using var context = new MarketPlaceDBContext();
            ProductBuilder productBuild = new ProductBuilder();

            var product = productBuild.SetName(name).SetCount(count).SetPrice(price).SetCategory(categoryId).Build();

            // валидация продукта
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(product, null, null);
            bool isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);

            if(!isValid)
            {
                Console.WriteLine("Ошибка валидации:");
                foreach(var validationResult in validationResults)
                {
                    Console.WriteLine(validationResult.ErrorMessage);
                }
                return;
            }

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

    //реализую строитель Product (Fluent API)
    public class ProductBuilder
    {
        private Product m_product = new Product();

        public ProductBuilder SetName(string name)
        {
            m_product.Name = name;
            return this;
        }

        public ProductBuilder SetPrice(double price)
        {
            m_product.Price = (decimal)price;
            return this;
        }

        public ProductBuilder SetCount(int count)
        {
            m_product.Count = count;
            return this;
        }

        public ProductBuilder SetCategory(int categoryId)
        {
            m_product.CategoryId = categoryId;
            return this;
        }

        public Product Build()
        {
            return m_product;
        }

    }
}
