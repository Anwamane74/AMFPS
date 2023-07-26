using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EFCore.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Configuration;

namespace Services.Features.UserSecurity;

public class UserRegistrationUseCase : IUseCase
{
    public class UserForRegistrationCommand : IRequest<RegistrationResponseDto>
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    
    public class RegistrationResponseDto
    {
        public bool IsSuccessfulRegistration { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

    public class RequestHandler : IRequestHandler<UserForRegistrationCommand, RegistrationResponseDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RequestHandler(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<RegistrationResponseDto> Handle(UserForRegistrationCommand userForRegistration,
            CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(userForRegistration);

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (result.Succeeded)
            {
                return new RegistrationResponseDto{ IsSuccessfulRegistration = true };
            }

            var errors = result.Errors.Select(e => e.Description);
            return new RegistrationResponseDto { Errors = errors };

        }
    }
}