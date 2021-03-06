using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Cyf.Log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cyf.NetCore
{
    /// <summary>
    /// *********** 管道处理模型--中间件 ***********
    /// Startup--Config里面去指定了Http请求管道
    /// 何谓http请求的管道呢？
    /// 就是对Http请求的一连串的处理过程
    /// 就是给你一个HttpContext，然后一步步的处理，最终的得到结果
    /// 
    /// 
    /// *********** 新旧管道模型的区别 ***********
    /// Asp.Net请求管道： 请求最终会由一个具体的HttpHandler处理(page/ashx/mvchttphandler--action)
    ///                   但是还有多个步骤，被封装成事件--可以注册可以扩展--IHttpModule--提供了非常优秀的扩展性
    /// 有一个缺陷：太多管闲事儿了--一个http请求最核心是IHttpHandler--cookie Session  Cache NeginRequest endrequest maprequesthandler 授权
    /// ----这些不一定非得有---但是写死了---默认认为那些步骤是必须的---跟框架的设计思想有关---.Net入门简单精通难---因为框架大包大揽，
    /// 全家桶式，随便拖一下控件，写点数据库，一个项目就出来了---所以精通也难----也要付出代价，就是包袱比较重，不能轻装前行
    /// ---.NetCore是一套全新的平台，已经不再向前兼容---设计更追求组件化，追求高性能---没有全家桶        
    /// 
    /// Asp.NetCore全新的请求管道：
    /// 默认情况，管道只有一个404
    /// 然后你可以增加请求的处理(UseEndPoint)---这就是以前handler，只包含业务处理
    /// 其他的就是中间件middleware
    /// 
    /// 
    /// *********** 配置Autofac（AOP,IOC）框架 ***********
    /// a nuget--可以参考依赖项里面的autofac相关（3个类库）
    /// b [Program][CreateHostBuilder]下增加UseServiceProviderFactory扩展
    /// c [Startup]增加ConfigureContainer(ContainerBuilder containerBuilder)方法
    /// 
    /// 
    /// *********** Filter过滤器：Action,Result,Exception, Resource ***********
    ///  Asp.Net Core:Action&Result&Exception
    ///  
    /// 全局注册是在ConfigureServices[addmvc][addfilter]完成的
    ///      
    /// 全局 控制器  Action 分别注册，执行顺序是
    /// 全局--控制器--Action--Action执行过程--Action--控制器--全局
    /// 
    /// Result就在Action之后
    /// 
    /// 特性在编译的时候，会生成metadata，生成IL  必须是确定的，不能是DI提供
    /// 如果Filter需要注入，那么不能直接标记的
    /// 1 ServiceFilter   还需要在ConfigService指定一下
    /// 2 TypeFilter
    /// 3 全局
    /// 4 IFilterFactory
    /// order 小的先执行  默认是0
    /// 
    /// AuthorityFilter第一顺序
    /// ResourceFilter第二顺序
    /// 【敲黑板】完成后，才会去实例化控制器！！！
    /// Resource最适合做缓存，可以在最早的时候完成缓存
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddCommandLine(args)//支持命令行传参
                .Build();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.UseLog();
                })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
