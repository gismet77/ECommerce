﻿using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories 
{
    public class ProductImageFileReadRepository : ReadRepository<ProductImageFile>, IProductImageFileReadRepository
    {
        public ProductImageFileReadRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
