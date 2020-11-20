using FreeSql;
using FreeSql.Internal;
using Nps.Application.SysLog.Dtos;
using Nps.Application.SysLog.Services;
using Nps.Core.Aop.Attributes;
using Nps.Core.Entities;
using Nps.Core.Infrastructure.Configs;
using Nps.Core.Infrastructure.IdGenerators;
using Nps.Data.FreeSql;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StackExchange.Profiling;
using System;
using System.Threading.Tasks;

namespace Nps.Api.Framework.ServiceExtensions
{
    /// <summary>
    /// IServiceCollection扩展-FreeSqlORM
    /// </summary>
    public static partial class FreeSqlExtension
    {
        /// <summary>
        /// 注入FreeSql
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public static void AddFreeSql(this IServiceCollection services)
        {
            Log.Logger.Information("Initialize FreeSql Start;");

            //获取数据库类型及其连接字符串
            var dataTypeValue = AppSettings.Get(new string[] { "Database", "DataType" });
            var dataTypeConnectionString = string.Empty;
            if (Enum.TryParse(dataTypeValue, out DataType dataType))
            {
                if (!Enum.IsDefined(typeof(DataType), dataType))
                {
                    Log.Error($"数据库配置Database:ConnectionStrings:DataType:{dataType}无效");
                }
                dataTypeConnectionString = AppSettings.Get(new string[] { "Database", "MasterConnectionStrings" });
                if (dataTypeConnectionString.IsNullOrWhiteSpace())
                {
                    Log.Error($"数据库配置Database:ConnectionStrings:{dataType}连接字符串无效");
                }
            }
            else
            {
                Log.Error($"数据库配置Database:ConnectionStrings:DataType:{dataTypeValue}无效");
            }

            //创建建造器
            var builder = new FreeSqlBuilder()
                .UseConnectionString(dataType, dataTypeConnectionString)
                .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithLower)
                //设置是否自动同步表结构，开发环境必备
                .UseAutoSyncStructure(AppSettings.Get(new string[] { "Database", "SyncStructure" }).ToBooleanOrDefault(false))
                .UseNoneCommandParameter(true)
                .UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
                {//监听所有命令
                    Log.Logger.Information($"MonitorCommand:{traceLog};");
                });

            //生成数据库操作对象
            IFreeSql freeSql = builder.Build();
            //开启联级保存功能（默认为关闭）
            freeSql.SetDbContextOptions(opts => opts.EnableAddOrUpdateNavigateList = true);
            //加载过滤器，设置全局软删除为false
            freeSql.GlobalFilter.Apply<ISoftDelete>("IsDeleted", a => a.IsDeleted == false);

            //监听CURD操作
            freeSql.Aop.CurdAfter += (s, e) =>
            {
                //Task.Run(() =>//写法一
                Parallel.For(0, 1, a =>//写法二
                {
                    //添加MiniProfiler监控SQL性能
                    MiniProfiler.Current.CustomTiming("SQL：", $"【SQL语句】：{e.Sql}，耗时：{e.ElapsedMilliseconds}毫秒");

                    //若实体中不含DisableSqlCurd标记，则将记录写入至数据库
                    if (e.EntityType.GetCustomAttribute<DisableSqlCurdAttribute>(false) == null)
                    {
                        //获取ISqlCurdService对象，在获取之前需要注入
                        var sqlCurdService = services.BuildServiceProvider().GetRequiredService<ISqlCurdService>();
                        sqlCurdService.AddLog(new SqlCurdAddInput
                        {
                            FullName = e.EntityType.FullName,
                            ExecuteMilliseconds = e.ElapsedMilliseconds,
                            Sql = e.Sql
                        });
                    }
                });
            };

            //获取IdGenerator对象，在获取之前需要注入
            var longGenerator = services.BuildServiceProvider().GetRequiredService<ILongIdGenerator>();
            var guidGenerator = services.BuildServiceProvider().GetRequiredService<IGuidGenerator>();
            //审计Curd
            freeSql.Aop.AuditValue += (s, e) =>
            {
                if (e.Column.CsType == typeof(long))
                {
                    if (e.Property.GetCustomAttribute<IdGeneratorAttribute>(false) != null)
                    {
                        if (e.Value?.ToString() == "0")
                        {//当主键为long类型且拥有Id自生成标记且值为0时，主键列不能设置自增属性
                            e.Value = longGenerator?.Create();
                        }
                    }
                }
                else if (e.Column.CsType == typeof(Guid))
                {
                    if (e.Property.GetCustomAttribute<IdGeneratorAttribute>(false) != null)
                    {
                        if (e.Value?.ToString() == "00000000-0000-0000-0000-000000000000")
                        {//当主键为Guid类型且拥有Id自生成标记且值全为0时
                            e.Value = guidGenerator?.Create();
                        }
                    }
                }
            };

            //注入IOC框架
            services.AddSingleton(freeSql);
            //注入仓储
            services.AddFreeRepository();
            //注入工作单元
            services.AddScoped<UnitOfWorkManager>();
            //注入SQLCurd语句写入服务
            services.AddScoped<ISqlCurdService, SqlCurdService>();

            //连接数据库
            try
            {
                using var objPool = freeSql.Ado.MasterPool.Get();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex + ex.StackTrace + ex.Message + ex.InnerException);
                return;
            }

            //同步表结构及初始化数据
            try
            {
                //注意：只有当CURD到此表时，才会自动生成表结构。
                //如需系统运行时迁移表结构，请使用SyncStructure方法
                //在运行时直接生成表结构
                if (AppSettings.Get(new string[] { "Database", "SyncStructure" }).ToBooleanOrDefault(true))
                {
                    freeSql.CodeFirst
                        .ConfigEntity()
                        .SeedData(AppSettings.Get(new string[] { "Database", "SyncData" }).ToBooleanOrDefault(true))//初始化部分数据
                        .SyncStructure(FreeSqlEntitySyncStructure.FindIEntities(new string[] { "Nps.Data" }));
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex + ex.StackTrace + ex.Message + ex.InnerException);
                return;
            }

            Log.Logger.Information("Initialize FreeSql End;");
        }
    }
}