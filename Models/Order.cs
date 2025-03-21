
using CodeFirst.Contexts;
using System.ComponentModel.DataAnnotations;

namespace CodeFirst.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Range(1, 100, ErrorMessage = "Недопустимое значение кол-ва!")]
        public int Count { get; set; }

        [Required(ErrorMessage ="Заказчик должен быть указан в заказе!")]
        //один ко многим (один заказчик много заказов)
        public int UserId { get; set; } //внешний ключ заказчика
        public User? User { get; set; }

        //многие ко многим (разные товары для разных заказов)
        public ICollection<ProductToOrder>? ProductToOrders { get; set; }
    }

    public class OrderRepository
    {
        public void GetAllOrders()
        {
            using var context = new MarketPlaceDBContext();
            var orders = context.Orders?.ToList();
            foreach(var order in orders!)
            {
                Console.WriteLine($"{order.Count} {order.User?.Name}");
            }
        }
        public void GetOrderById(int id)
        {
            using var context = new MarketPlaceDBContext();
            var order = context.Orders?.FirstOrDefault(o => o.Id == id);
            if(order == null)
            {
                Console.WriteLine($"Заказ с id {id} не найден.");
                return;
            }
            Console.WriteLine($"{order.Count} {order.User?.Name}");
        }
        public void AddOrder(in Order newOrder)
        {
            //переработать добавление заказа
            //using var context = new MarketPlaceDBContext();
            //var order = newOrder;
            //context.Orders?.Add(order);
            //context.SaveChanges();
            //Console.WriteLine($"Заказ {order.Count} добавлен!");
        }

        public void UpdateOrder(int orderId, in Order newOrder)
        {
            using var context = new MarketPlaceDBContext();
            var order = context.Orders?.FirstOrDefault(o => o.Id == orderId);
            if(order == null)
            {
                Console.WriteLine($"Заказ с id {orderId} не найден.");
                return;
            }
            order.Count = newOrder.Count;
            order.UserId = newOrder.UserId;
            context.SaveChanges();
            Console.WriteLine($"Заказ с id {orderId} обновлен!");
        }
        public void DeleteOrder(int orderId)
        {
            using var context = new MarketPlaceDBContext();
            var order = context.Orders?.FirstOrDefault(o => o.Id == orderId);
            if(order == null)
            {
                Console.WriteLine($"Заказ с id {orderId} не найден.");
                return;
            }
            context.Orders?.Remove(order);
            context.SaveChanges();
            Console.WriteLine($"Заказ с id {orderId} удален!");
        }
    }
    
}
