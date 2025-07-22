using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Exceptions
{
    public class UserCreateFailedException : Exception
    {
        public UserCreateFailedException():base("Istifadeci qeydiyyatdan kecerken bir xeta bash verdi")
        {
                
        }
        public UserCreateFailedException(string? message):base(message)
        {
            
        }

        public UserCreateFailedException(string? message, Exception? innerException) : base(message, innerException) 
        {
            
        }
    }
}
