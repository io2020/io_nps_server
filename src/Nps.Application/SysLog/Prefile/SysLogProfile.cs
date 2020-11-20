using AutoMapper;
using Nps.Application.SysLog.Dtos;
using Nps.Data.Entities;

namespace Nps.Application.SysLog.Prefile
{
    public class SysLogProfile : Profile
    {
        public SysLogProfile()
        {
            CreateMap<SqlCurdAddInput, SqlCurdLog>();
        }
    }
}