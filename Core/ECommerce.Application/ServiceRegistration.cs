using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection collection) 
        {
            collection.AddMediatR(typeof(ServiceRegistration)); // 9cu versiya olmalidir Mediatr typeof islemeyi ucun
            collection.AddHttpClient(); //Facebooka gore
        }
    }
}
