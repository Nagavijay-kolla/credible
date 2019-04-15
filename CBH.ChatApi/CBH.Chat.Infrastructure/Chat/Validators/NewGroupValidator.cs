using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using FluentValidation;

namespace CBH.Chat.Infrastructure.Chat.Validators
{
    public class NewGroupValidator : AbstractValidator<NewGroupRequestModel>
    {
        public NewGroupValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.CreatedUserName).NotNull().NotEmpty();
            RuleFor(x => x.CreatedUserId).NotNull().NotEmpty();
        }
    }
}