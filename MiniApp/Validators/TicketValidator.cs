namespace MiniApp.Validators
{
    using FluentValidation;
    using MiniApp.DTOs;

    public class TicketValidator : AbstractValidator<TicketDto>
    {
        public TicketValidator()
        {
            RuleFor(x => x.EventName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Type).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.EventId).GreaterThan(0);
        }
    }
}
