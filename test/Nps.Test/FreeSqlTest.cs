using Nps.Infrastructure;
using Nps.Core.Repositories;
using Nps.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Nps.Test
{
    public class FreeSqlTest : BaseTest
    {
        private readonly IFreeSqlRepository<NpsServer> _npsServerRepository;

        private readonly IFreeSqlRepository<NpsAppSecret> _npsAppSecretRepository;

        public FreeSqlTest()
        {
            _npsServerRepository = GetRequiredService<IFreeSqlRepository<NpsServer>>();
            _npsAppSecretRepository = GetRequiredService<IFreeSqlRepository<NpsAppSecret>>();
        }

        //比如 Topic、Type 两个表，一个 Topic 只能属于一个 Type：
        //https://www.cnblogs.com/kellynic/p/13575053.html
        //多对一查询  多对一配置 N对1，这样配置的（从自己身上找一个字段，与目标类型主键关联）：
        /*
         * 思考：ToList 默认返回 topic.* 和 type.* 不对，因为当 Topic 下面的导航属性有很多的时候，每次都返回所有导航属性？
            于是：ToList 的时候只会返回 Include 过的，或者使用过的 N对1 导航属性字段。
            fsql.Select<Topic>().ToList(); 返回 topic.*
            fsql.Select<Topic>().Include(a => a.Type).ToList(); 返回 topic.* 和 type.*
            fsql.Select<Topic>().Where(a => a.Type.name == "c#").ToList(); 返回 topic.* 和 type.*，此时不需要显式使用 Include(a => a.Type)
            fsql.Select().ToList(a => new { Topic = a, TypeName = a.Type.name }); 返回 topic.* 和 type.name
         */
        //一对多查询 一对多配置 1对N，这样配置的（从目标类型上找字段，与自己的主键关联）：
        [Fact]
        public async Task FreeSqlNavigateTest()
        {
            var servers = await _npsServerRepository.Where(x => 1 == 1).ToListAsync();

            var serversIncludeMany = await _npsServerRepository.Select.IncludeMany(x => x.NpsAppSecrets).ToListAsync();

            Check.NotNull(servers, nameof(NpsServer));

            var appSecrets = await _npsAppSecretRepository.Where(x => x.NpsServerId == servers[0].Id).ToListAsync();

            var appSecrets1 = await _npsAppSecretRepository.Where(x => x.NpsServer.Id == servers[0].Id).ToListAsync();

            var include = await _npsAppSecretRepository.Where(x => x.NpsServerId == servers[0].Id).Include(x => x.NpsServer).ToListAsync();

            var include1 = await _npsAppSecretRepository.Where(x => new List<long> { servers[0].Id }.Contains(x.NpsServer.Id)).ToListAsync();

            Assert.NotNull(servers);
            Assert.NotNull(serversIncludeMany);
            Assert.NotNull(appSecrets);
            Assert.NotNull(appSecrets1);
            Assert.NotNull(include);
            Assert.NotNull(include1);
        }
    }
}