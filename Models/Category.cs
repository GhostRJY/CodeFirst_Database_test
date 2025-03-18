using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Contexts;

namespace CodeFirst.Models
{
    public class Category
    {
        //первичный ключ
        public int Id { get; set; }
        public string Name { get; set; }

        //связь один ко многим
        public ICollection<Product>? Products { get; set; }
    }

    public class CategoryRepository
    {
        public void GetAllCategories()
        {
            using var context = new MarketPlaceDBContext();
            var categories = context.Categories?.ToList();
            foreach(var category in categories!)
            {
                Console.WriteLine($"{category.Name}");
            }
        }
        public void GetCategoryById(int id)
        {
            using var context = new MarketPlaceDBContext();
            var category = context.Categories?.FirstOrDefault(c => c.Id == id);
            if(category == null)
            {
                Console.WriteLine($"Категория с id {id} не найдена.");
                return;
            }
            Console.WriteLine($"{category.Name}");
        }
        public void AddCategory(in Category newCategory)
        {
            using var context = new MarketPlaceDBContext();
            var category = newCategory;
            context.Categories?.Add(category);
            context.SaveChanges();
            Console.WriteLine($"Категория {category.Name} добавлена!");
        }
        public void UpdateCategory(int categoryId, in Category newCategory)
        {
            using var context = new MarketPlaceDBContext();
            var category = context.Categories?.FirstOrDefault(c => c.Id == categoryId);
            if(category == null)
            {
                Console.WriteLine($"Категория с id {categoryId} не найдена.");
                return;
            }
            category.Name = newCategory.Name;
            context.SaveChanges();
            Console.WriteLine($"Категория {category.Name} обновлена!");
        }

        public void DeleteCategory(int categoryId)
        {
            using var context = new MarketPlaceDBContext();
            var category = context.Categories?.FirstOrDefault(c => c.Id == categoryId);
            if(category == null)
            {
                Console.WriteLine($"Категория с id {categoryId} не найдена.");
                return;
            }
            context.Categories?.Remove(category);
            context.SaveChanges();
            Console.WriteLine($"Категория {category.Name} удалена!");
        }
    }
}
