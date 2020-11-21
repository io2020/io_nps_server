using FluentValidation;
using Nps.Application.Nps.Dots;

namespace Nps.Application.Nps.Validation
{
    /// <summary>
    /// 开通端口参数验证
    /// </summary>
    public class NpsOpenInputValidation : AbstractValidator<NpsOpenInput>
    {
        public NpsOpenInputValidation()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.DeviceUniqueId)
                .NotEmpty().WithName("设备唯一标识").WithMessage("{PropertyName}不能为空")
                .MinimumLength(10).WithMessage("{PropertyName}长度不能小于{MinLength}位")
                .MinimumLength(50).WithMessage("{PropertyName}长度不能大于{MaxLength}位");

            RuleFor(x => x.OpenPorts).NotNull().WithMessage("设备需要开通的端口号不能为空");

            RuleFor(x => x.Remark)
                .MaximumLength(100).WithName("设备备注信息")
                .WithMessage("{PropertyName}长度不能大于{MinLength}字符");
        }
    }
}