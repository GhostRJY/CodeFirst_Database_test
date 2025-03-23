
using Microsoft.EntityFrameworkCore;
using CodeFirst.Contexts;
using System.ComponentModel.DataAnnotations;



namespace CodeFirst.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage ="Заполни имя!")]
        [MinLength(3, ErrorMessage ="Минимально имя должно содержать 3 символа")]
        [MaxLength(50, ErrorMessage = "Имя не может содержать более 50 символов")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Адрес электронной почты обязателен!")]
        [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Пароль не заполнен!")]
        public string? Password { get; set; }

        //Связь с Order (один ко многим)
        public ICollection<Order>? Orders { get; set; }

        //Связь с Review (многие ко многим)
        public ICollection<UserToReview>? UserToReviews { get; set; }
    }

    public class PersonRepository
    {
        private readonly TestDBContext m_context;

        public List<User> GetAllUsers()
        {
            //using var context = new MarketPlaceDBContext();
            var users = m_context.Users?.Include(p => p.Orders).ToList();

            foreach(var person in users!)
            {
                Console.WriteLine($"{person.Name} {person.Email}  кол-во заказов {person.Orders?.Count()}");                
            }

            return users;
        }

        public void GetUserById(int id)
        {
            //using var context = new MarketPlaceDBContext();
            var person = m_context.Users?.FirstOrDefault(p => p.Id == id);
            if(person == null)
            {
                Console.WriteLine($"Пользователь с id {id} не найден.");
                return;
            }
            Console.WriteLine($"{person.Name} {person.Email}");
        }

        public void AddUser(in User newUser)
        {
            //using var context = new MarketPlaceDBContext();
            var user = newUser;

            m_context.Users?.Add(user);
            m_context.SaveChanges();
            Console.WriteLine($"Пользователь {user.Name} добавлен!");
        }

        public void UpdateUser(string? personName, in User updUser)
        {
            //using var context = new MarketPlaceDBContext();
            var person = m_context.Users.FirstOrDefault(u => u.Name == personName);
            if(person == null)
            {
                Console.WriteLine($"Человек с именем {personName} не найден.");
                return;
            }

            person = updUser;
            //person.Name = updUser.Name;
            //person.Email = updUser.Email;
            //person.Password = updUser.Password;

            m_context.SaveChanges();
            Console.WriteLine($"Данные пользователя {personName} обновлены!");
        }

        public void DeleteUser(string personName)
        {
            //using var context = new MarketPlaceDBContext();
            var person = m_context.Users.FirstOrDefault(u => u.Name == personName);
            if(person == null)
            {
                Console.WriteLine($"Человек с именем {personName} не найден.");
                return;
            }

            m_context.Users?.Remove(person);
            m_context.SaveChanges();
            Console.WriteLine($"Пользователь {personName} удален!");
        }
    }

    //реализую строитель user (Fluent API)
    public class UserBuilder
    {
        private User m_user = new User();
        public UserBuilder SetName(in string name)
        {
            m_user.Name = name;
            return this;
        }
        public UserBuilder SetEmail(in string email)
        {
            m_user.Email = email;
            return this;
        }
        public UserBuilder SetPassword(in string password)
        {
            m_user.Password = password;
            return this;
        }
        public User Build()
        {
            return m_user;
        }
    }

}
