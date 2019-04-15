using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using FluentValidation;

namespace CBH.Chat.Infrastructure.Chat.Validators
{
    public class NewBroadcastMessageValidator : AbstractValidator<NewBroadcastMessageRequestModel>
    {
        public NewBroadcastMessageValidator()
        {
            RuleFor(x => x.Content).NotNull().NotEmpty();
            RuleFor(x => x.FromUserName).NotNull().NotEmpty();
            RuleFor(x => x.FromUserId).NotNull().NotEmpty();
        }
    }
}