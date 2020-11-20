using Nps.Core.Entities;
using Nps.Core.Infrastructure.Helpers;
using System;
using System.Linq;
using System.Reflection;

namespace Nps.Data.FreeSql
{
    /// <summary>
    /// FreeSql同步实体
    /// </summary>
    public static class FreeSqlEntitySyncStructure
    {
        /// <summary>
        /// 根据程序集查找所有实现IEntity的实体
        /// </summary>
        /// <param name="assemblyStrings">程序集名称列表</param>
        /// <returns>返回程序集中所有继承IEntity的实体</returns>
        public static Type[] FindIEntities(params string[] assemblyStrings)
        {
            if (assemblyStrings.Any())
            {
                Type baseType = typeof(IEntity);
                Assembly[] assemblies = assemblyStrings.Select(x => Assembly.Load(x)).ToArray();
                var entities = assemblies
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type => ReflectionHelper.IsDeriveClassFrom(type, baseType)).Distinct().ToArray();
                return entities;
            }
            return Array.Empty<Type>();
        }
    }
}