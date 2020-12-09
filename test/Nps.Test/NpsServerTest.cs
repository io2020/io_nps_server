using Nps.Application.Nps.Services;
using System.Threading.Tasks;
using Xunit;

namespace Nps.Test
{
    public class NpsServerTest : BaseTest
    {
        private readonly INpsServerService _npsServerService;

        public NpsServerTest()
        {
            _npsServerService = GetRequiredService<INpsServerService>();
        }

        [Fact]
        public async Task GetListAsyncTest()
        {
            var result = await _npsServerService.SearchAsync(new Core.Data.PagingInput<Application.Nps.Dtos.NpsServerSearchInput>
            {
                PageIndex = 1,
                PageSize = 10,
                Filter = new Application.Nps.Dtos.NpsServerSearchInput
                {
                    ServerIPAddress = "8.131.77.125"
                }
            });

            Assert.NotNull(result);
        }
    }
}