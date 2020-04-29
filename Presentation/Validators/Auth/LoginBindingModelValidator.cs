using FluentValidation;
using Presentation.Models;

namespace Presentation.Validators
{
    public class LoginBindingModelValidator : AbstractValidator<LoginBindingModel>
    {
        public LoginBindingModelValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(20);
        }
    }
}
