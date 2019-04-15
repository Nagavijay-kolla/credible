using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using FluentValidation;

namespace CBH.Chat.Infrastructure.Chat.Validators
{
    public class NewMessageValidator : AbstractValidator<NewMessageRequestModel>
    {
        public NewMessageValidator()
        {
            RuleFor(x => x.ThreadId).NotNull().NotEmpty();
            RuleFor(x => x.Content).NotNull().NotEmpty();
            RuleFor(x => x.FromUserName).NotNull().NotEmpty();
            RuleFor(x => x.FromUserId).NotNull().NotEmpty();
        }
    }
}