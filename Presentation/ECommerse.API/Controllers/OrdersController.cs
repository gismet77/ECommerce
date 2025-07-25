using ECommerce.Application.Features.Commands.Order.CreateOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]

    public class OrdersController : ControllerBase
    {
        readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommandRequsent createOrderCommandRequsent) 
        {
            CreateOrderCommandResponse response = await _mediator.Send(createOrderCommandRequsent);
            return Ok(response);
        }
        //test comment
    }
}
