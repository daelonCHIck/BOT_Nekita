using System;
using System.Collections.Generic;

#nullable disable

namespace BOT.Models
{
    public partial class Product
    {
        public Product()
        {
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public string Category { get; set; }
        public int Price { get; set; }
        public int ProductId { get; set; }

        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
