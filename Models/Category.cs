
using CodeFirst.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirst.Models
{
    
    public class Category
    {
        //первичный ключ
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "не указано имя категории!")]
        [MinLength(3, ErrorMessage ="Название должно быть более 3 символов")]
        public string Name { get; set; }

        //связь один ко многим
        public ICollection<Product>? Products { get; set; }
    }

    public class CategoryRepository
    {

        private readonly TestDBContext m_context;

        public CategoryRepository(TestDBContext context)
        {
            m_context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public List<Category> GetAllCategories()
        {
            //using var context = new MarketPlaceDBContext();
            var categories = m_context.Categories?.ToList();
            foreach(var category in categories!)
            {
                Console.WriteLine($"{category.Name}");
            }

            return categories;
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
        public void AddCategory(in string newCategory)
        {
            var context = new MarketPlaceDBContext();
            CategoryBuilder categoryBuild = new CategoryBuilder();
            var category = categoryBuild.WithName(newCategory).Build();
            
            // валидация категории
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(category, null, null);
            bool isValid = Validator.TryValidateObject(category, validationContext, validationResults, true);

            if(!isValid)
            {
                Console.WriteLine("Ошибка валидации:");
                foreach(var validationResult in validationResults)
                {
                    Console.WriteLine(validationResult.ErrorMessage);
                }
                return;
            }

            context.Categories?.Add(category);            
            context.SaveChanges();            
            context.Dispose();
            Console.WriteLine($"Категория {newCategory} добавлена!");
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

    //реализую строитель Category (Fluent API)
    public class CategoryBuilder
    {
        private Category m_category = new Category();
        public CategoryBuilder WithName(string name)
        {
            m_category.Name = name;
            return this;
        }
        public Category Build()
        {
            return m_category;
        }
    }

}
