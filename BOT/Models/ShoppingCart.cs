using System;
using System.Collections.Generic;

#nullable disable

namespace BOT.Models
{
    public partial class ShoppingCart
    {
        public int Product { get; set; }
        public int Customer { get; set; }

        public virtual User CustomerNavigation { get; set; }
        public virtual Product ProductNavigation { get; set; }
    }
}
