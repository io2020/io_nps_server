using Nps.Application.Nps.Dots;
using Nps.Application.Nps.Services;
using Nps.Core.Data;
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
        public async Task NpsClientSearchTestAsync()
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

            var input1 = new PagingInput<NpsClientSearchInput>();
            var input2 = new PagingInput<NpsClientSearchInput>
            {
                Filter = new NpsClientSearchInput { DeviceUniqueId = "AAAAAAAAAA" }
            };
            var input3 = new PagingInput<NpsClientSearchInput>
            {
                Filter = new NpsClientSearchInput { DeviceUniqueId = "BBBBBBBBBB", SearchPorts = new List<string> { "1111", "5555", "6666" } }
            };

            var searchResult1 = await _npsClientService.SearchAsync(input1);
            var searchResult2 = await _npsClientService.SearchAsync(input2);
            var searchResult3 = await _npsClientService.SearchAsync(input3);

            Assert.NotNull(searchResult1);
            Assert.NotNull(searchResult2);
            Assert.NotNull(searchResult3);
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

            var input = new NpsClientOpenInput
            {
                DeviceUniqueId = "BBBBBBBBBB",
                OpenPorts = new List<string> { "1111", "2222", "3333", "4444", "5555", "6666" },//, "4444", "5555", "6666", "7777", "8888", "9999"
                Remark = "开通端口单元测试"
            };

            var openResult = await _npsClientService.OpenAsync(input);

            Assert.NotNull(openResult);
        }

        [Fact]
        public async Task NpsClientDeleteTestAsync()
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

            var input = new NpsClientDeleteInput
            {
                DeviceUniqueId = "AAAAAAAAAA",
                DeletePorts = new List<string> { "1111", "2222", "3333" }
            };

            var deleteResult = await _npsClientService.DeleteAsync(input);

            Assert.NotNull(deleteResult);
        }
    }
}