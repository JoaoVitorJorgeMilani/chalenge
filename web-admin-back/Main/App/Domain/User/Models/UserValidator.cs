using FluentValidation;

namespace Main.App.Domain.User
{
    public class UserValidator : AbstractValidator<UserEntity>
    {
        public UserValidator()
        {
            // TODO - Implementar regras de validação
            RuleFor(user => user.Name).NotEmpty();
            RuleFor(user => user.Cnpj).NotEmpty();
            RuleFor(user => user.Cnh).NotEmpty();
            RuleFor(user => user.Birthday).NotEmpty();
        }
    }

    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            // TODO - Implementar regras de validação
            RuleFor(user => user.name).NotEmpty();
            RuleFor(user => user.cnpj).NotEmpty();
            RuleFor(user => user.cnh).NotEmpty();
            RuleFor(user => user.birthday).NotEmpty();
        }
    }
}