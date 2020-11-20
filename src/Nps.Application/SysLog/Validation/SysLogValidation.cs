using FluentValidation;
using Nps.Application.SysLog.Dtos;

namespace Nps.Application.SysLog.Validation
{
    public class SqlCurdAddInputValidation : AbstractValidator<SqlCurdAddInput>
    {
        public SqlCurdAddInputValidation()
        {
            CascadeMode = CascadeMode.Stop;
        }
    }
}