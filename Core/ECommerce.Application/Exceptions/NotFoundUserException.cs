﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Exceptions
{
    public class NotFoundUserException : Exception
    {
        public NotFoundUserException(): base("Istifadeci adi ve ya sifresi sehvdir...")
        {
        }

        public NotFoundUserException(string? message) : base(message)
        {
        }

        public NotFoundUserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
