using System;
using System.Collections.Generic;

#nullable disable

namespace BOT.Models
{
    public partial class User
    {
        public User()
        {
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public int UserId { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
