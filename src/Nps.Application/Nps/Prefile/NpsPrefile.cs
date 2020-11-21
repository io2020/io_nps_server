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
            CreateMap<NpsServer, NpsServerOutput>();

            CreateMap<NpsAppSecret, NpsOpenedOutput>()
                .ForMember(dest => dest.DeviceUniqueId, options => options.MapFrom(src => src.DeviceUniqueId))
                .ForMember(dest => dest.OpenPorts, options => options.MapFrom(src => DefineConvertCleverMagic(src)));
        }

        private static List<NpsOpenedPortOutput> DefineConvertCleverMagic(NpsAppSecret source)
        {
            var output = new List<NpsOpenedPortOutput>();

            source?.NpsClient?.NpsChannels?.ForEach(src =>
            {
                output.Add(new NpsOpenedPortOutput
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