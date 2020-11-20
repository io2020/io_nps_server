using FreeSql;
using Nps.Core.Infrastructure.Helpers;
using Nps.Data.Entities;
using System;
using System.Collections.Generic;

namespace Nps.Data.FreeSql
{
    /// <summary>
    /// FreeSql扩展--初始化数据
    /// </summary>
    public static partial class FreeSqlSeedDataExtension
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="codeFirst">ICodeFirst对象</param>
        /// <param name="isSyncData">是否初始化数据</param>
        /// <returns>返回ICodeFirst对象</returns>
        public static ICodeFirst SeedData(this ICodeFirst codeFirst, bool isSyncData)
        {
            if (!isSyncData) return codeFirst;

            //初始化用户数据
            codeFirst.Entity<User>(e =>
            {
                e.HasData(new List<User>
                {
                    new User
                    {
                        UserName="admin",
                        NikeName="管理员",
                        Email ="admin@yeah.net",
                        Mobile = "18655556666",
                        Password = EncryptHelper.Md5By32("superadmin"),
                        CreateTime=DateTime.Now
                    },
                    new User
                    {
                        UserName="ioself",
                        NikeName="自研",
                        Email ="ioself@yeah.net",
                        Mobile = "18655556667",
                        Password = EncryptHelper.Md5By32("ioself"),
                        CreateTime=DateTime.Now
                    }
                });
            });

            //初始化Nps服务器数据
            codeFirst.Entity<NpsServer>(e =>
            {
                e.HasData(new NpsServer
                {
                    HostDomain = "http://8.131.77.125:7501",
                    HostIP = "8.131.77.125",
                    HostPort = 7501,
                    ClientConnectPort = 8024,
                    ProtocolType = "tcp",
                    HttpPort = 8001,
                    HttpsPort = 8443,
                    Version = "0.26.9",
                    CreateTime = DateTime.Now,
                    //初始化Nps服务器数据 1对多级联保存
                    NpsAppSecrets = new List<NpsAppSecret>
                    {
                        new NpsAppSecret
                        {
                            DeviceUniqueId=Guid.NewGuid().ToString("N"),
                            AppSecret=Guid.NewGuid().ToString("N"),
                            CreateTime=DateTime.Now,
                            IsDeleted=false,
                            NpsClient=new NpsClient
                            {
                                RemoteClientId=11,
                                IsConfigConnAllow=true,
                                IsCompress=false,
                                IsCrypt=true,
                                Remark="添加客户端",
                                NpsChannels=new List<NpsChannel>
                                {
                                    new NpsChannel
                                    {
                                        RemoteChannelId=15,
                                        ServerPort=20005,
                                        TargetAddress="6688",
                                        Remark="添加隧道"
                                    }
                                }
                            }
                        },
                        new NpsAppSecret
                        {
                            DeviceUniqueId=Guid.NewGuid().ToString("N"),
                            AppSecret=Guid.NewGuid().ToString("N"),
                            CreateTime=DateTime.Now,
                            IsDeleted=false
                        },
                        new NpsAppSecret
                        {
                            DeviceUniqueId=Guid.NewGuid().ToString("N"),
                            AppSecret=Guid.NewGuid().ToString("N"),
                            CreateTime=DateTime.Now,
                            IsDeleted=false
                        }
                    }
                });
            });

            return codeFirst;
        }
    }
}