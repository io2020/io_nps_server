﻿using Microsoft.Extensions.DependencyInjection;
using Nps.Infrastructure;
using Nps.Infrastructure.IdGenerators;
using Nps.Infrastructure.IdGenerators.Core;
using Nps.Infrastructure.IdGenerators.Ids;
using Serilog;

namespace Nps.Api.Extension.Service
{
    /// <summary>
    /// IServiceCollection扩展-ID生成器
    /// </summary>
    public static partial class IdGeneratorExtension
    {
        /// <summary>
        /// 注入ID生成器，首先需要注入此，避免之后引用时无法发现
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public static void AddIdGenerator(this IServiceCollection services)
        {
            Log.Logger.Information("Initialize IdGenerator Start;");

            Check.NotNull(services, nameof(services));

            //注入雪花算法Id
            services.AddSingleton(x =>
            {
                var dataCenterId = NpsEnvironment.NPS_IDGENERATOR_DATACENTERID.ToInt32OrDefault(11);
                var workerId = NpsEnvironment.NPS_IDGENERATOR_WORKEID.ToInt32OrDefault(11);

                return new SnowflakeId(dataCenterId, workerId);
            });
            services.AddSingleton<ILongIdGenerator, LongIdGenerator>();

            //注入有序Guid
            services.AddSingleton(x =>
            {
                var dataTypeValue = NpsEnvironment.NPS_DB_DATETYPE.ToInt32OrDefault(0);
                var sequentialGuidType = SequentialGuidType.SequentialAsString;
                if (dataTypeValue == 1)
                {
                    sequentialGuidType = SequentialGuidType.SequentialAtEnd;
                }
                else if (dataTypeValue == 3)
                {
                    sequentialGuidType = SequentialGuidType.SequentialAsBinary;
                }

                return new SequentialGuid(sequentialGuidType);
            });
            services.AddSingleton<IGuidGenerator, GuidGenerator>();

            //注入ObjectId
            services.AddSingleton<IStringIdGenerator, StringIdGenerator>();

            Log.Logger.Information("Initialize IdGenerator End;");
        }
    }
}