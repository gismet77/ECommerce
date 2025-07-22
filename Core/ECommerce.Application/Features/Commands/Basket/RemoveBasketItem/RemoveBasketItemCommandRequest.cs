using MediatR;

namespace ECommerce.Application.Features.Commands.Basket.RemoveBasketItem
{
    public class RemoveBasketItemCommandRequest : IRequest<RemoveBasketItemCommanResponse>
    {
        public string BasketItemId { get; set; }
    }
}