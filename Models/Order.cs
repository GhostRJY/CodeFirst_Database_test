using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Contexts;

namespace CodeFirst.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int Count { get; set; }

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
            using var context = new MarketPlaceDBContext();
            var order = newOrder;
            context.Orders?.Add(order);
            context.SaveChanges();
            Console.WriteLine($"Заказ {order.Count} добавлен!");
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
