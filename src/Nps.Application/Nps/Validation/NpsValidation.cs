using FluentValidation;
using Nps.Application.Nps.Dots;

namespace Nps.Application.Nps.Validation
{
    public class NpsClientInputValidation : AbstractValidator<NpsClientInput>
    {
        public NpsClientInputValidation()
        {
            CascadeMode = CascadeMode.Stop;
        }
    }
}