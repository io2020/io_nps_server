using Microsoft.AspNetCore.Builder;
using Nps.Infrastructure;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;

namespace Nps.Api.Extension.Application
{
    /// <summary>
    /// IApplicationBuilder扩展-Swagger
    /// </summary>
    public static partial class SwaggerExtension
    {
        /// <summary>
        /// 使用Swagger
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="streamHtml">自定义首页路径</param>
        public static void UseDefineSwagger(this IApplicationBuilder app, Func<Stream> streamHtml)
        {
            Check.NotNull(app, nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/nps/swagger.json", "nps");
                //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                options.RoutePrefix = "";//直接根目录访问
                options.DocExpansion(DocExpansion.None);//折叠Api
                options.ConfigObject.DisplayOperationId = true;

                // 将swagger首页，设置成我们自定义的页面，记得这个字符串的写法：{项目名.index.html}
                if (streamHtml.Invoke() == null)
                {
                    var msg = "index.html的属性，必须设置为嵌入的资源";
                    Log.Error(msg);
                    throw new Exception(msg);
                }
                options.IndexStream = streamHtml;
            });
        }
    }
}