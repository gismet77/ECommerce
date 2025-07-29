﻿using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.AppUser.UpdatePassword
{
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommandRequest, UpdatePasswordCommandResponse>
    {
        readonly IUserService _userService;

        public UpdatePasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UpdatePasswordCommandResponse> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
        {
            if(!request.Password.Equals(request.PasswordConfirm))
                throw new PasswordChangeFailedException("Sifreni dogrulayin");

            await _userService.UpdatePasswordAsync(request.UserId, request.ResetToken, request.Password);

            return new();
        }
    }
}
