using FluentValidation;
using Presentation.Models;

namespace Presentation.Validators
{
    public class LoginBindingModelValidator : AbstractValidator<LoginBindingModel>
    {
        public LoginBindingModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(20);
        }
    }
}
