namespace MiniApp.Validators
{
    using FluentValidation;
    using MiniApp.DTOs;

    public class EventValidator : AbstractValidator<EventDto>
    {
        public EventValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Description).MaximumLength(500);
            RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Date).GreaterThan(DateTime.UtcNow);
            RuleFor(x => x.OrganizerId).GreaterThan(0);

        }
    }

}
