using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Contexts;
using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Tests
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private TestDBContext? m_context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TestDBContext>() 
            .UseInMemoryDatabase("TestDB")
            .Options;

            m_context = new TestDBContext(options);
        }

        [Test]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            var category1 = new Category { Name = "Книги" };
            var category2 = new Category { Name = "GPU" };

            m_context?.Categories?.AddRange(category1, category2);
            m_context?.SaveChanges();

            var product1 = new Product { Name = "Война и мир", Count = 10, Price = 100.00, CategoryId = 1 };
            var product2 = new Product { Name = "RTX 3090", Count = 5, Price = 1000.00, CategoryId = 2 };

            m_context?.Products?.AddRange(product1, product2);
            m_context?.SaveChanges();

            var result = m_context?.Products?.Include(c=>c.Category).ToList();

            Assert.That(result?.Count, Is.EqualTo(2));
            Assert.That(result?[0].Name, Is.EqualTo("Война и мир"));
            //Assert.That(result?[0].Category?.Name, Is.EqualTo("Книги"));
        }
    }
}
