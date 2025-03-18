using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CodeFirst.Contexts;

namespace CodeFirst.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        //Связь с Order (один ко многим)
        public ICollection<Order>? Orders { get; set; }

        //Связь с Review (многие ко многим)
        public ICollection<UserToReview>? UserToReviews { get; set; }
    }

    public class PersonRepository
    {
        public void GetAllUsers()
        {
            using var context = new MarketPlaceDBContext();
            var users = context.Users?.Include(p => p.Orders).ToList();

            foreach(var person in users!)
            {
                Console.WriteLine($"{person.Name} {person.Email}  кол-во заказов {person.Orders?.Count()}");                
            }
        }

        public void GetUserById(int id)
        {
            using var context = new MarketPlaceDBContext();
            var person = context.Users?.FirstOrDefault(p => p.Id == id);
            if(person == null)
            {
                Console.WriteLine($"Пользователь с id {id} не найден.");
                return;
            }
            Console.WriteLine($"{person.Name} {person.Email}");
        }

        public void AddUser(in User newUser)
        {
            using var context = new MarketPlaceDBContext();
            var user = newUser;


            context.Users?.Add(user);
            context.SaveChanges();
            Console.WriteLine($"Пользователь {user.Name} добавлен!");
        }

        public void UpdateUser(string? personName, in User updUser)
        {
            using var context = new MarketPlaceDBContext();
            var person = context.Users.FirstOrDefault(u => u.Name == personName);
            if(person == null)
            {
                Console.WriteLine($"Человек с именем {personName} не найден.");
                return;
            }

            person = updUser;
            //person.Name = updUser.Name;
            //person.Email = updUser.Email;
            //person.Password = updUser.Password;

            context.SaveChanges();
            Console.WriteLine($"Данные пользователя {personName} обновлены!");
        }

        public void DeleteUser(string personName)
        {
            using var context = new MarketPlaceDBContext();
            var person = context.Users.FirstOrDefault(u => u.Name == personName);
            if(person == null)
            {
                Console.WriteLine($"Человек с именем {personName} не найден.");
                return;
            }

            context.Users?.Remove(person);
            context.SaveChanges();
            Console.WriteLine($"Пользователь {personName} удален!");
        }
    }
}
