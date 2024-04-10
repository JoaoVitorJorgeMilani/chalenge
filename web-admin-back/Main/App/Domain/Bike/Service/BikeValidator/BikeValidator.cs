using FluentValidation;

namespace Main.App.Domain.Bike
{
    public class BikeValidator : AbstractValidator<Bike>
    {
        public BikeValidator()
        {
            RuleFor(x => x.Identifier)
              .NotEmpty()
              .Length(10, 15);

            RuleFor(x => x.ManufacturingYear)
              .NotEmpty()
              .Length(4, 4)
              .Must(x => int.TryParse(x, out var val) && val > 0)
              .Must(x => int.TryParse(x, out var val) && val >= 1930 && val <= DateTime.Now.Year);

            RuleFor(x => x.BikeModel)
              .NotEmpty()
              .Length(5, 30);

            RuleFor(x => x.LicensePlate)
              .NotEmpty()
              .Length(7, 7);
        }

        
    }
}