using AutoMapper;
using Nps.Application.Nps.Dots;
using Nps.Data.Entities;
using System.Collections.Generic;

namespace Nps.Application.Nps.Prefile
{
    public class NpsPrefile : Profile
    {
        public NpsPrefile()
        {
            CreateMap<NpsServer, NpsServerSearchOutput>();

            CreateMap<NpsAppSecret, NpsClientOpenedOutput>()
                .ForMember(dest => dest.DeviceUniqueId, options => options.MapFrom(src => src.DeviceUniqueId))
                .ForMember(dest => dest.OpenPorts, options => options.MapFrom(src => DefineConvertCleverMagic(src)));
        }

        private static List<NpsClientOpenedPortOutput> DefineConvertCleverMagic(NpsAppSecret source)
        {
            var output = new List<NpsClientOpenedPortOutput>();

            source?.NpsClient?.NpsChannels?.ForEach(src =>
            {
                output.Add(new NpsClientOpenedPortOutput
                {
                    ServerIPAddress = source?.NpsServer?.ServerIPAddress,
                    ClientConnectPort = source?.NpsServer?.ClientConnectPort.ToString(),
                    ServerPort = src?.ServerPort.ToString(),
                    DeviceAddress = src?.DeviceAddress
                });
            });

            return output;
        }
    }
}