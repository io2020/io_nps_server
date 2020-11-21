using Nps.Application.NpsApi;
using Nps.Core.Infrastructure.IdGenerators;
using System.Threading.Tasks;
using Xunit;

namespace Nps.Test
{
    public class NpsApiTest : BaseTest
    {
        private readonly INpsApi _npsApi;

        private readonly IGuidGenerator _guidGenerator;

        private int _serverPort = 20000;

        public NpsApiTest()
        {
            _npsApi = GetRequiredService<INpsApi>();
            _guidGenerator = GetRequiredService<IGuidGenerator>();
        }

        [Fact]
        public async Task NpsClientTest()
        {
            var authCpryKey = await _npsApi.AuthKeyAsync();

            var serverTime = await _npsApi.ServerTimeAsync();
            var authKey = Core.Infrastructure.Helpers.EncryptHelper.Md5By32($"jyioself2020{serverTime.Timestamp}").ToLower();

            var clients = await _npsApi.ClientListAsync(new Application.NpsApi.Dtos.ClientListInput
            {
                AuthKey = authKey,
                Timestamp = serverTime.Timestamp,
                KeyWords = "",//ux5zk6i9jbguetvc可根据唯一验证密钥查询
                OrderType = "asc",
                Offset = "0",
                Limit = "10"
            });

            serverTime = await _npsApi.ServerTimeAsync();
            authKey = Core.Infrastructure.Helpers.EncryptHelper.Md5By32($"jyioself2020{serverTime.Timestamp}").ToLower();
            var client = await _npsApi.ClientAsync(new Application.NpsApi.Dtos.ClientIdInput
            {
                Id = 2,
                AuthKey = authKey,
                Timestamp = serverTime.Timestamp
            });

            serverTime = await _npsApi.ServerTimeAsync();
            authKey = Core.Infrastructure.Helpers.EncryptHelper.Md5By32($"jyioself2020{serverTime.Timestamp}").ToLower();
            var appSecret = _guidGenerator.Create().ToString("N");

            var addClient = await _npsApi.AddClientAsync(new Application.NpsApi.Dtos.ClientAddInput
            {
                AuthKey = authKey,
                Timestamp = serverTime.Timestamp,
                Remark = $"测试添加客户端_{appSecret.Substring(0, 8)}",
                AppSecret = appSecret
            });

            serverTime = await _npsApi.ServerTimeAsync();
            authKey = Core.Infrastructure.Helpers.EncryptHelper.Md5By32($"jyioself2020{serverTime.Timestamp}").ToLower();
            appSecret = _guidGenerator.Create().ToString("N");

            var editClient = await _npsApi.EditClientAsync(new Application.NpsApi.Dtos.ClientEditInput
            {
                AuthKey = authKey,
                Timestamp = serverTime.Timestamp,
                Id = 7,
                Remark = $"测试编辑客户端_{appSecret.Substring(0, 8)}",
                AppSecret = appSecret
            });

            serverTime = await _npsApi.ServerTimeAsync();
            authKey = Core.Infrastructure.Helpers.EncryptHelper.Md5By32($"jyioself2020{serverTime.Timestamp}").ToLower();
            var deleteClient = await _npsApi.DeleteClientAsync(new Application.NpsApi.Dtos.ClientIdInput
            {
                AuthKey = authKey,
                Timestamp = serverTime.Timestamp,
                Id = 7
            });

            Assert.True(addClient.Status == 1);
            Assert.True(editClient.Status == 1);
            Assert.True(deleteClient.Status == 1);
        }

        [Fact]
        public async Task NpsClientChannelTest()
        {
            var authCpryKey = await _npsApi.AuthKeyAsync();

            var serverTime = await _npsApi.ServerTimeAsync();
            var authKey = Core.Infrastructure.Helpers.EncryptHelper.Md5By32($"jyioself2020{serverTime.Timestamp}").ToLower();

            var clientChannels = await _npsApi.ChannelListAsync(new Application.NpsApi.Dtos.ChannelListInput
            {
                AuthKey = authKey,
                Timestamp = serverTime.Timestamp,
                ClientId = 5,
                KeyWords = "",//10004可根据端口查询
                Offset = "0",
                Limit = "10"
            });

            serverTime = await _npsApi.ServerTimeAsync();
            authKey = Core.Infrastructure.Helpers.EncryptHelper.Md5By32($"jyioself2020{serverTime.Timestamp}").ToLower();
            var clientChannel = await _npsApi.ChannelAsync(new Application.NpsApi.Dtos.ChannelIdInput
            {
                Id = 7,
                AuthKey = authKey,
                Timestamp = serverTime.Timestamp
            });

            serverTime = await _npsApi.ServerTimeAsync();
            authKey = Core.Infrastructure.Helpers.EncryptHelper.Md5By32($"jyioself2020{serverTime.Timestamp}").ToLower();

            var addClientChannel = await _npsApi.AddChannelAsync(new Application.NpsApi.Dtos.ChannelAddInput
            {
                AuthKey = authKey,
                Timestamp = serverTime.Timestamp,
                Remark = $"测试添加客户端隧道_{_serverPort++}",
                ServerPort = _serverPort,
                TargetAddress = "6677",
                ClientId = 9
            });

            serverTime = await _npsApi.ServerTimeAsync();
            authKey = Core.Infrastructure.Helpers.EncryptHelper.Md5By32($"jyioself2020{serverTime.Timestamp}").ToLower();

            var editClientChannel = await _npsApi.EditChannelAsync(new Application.NpsApi.Dtos.ChannelEditInput
            {
                AuthKey = authKey,
                Timestamp = serverTime.Timestamp,
                Id = 7,
                Remark = $"测试编辑客户端隧道_{_serverPort++}",
                ServerPort = _serverPort,
                TargetAddress = "6677",
                ClientId = 9
            });

            serverTime = await _npsApi.ServerTimeAsync();
            authKey = Core.Infrastructure.Helpers.EncryptHelper.Md5By32($"jyioself2020{serverTime.Timestamp}").ToLower();
            var startClientChannel = await _npsApi.StartChannelAsync(new Application.NpsApi.Dtos.ChannelIdInput
            {
                AuthKey = authKey,
                Timestamp = serverTime.Timestamp,
                Id = 7
            });

            serverTime = await _npsApi.ServerTimeAsync();
            authKey = Core.Infrastructure.Helpers.EncryptHelper.Md5By32($"jyioself2020{serverTime.Timestamp}").ToLower();
            var stopClientChannel = await _npsApi.StopChannelAsync(new Application.NpsApi.Dtos.ChannelIdInput
            {
                AuthKey = authKey,
                Timestamp = serverTime.Timestamp,
                Id = 7
            });

            serverTime = await _npsApi.ServerTimeAsync();
            authKey = Core.Infrastructure.Helpers.EncryptHelper.Md5By32($"jyioself2020{serverTime.Timestamp}").ToLower();
            var deleteClientChannel = await _npsApi.DeleteChannelAsync(new Application.NpsApi.Dtos.ChannelIdInput
            {
                AuthKey = authKey,
                Timestamp = serverTime.Timestamp,
                Id = 7
            });

            Assert.True(addClientChannel.Status == 1);
            Assert.True(editClientChannel.Status == 1);
            Assert.True(deleteClientChannel.Status == 1);
        }
    }
}