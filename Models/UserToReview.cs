using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
