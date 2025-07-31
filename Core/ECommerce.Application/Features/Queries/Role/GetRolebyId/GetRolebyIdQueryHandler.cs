using ECommerce.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Queries.Role.GetRolebyId
{
    public class GetRolebyIdQueryHandler : IRequestHandler<GetRolebyIdQueryRequest, GetRolebyIdQueryResponse>
    {
        readonly IRoleService _roleService;

        public GetRolebyIdQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetRolebyIdQueryResponse> Handle(GetRolebyIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _roleService.GetRoleById(request.Id);
            return new()
            {
                Id = data.id,
                Name = data.name
            };
        }
    }
}
