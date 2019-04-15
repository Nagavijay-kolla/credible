using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using FluentValidation;

namespace CBH.Chat.Infrastructure.Chat.Validators
{
    public class NewUserValidator : AbstractValidator<NewUserRequestModel>
    {
        public NewUserValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Role).NotNull().NotEmpty();
        }
    }
}