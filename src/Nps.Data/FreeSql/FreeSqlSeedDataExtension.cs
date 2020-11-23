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

            return codeFirst;
        }
    }
}