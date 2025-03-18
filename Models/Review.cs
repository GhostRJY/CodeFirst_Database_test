using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Contexts;

namespace CodeFirst.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string? Text { get; set; }

        //Связь с Person
        public ICollection<UserToReview>? UserToReviews { get; set; }
    }

    public class ReviewRepository
    {
        public void GetAllReviews()
        {
            using var context = new MarketPlaceDBContext();
            var reviews = context.Reviews?.ToList();
            foreach(var review in reviews!)
            {
                Console.WriteLine($"{review.Text}");
            }
        }
        public void GetReviewById(int id)
        {
            using var context = new MarketPlaceDBContext();
            var review = context.Reviews?.FirstOrDefault(r => r.Id == id);
            if(review == null)
            {
                Console.WriteLine($"Отзыв с id {id} не найден.");
                return;
            }
            Console.WriteLine($"{review.Text}");
        }
        public void AddReview(in Review newReview)
        {
            using var context = new MarketPlaceDBContext();
            var review = newReview;
            context.Reviews?.Add(review);
            context.SaveChanges();
            Console.WriteLine($"Отзыв добавлен!");
        }
        public void UpdateReview(int reviewId, in Review newReview)
        {
            using var context = new MarketPlaceDBContext();
            var review = context.Reviews?.FirstOrDefault(r => r.Id == reviewId);
            if(review == null)
            {
                Console.WriteLine($"Отзыв с id {reviewId} не найден.");
                return;
            }
            review.Text = newReview.Text;
            context.SaveChanges();
            Console.WriteLine($"Отзыв обновлен!");
        }

        public void DeleteReview(int reviewId)
        {
            using var context = new MarketPlaceDBContext();
            var review = context.Reviews?.FirstOrDefault(r => r.Id == reviewId);
            if(review == null)
            {
                Console.WriteLine($"Отзыв с id {reviewId} не найден.");
                return;
            }
            context.Reviews?.Remove(review);
            context.SaveChanges();
            Console.WriteLine($"Отзыв удален!");
        }
    }
}
