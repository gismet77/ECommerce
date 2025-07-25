﻿using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities
{
    public class Order : BaseEntity
    {
        //public Guid CustomerId { get; set; } 
        //public Guid BasketId { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string OrderCode { get; set; }
        public Basket Basket { get; set; }
       // public ICollection<Product> Products { get; set; }
       // public Customer Customer { get; set; }
    }
}
