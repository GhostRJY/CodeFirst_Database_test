

namespace CodeFirst.Models
{
    //таблица посредник
    public class UserToReview
    {
        public int User_id { get; set; }
        public User? User { get; set; }

        public int Review_id { get; set; }
        public Review? Review { get; set; }
    }
}
