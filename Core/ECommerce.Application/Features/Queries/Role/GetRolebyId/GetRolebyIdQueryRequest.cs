using MediatR;

namespace ECommerce.Application.Features.Queries.Role.GetRolebyId
{
    public class GetRolebyIdQueryRequest : IRequest<GetRolebyIdQueryResponse>
    {
        public string Id { get; set; }
    }
}