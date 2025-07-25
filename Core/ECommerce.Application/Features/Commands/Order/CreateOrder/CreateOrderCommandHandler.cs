using ECommerce.Application.Abstractions.Hubs;
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
        readonly IBasketService _basketService;
        readonly IOrderHubService _orderHubService;

        public CreateOrderCommandHandler(IOrderService orderService, IBasketService basketService, IOrderHubService orderHubService)
        {
            _orderService = orderService;
            _basketService = basketService;
            _orderHubService = orderHubService;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequsent request, CancellationToken cancellationToken)
        {
            await _orderService.CreateOrderAsync(new()
            {
                Description = request.Description,
                Address = request.Address,
                BasketId = _basketService.GetUserActiveBasket?.Id.ToString()
            });

            await _orderHubService.OrderAddedMessageAsync("Yeni bir sifarish geldi!!!");

            return new();
        }
    }
}
