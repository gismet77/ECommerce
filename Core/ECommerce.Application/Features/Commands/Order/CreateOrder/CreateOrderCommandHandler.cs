using ECommerce.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.Order.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequsent, CreateOrderCommandResponse>
    {
        readonly IOrderService _orderService;

        public CreateOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequsent request, CancellationToken cancellationToken)
        {
            await _orderService.CreateOrderAsync(new()
            {
                Description = request.Description,
                Address = request.Address,
                BasketId = 
            });
        }
    }
}
