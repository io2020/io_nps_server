using FluentValidation;
using Nps.Application.Account.Dtos;

namespace Nps.Application.Account.Validation
{
    public class LoginInputValidation : AbstractValidator<LoginInput>
    {
        public LoginInputValidation()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.UserName).NotEmpty().WithMessage("用户名不能为空");

            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空");
        }
    }
}