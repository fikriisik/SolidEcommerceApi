using FluentValidation;
using SolidEcommerceApi.DTOs;

namespace SolidEcommerceApi.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("İsim boş olamaz.")
            .MaximumLength(50).WithMessage("İsim 50 karakteri geçemez.");
    }
}