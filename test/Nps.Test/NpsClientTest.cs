using Nps.Application.Nps.Dots;
using Nps.Application.Nps.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Nps.Test
{
    public class NpsClientTest : BaseTest
    {
        private readonly INpsClientService _npsClientService;

        public NpsClientTest()
        {
            _npsClientService = GetRequiredService<INpsClientService>();
        }

        [Fact]
        public async Task NpsClientOpenTestAsync()
        {
            Thread.CurrentPrincipal = new ClaimsPrincipal
            (
                new ClaimsIdentity
                (
                    new List<Claim>
                    {
                        new Claim (ClaimTypes.NameIdentifier, "117084294703656960")
                    }
                )
            );

            var input = new NpsOpenInput
            {
                DeviceUniqueId = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                OpenPorts = new List<string> { "5555", "6666", "7777", "8888", "9999", "3333", "2222" },
                Remarks = "开通端口单元测试"
            };

            var openResult = await _npsClientService.OpenAsync(input);

            Assert.True(openResult);
        }
    }
}