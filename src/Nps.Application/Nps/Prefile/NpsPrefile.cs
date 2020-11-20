using AutoMapper;
using Nps.Application.Nps.Dots;
using Nps.Data.Entities;

namespace Nps.Application.Nps.Prefile
{
    public class NpsPrefile : Profile
    {
        public NpsPrefile()
        {
            CreateMap<NpsServer, NpsServerOutput>();
        }
    }
}