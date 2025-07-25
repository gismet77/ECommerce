﻿using MediatR;

namespace ECommerce.Application.Features.Commands.Order.CreateOrder
{
    public class CreateOrderCommandRequsent : IRequest<CreateOrderCommandResponse>
    { 
        public string Description { get; set; }
        public string Address { get; set; }
    }
}