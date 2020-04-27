using FluentValidation;
using Presentation.Models;

namespace Presentation.Validators.Auth
{
    public class RegisterBindingModelValidator : AbstractValidator<RegisterBindingModel>
    {
        public RegisterBindingModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(250);
            RuleFor(x => x.Username).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(20);
        }
    }
}
