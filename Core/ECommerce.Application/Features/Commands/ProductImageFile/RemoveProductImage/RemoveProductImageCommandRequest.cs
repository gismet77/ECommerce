﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.ProductImageFile.RemoveProductImage
{
    public class RemoveProductImageCommandRequest : IRequest<RemoveProductImageCommandResponse>
    {
        public string Id { get; set; }
        public string? ImageId { get; set; }
    }
}
