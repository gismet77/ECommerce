﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Identity
{
    // nugetden asagidaki paketleri yukle
    //Microsoft.AspNetCore.Identity.Entity
    //Microsoft.AspNetCore.Identity.EntityFrameworkCore
    public class AppUser : IdentityUser<string>
    {
        public string NameSurname { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
        public ICollection<Basket> Baskets { get; set; }
    }
}
