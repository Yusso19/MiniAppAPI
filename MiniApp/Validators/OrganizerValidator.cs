namespace MiniApp.Validators
{
    using FluentValidation;
    using MiniApp.DTOs;

    public class OrganizerValidator : AbstractValidator<OrganizerDto>
    {
        public OrganizerValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Phone).MaximumLength(20);
        }
    }
}
