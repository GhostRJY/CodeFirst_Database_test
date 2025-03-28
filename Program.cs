﻿using CodeFirst.Contexts;
using CodeFirst.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;


namespace CodeFirst_Database_test
{
    internal class Program
    {

        //static void TestAnnotationAndFluent() 
        //{
        //    MarketPlaceDBContext context = new MarketPlaceDBContext();
        //    context.ResetDatabase();
        //    context.Dispose();

        //    //Добавление категорий
        //    CategoryRepository categoryRepo = new CategoryRepository();


        //    categoryRepo.AddCategory("Книги");
        //    categoryRepo.AddCategory("GPU");
        //    categoryRepo.AddCategory("CPU");

        //    categoryRepo.AddCategory(""); //проверка на пустое имя
        //    categoryRepo.GetAllCategories();


        //    //Добавление товаров
        //    ProductRepository productRepo = new ProductRepository();

        //    productRepo.AddProduct("Война и мир", 10, 100.00, 1);
        //    productRepo.AddProduct("RTX 3090", 5, 700.00, 2);
        //    productRepo.AddProduct("Ryzen 9 5900X", 3, 500.00, 3);

        //    productRepo.AddProduct("", 5, 10.00, 1); //проверка на пустое имя

        //    productRepo.GetAllProducts();
        //}

        //static void CreateStoredProcedure(in string procedureText)
        //{
        //    string connectionString = "Server=localhost;Database=MarketPlace;Trusted_Connection=True;TrustServerCertificate=True;";
        //    using(var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        // SQL для создания хранимой процедуры
                
        //        using(var command = new SqlCommand(procedureText, connection))
        //        {
        //            command.ExecuteNonQuery();
        //            Console.WriteLine("Хранимая процедура была успешно создана.");
        //        }
        //        connection.Close();
        //    }

        //}

        static void Main(string[] args)
        {
            //TestAnnotationAndFluent();

            //тест создания хранимой процедуры
            //CreateStoredProcedure(@"
            //    CREATE OR ALTER PROCEDURE GetCategoriesOfProduct
            //    AS
            //    BEGIN
            //        SELECT p.Id, p.Name, c.Name AS CategoryName
            //        FROM Products p
            //        JOIN Categories c ON p.CategoryId = c.Id
            //    END
            //");

            //ProductRepository repo = new ProductRepository();
            //repo.ExecStoredProcedure("GetCategoriesOfProduct");

            //проверка Explicit загрузки
            //repo.GetAllOrdersWithProducts();

            var options = new DbContextOptionsBuilder<TestDBContext>()
            .UseInMemoryDatabase("TestDB")
            .Options;

            using var context = new TestDBContext(options);
            var repoProduct = new ProductRepository(context);
            var repoCategory = new CategoryRepository(context);
            
            var category1 = new Category { Name = "Книги" };
            var category2 = new Category { Name = "GPU" };

            context.Categories?.AddRange(category1, category2);
            context.SaveChanges();

            var product1 = new Product { Name = "Война и мир", Count = 10, Price = 100.00, CategoryId = 1};
            var product2 = new Product { Name = "RTX 3090", Count = 5, Price = 1000.00, CategoryId = 2 };

            context.Products?.AddRange(product1, product2);
            context.SaveChanges();

            repoProduct.GetAllProducts();

        }
    }
}
