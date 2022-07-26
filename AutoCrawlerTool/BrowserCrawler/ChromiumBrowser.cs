﻿using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BrowserCrawler
{
    public static class ChromiumBrowser
    {
        public const string NativeEdgePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";

        private const string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.66 Safari/537.36 Edg/103.0.1264.44";

        private const string _userAgentPhone = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_2_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0.3 Mobile/15E148 Safari/604.1 Edg/103.0.5060.66";

        #region 定义浏览器页面属性的字符串脚本（为了绕过反爬虫的js检测）

        private const string _navigator_languages = @"
        () => {
            Object.defineProperty(navigator, 'languages', {
                get: () => ['zh-CN', 'en-US', 'en'],
            });
        }";

        private const string _navigator_webdriver = @"
        () => {
            Object.defineProperty(navigator, 'webdriver', {
                get: () => false,
            });
        }";

        private const string _navigator_connection_rtt = @"
        () => {
            Object.defineProperty(navigator.connection, 'rtt', {
                get: () => 50,
            });
        }";

        private const string _navigator_plugins = @"
        () => {
            Object.defineProperty(navigator, 'plugins', {
                get: () => {
                    var ChromiumPDFPlugin = {};
                    ChromiumPDFPlugin.__proto__ = Plugin.prototype;
                    var plugins = {
                        0: ChromiumPDFPlugin,
                        description: 'Portable Document Format',
                        filename: 'internal-pdf-viewer',
                        length: 1,
                        name: 'Chromium PDF Plugin',
                        __proto__: PluginArray.prototype,
                    };
                    return plugins;
                },
            });
        }";

        private const string _chrome = @"
        () => {
            chrome = { 'app': { 'isInstalled': false }, 'webstore': { 'onInstallStageChanged': {}, 'onDownloadProgress': {} }, 'runtime': { 'PlatformOs': { 'MAC': 'mac', 'WIN': 'win', 'ANDROID': 'android', 'CROS': 'cros', 'LINUX': 'linux', 'OPENBSD': 'openbsd' }, 'PlatformArch': { 'ARM': 'arm', 'X86_32': 'x86-32', 'X86_64': 'x86-64' }, 'PlatformNaclArch': { 'ARM': 'arm', 'X86_32': 'x86-32', 'X86_64': 'x86-64' }, 'RequestUpdateCheckStatus': { 'THROTTLED': 'throttled', 'NO_UPDATE': 'no_update', 'UPDATE_AVAILABLE': 'update_available' }, 'OnInstalledReason': { 'INSTALL': 'install', 'UPDATE': 'update', 'CHROME_UPDATE': 'chrome_update', 'SHARED_MODULE_UPDATE': 'shared_module_update' }, 'OnRestartRequiredReason': { 'APP_UPDATE': 'app_update', 'OS_UPDATE': 'os_update', 'PERIODIC': 'periodic' } } };
        }";

        private const string _window_chrome = @"
        () => {
            window.chrome = { 'app': { 'isInstalled': false }, 'webstore': { 'onInstallStageChanged': {}, 'onDownloadProgress': {} }, 'runtime': { 'PlatformOs': { 'MAC': 'mac', 'WIN': 'win', 'ANDROID': 'android', 'CROS': 'cros', 'LINUX': 'linux', 'OPENBSD': 'openbsd' }, 'PlatformArch': { 'ARM': 'arm', 'X86_32': 'x86-32', 'X86_64': 'x86-64' }, 'PlatformNaclArch': { 'ARM': 'arm', 'X86_32': 'x86-32', 'X86_64': 'x86-64' }, 'RequestUpdateCheckStatus': { 'THROTTLED': 'throttled', 'NO_UPDATE': 'no_update', 'UPDATE_AVAILABLE': 'update_available' }, 'OnInstalledReason': { 'INSTALL': 'install', 'UPDATE': 'update', 'CHROME_UPDATE': 'chrome_update', 'SHARED_MODULE_UPDATE': 'shared_module_update' }, 'OnRestartRequiredReason': { 'APP_UPDATE': 'app_update', 'OS_UPDATE': 'os_update', 'PERIODIC': 'periodic' } } };
        }";

        private const string _window_navigator_chrome = @"
        () => {
            window.navigator.chrome = { 'app': { 'isInstalled': false }, 'webstore': { 'onInstallStageChanged': {}, 'onDownloadProgress': {} }, 'runtime': { 'PlatformOs': { 'MAC': 'mac', 'WIN': 'win', 'ANDROID': 'android', 'CROS': 'cros', 'LINUX': 'linux', 'OPENBSD': 'openbsd' }, 'PlatformArch': { 'ARM': 'arm', 'X86_32': 'x86-32', 'X86_64': 'x86-64' }, 'PlatformNaclArch': { 'ARM': 'arm', 'X86_32': 'x86-32', 'X86_64': 'x86-64' }, 'RequestUpdateCheckStatus': { 'THROTTLED': 'throttled', 'NO_UPDATE': 'no_update', 'UPDATE_AVAILABLE': 'update_available' }, 'OnInstalledReason': { 'INSTALL': 'install', 'UPDATE': 'update', 'CHROME_UPDATE': 'chrome_update', 'SHARED_MODULE_UPDATE': 'shared_module_update' }, 'OnRestartRequiredReason': { 'APP_UPDATE': 'app_update', 'OS_UPDATE': 'os_update', 'PERIODIC': 'periodic' } } };
        }";

        private const string _window_navigator_permissions_query = @"
        () => {
            const originalQuery = window.navigator.permissions.query;
            return window.navigator.permissions.query = (parameters) => (
                parameters.name === 'notifications' ? Promise.resolve({ state: Notification.permission }) : originalQuery(parameters)
            );
        }";

        private const string _webGLRenderingContext_getParameter = @"
        () => {
            const getParameter = WebGLRenderingContext.getParameter;
            WebGLRenderingContext.prototype.getParameter = function(parameter) {
                // UNMASKED_VENDOR_WEBGL
                if (parameter === 37445) {
                  return 'Intel Open Source Technology Center';
                }
                // UNMASKED_RENDERER_WEBGL
                if (parameter === 37446) {
                  return 'Mesa DRI Intel(R) Ivybridge Mobile';
                }
                return getParameter(parameter);
            };
        }";

        #endregion


        /// <summary>
        /// <para>设置一个 Chromium 浏览器 启动选项 的对象，并返回这个对象；此方法兼容 Windows7 / Windows Server 2008</para>
        /// <para>异常可能：程序运行目录下 Chromium 浏览器不可用。</para>
        /// </summary>
        /// <param name="checkIsDownload">检查 是否下载 Chromium 浏览器；默认 false</param>
        /// <param name="isDisplay">Chromium 运行时 是否显示界面；默认 true</param>
        /// <param name="args">要传递给 Chromium 浏览器实例的其他参数。（ 此方法会自动传入 "--no-sandbox" 参数 ）
        /// <para>参考：https://peter.sh/experiments/chromium-command-line-switches/#no-sandbox </para>
        /// </param>
        /// <param name="ignoredDefaultArgs">如果给出数组，则过滤掉给定的 Puppeteer.DefaultArgs 默认参数。
        /// （ 此方法会自动设置 "--enable-automation" 过滤掉这个参数 ）
        /// <para>参考：https://peter.sh/experiments/chromium-command-line-switches/#no-sandbox </para>
        /// </param>
        /// <returns></returns>
        private static async Task<LaunchOptions> SetChromiumLaunchOptions(
            bool checkIsDownload = false,
            bool isDisplay = true,
            string[] args = null,
            string[] ignoredDefaultArgs = null)
        {
            return await Task.Run(async () =>
            {
                BrowserFetcher browserFetcher = Puppeteer.CreateBrowserFetcher(new BrowserFetcherOptions());
                RevisionInfo revisionInfo = browserFetcher.RevisionInfo(BrowserFetcher.DefaultChromiumRevision);

                #region 检查下载 Chromium
                if (!(revisionInfo.Downloaded && revisionInfo.Local))
                {
                    if (checkIsDownload)
                    {
                        // 检查 revisionInfo.Revision 这个版本的 Chromium 浏览器 是否 可下载
                        bool isCan = await browserFetcher.CanDownloadAsync(revisionInfo.Revision);
                        if (isCan)
                        {
                            // 下载 revisionInfo.Revision 这个版本的无头浏览器；可能需要等待一些时间
                            await browserFetcher.DownloadAsync(revisionInfo.Revision);
                        }
                        else
                        {
                            throw new Exception($"程序检测出 Chromium 浏览器（默认版本 {revisionInfo.Revision}）无法更新！");
                        }
                    }
                    else
                    {
                        throw new Exception("程序运行目录下 Chromium 浏览器不可用。请开发人员检查 程序运行目录下 是否正确安装 Chromium 浏览器。");
                    }
                }
                #endregion

                #region 兼容 Windows7 / Windows Server 2008
                LaunchOptions launchOptions;
                // 这个判断是为了兼容 Windows7 和 Windows Server 2008
                if (OSHelper.IsWin7Under())
                {
                    launchOptions = new LaunchOptions
                    {
                        WebSocketFactory = async (uri, socketOptions, cancellationToken) =>
                        {
                            WebSocket client = SystemClientWebSocket.CreateClientWebSocket();
                            if (client is System.Net.WebSockets.Managed.ClientWebSocket managed)
                            {
                                managed.Options.KeepAliveInterval = TimeSpan.FromSeconds(0);
                                await managed.ConnectAsync(uri, cancellationToken);
                            }
                            else
                            {
                                ClientWebSocket coreSocket = (ClientWebSocket)client;
                                coreSocket.Options.KeepAliveInterval = TimeSpan.FromSeconds(0);
                                await coreSocket.ConnectAsync(uri, cancellationToken);
                            }
                            return client;
                        }
                    };
                }
                else
                {
                    launchOptions = new LaunchOptions();
                }
                #endregion

                launchOptions.Headless = !isDisplay; // Headless : true 是无头模式，无界面；false，有界面

                #region 设置 Args 参数
                string[] argss;
                if (args != null && args.Length > 0)
                {
                    List<string> argsList = args.ToList<string>();
                    argsList.Add("--no-sandbox");
                    argss = argsList.ToArray();
                }
                else
                {
                    argss = new string[] { "--no-sandbox" };
                }
                launchOptions.Args = argss; // 这些参数将会传递给 Chromium
                #endregion

                #region 设置 IgnoredDefaultArgs 参数
                string[] defaultArgs;
                if (ignoredDefaultArgs != null && ignoredDefaultArgs.Length > 0)
                {
                    List<string> ignoredDefaultArgsList = ignoredDefaultArgs.ToList<string>();
                    ignoredDefaultArgsList.Add("--enable-automation");
                    defaultArgs = ignoredDefaultArgsList.ToArray();
                }
                else
                {
                    defaultArgs = new string[] { "--enable-automation" };
                }
                launchOptions.IgnoredDefaultArgs = defaultArgs; // 这些参数将被 Chromium 忽略
                #endregion

                return launchOptions;
            });
        }


        /// <summary>
        /// 获取本机 Chromium 浏览器
        /// </summary>
        /// <param name="chromiumBrowserPath">本机 Chromium 浏览器的绝对路径；默认 本机的 Edge 浏览器 绝对路径</param>
        /// <param name="isDisplay">Chromium 运行时 是否显示界面；默认 false</param>
        /// <returns></returns>
        public static LaunchOptions GetNativeChromiumBrowser(string chromiumBrowserPath = null, bool isDisplay = false)
        {
            if (string.IsNullOrWhiteSpace(chromiumBrowserPath))
                chromiumBrowserPath = NativeEdgePath;

            if (!File.Exists(chromiumBrowserPath) && chromiumBrowserPath == NativeEdgePath)
            {
                throw new FileNotFoundException("Please install the Edge browser.");
            }

            return new LaunchOptions()
            {
                ExecutablePath = chromiumBrowserPath,
                Headless = !isDisplay // Headless : true 是无头模式，无界面；false，有界面
            };
        }


        /// <summary>
        /// <para>获取一个 Chromium 浏览器 启动选项 的对象，并返回这个对象；此方法兼容 Windows7 / Windows Server 2008</para>
        /// <para>检查不下载</para>
        /// <para>Chromium 运行时显示界面</para>
        /// <para>自动传入 "--no-sandbox" 参数</para>
        /// <para>自动过滤掉 "--enable-automation" 参数</para>
        /// <para>△ 异常可能：程序运行目录下 Chromium 浏览器不可用。</para>
        /// </summary>
        /// <returns></returns>
        public static async Task<LaunchOptions> ChromiumLaunchOptions()
        {
            return await SetChromiumLaunchOptions(false, true, null, null);
        }

        /// <summary>
        /// <para>获取一个 Chromium 浏览器 启动选项 的对象，并返回这个对象；此方法兼容 Windows7 / Windows Server 2008</para>
        /// <para>△ 异常可能：程序运行目录下 Chromium 浏览器不可用。</para>
        /// </summary>
        /// <param name="args">要传递给 Chromium 浏览器实例的其他参数。（ 此方法会自动传入 "--no-sandbox" 参数 ）</param>
        /// <param name="ignoredDefaultArgs">如果给出数组，则过滤掉给定的 Puppeteer.DefaultArgs 默认参数。
        /// （ 此方法会自动设置 "--enable-automation" 过滤掉这个参数 ）
        /// </param>
        /// <returns></returns>
        public static async Task<LaunchOptions> ChromiumLaunchOptions(string[] args,
            string[] ignoredDefaultArgs = null)
        {
            return await SetChromiumLaunchOptions(false, true, args, ignoredDefaultArgs);
        }

        /// <summary>
        /// <para>获取一个 Chromium 浏览器 启动选项 的对象，并返回这个对象；此方法兼容 Windows7 / Windows Server 2008</para>
        /// <para>△ 异常可能：程序运行目录下 Chromium 浏览器不可用。</para>
        /// </summary>
        /// <param name="isDisplay">Chromium 运行时 是否显示界面</param>
        /// <param name="args">要传递给 Chromium 浏览器实例的其他参数。（ 此方法会自动传入 "--no-sandbox" 参数 ）</param>
        /// <param name="ignoredDefaultArgs">如果给出数组，则过滤掉给定的 Puppeteer.DefaultArgs 默认参数。
        /// （ 此方法会自动设置 "--enable-automation" 过滤掉这个参数 ）
        /// </param>
        /// <returns></returns>
        public static async Task<LaunchOptions> ChromiumLaunchOptions(bool isDisplay,
            string[] args = null, string[] ignoredDefaultArgs = null)
        {
            return await SetChromiumLaunchOptions(false, isDisplay, args, ignoredDefaultArgs);
        }

        /// <summary>
        /// <para>获取一个 Chromium 浏览器 启动选项 的对象，并返回这个对象；此方法兼容 Windows7 / Windows Server 2008</para>
        /// <para>△ 异常可能：程序运行目录下 Chromium 浏览器不可用。</para>
        /// </summary>
        /// <param name="checkIsDownload">检查 是否下载 Chromium 浏览器</param>
        /// <param name="isDisplay">Chromium 运行时 是否显示界面</param>
        /// <param name="args">要传递给 Chromium 浏览器实例的其他参数。（ 此方法会自动传入 "--no-sandbox" 参数 ）</param>
        /// <param name="ignoredDefaultArgs">如果给出数组，则过滤掉给定的 Puppeteer.DefaultArgs 默认参数。
        /// （ 此方法会自动设置 "--enable-automation" 过滤掉这个参数 ）
        /// </param>
        /// <returns></returns>
        public static async Task<LaunchOptions> ChromiumLaunchOptions(bool checkIsDownload, bool isDisplay,
            string[] args = null, string[] ignoredDefaultArgs = null)
        {
            return await SetChromiumLaunchOptions(checkIsDownload, isDisplay, args, ignoredDefaultArgs);
        }


        /// <summary>
        /// 新建一个 Page（页面）并且初始化后再返回当前 Page 对象（页面）【避免js检测出 当前客户行为是无头浏览器自动化程序】
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="isPhone">是否是手机端页面，默认 false</param>
        /// <param name="pageTimeout">page 超时时间，单位(s)秒；仅大于 0 时，设置该参数</param>
        /// <param name="pageWidth">页面显示区的 宽 px；0 表示不限制</param>
        /// <param name="pageHeight">页面显示区的 高 px；0 表示不限制</param>
        /// <returns></returns>
        public static async Task<Page> NewPageAndInitAsync(Browser browser,
            bool isPhone = false, int pageTimeout = 0, int pageWidth = 0, int pageHeight = 0)
        {
            if (browser == null)
            {
                throw new Exception("传入了一个空的 Chromium 浏览器对象。Browser == null");
            }

            Page page;
            Page[] pages = await browser.PagesAsync();
            if (pages != null && pages.Length == 1 && pages[0].Url == "about:blank")
            {
                page = pages[0];
            }
            else
            {
                page = await browser.NewPageAsync();
            }

            #region 定义浏览器页面属性（这里是为了绕过反爬虫的js检测）
            if (isPhone)
                await page.SetUserAgentAsync(_userAgentPhone);
            else
                await page.SetUserAgentAsync(_userAgent);

            await page.EvaluateFunctionOnNewDocumentAsync(_navigator_languages);
            await page.EvaluateFunctionOnNewDocumentAsync(_navigator_webdriver);
            await page.EvaluateFunctionOnNewDocumentAsync(_navigator_connection_rtt);
            await page.EvaluateFunctionOnNewDocumentAsync(_navigator_plugins);
            await page.EvaluateFunctionOnNewDocumentAsync(_chrome);
            await page.EvaluateFunctionOnNewDocumentAsync(_window_chrome);
            await page.EvaluateFunctionOnNewDocumentAsync(_window_navigator_chrome);
            await page.EvaluateFunctionOnNewDocumentAsync(_window_navigator_permissions_query);
            await page.EvaluateFunctionOnNewDocumentAsync(_webGLRenderingContext_getParameter);
            #endregion

            await page.SetViewportAsync(new ViewPortOptions
            {
                Width = pageWidth,
                Height = pageHeight
            });

            if (pageTimeout > 0)
            {
                page.DefaultNavigationTimeout = pageTimeout * 1000;
                page.DefaultTimeout = pageTimeout * 1000;
            }

            return page;
        }


        /// <summary>
        /// 保存 Page 页面截图
        /// </summary>
        /// <param name="page">Chromium 的 Page 对象</param>
        /// <param name="path">保存截图的绝对路径</param>
        /// <param name="isFullPage">true 获取整个可滚动页面的屏幕截图；false 获取页面可见部分的屏幕截图。默认值：false</param>
        /// <returns></returns>
        public static async Task SavePageScreenshotAsync(Page page, string path, bool isFullPage = false)
        {
            await SavePageScreenshotAsync(page, new ScreenshotOptions() { FullPage = isFullPage }, path);
        }

        /// <summary>
        /// 保存 Page 页面截图
        /// </summary>
        /// <param name="page">Chromium 的 Page 对象</param>
        /// <param name="screenshotOptions">ScreenshotOptions 截图选项 【 如果传入空值，则自动实例化一个 ScreenshotOptions 对象 】</param>
        /// <param name="path">保存截图的绝对路径</param>
        /// <returns></returns>
        public static async Task SavePageScreenshotAsync(Page page, ScreenshotOptions screenshotOptions, string path)
        {
            if (page == null)
            {
                throw new Exception("传入了一个空的 Chromium Page 对象。Page == null");
            }

            if (screenshotOptions == null)
            {
                screenshotOptions = new ScreenshotOptions();
            }

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            if (Path.GetExtension(path).ToLower() != ".png")
            {
                path += ".png";
            }

            await page.ScreenshotAsync(path, screenshotOptions);
        }


        /// <summary>
        /// 获取 html 元素的内容
        /// </summary>
        /// <param name="page"></param>
        /// <param name="selector">选择器，示例： "div[class='name']" , "#name"</param>
        /// <param name="propertyName">属性名，示例： "textContent" , "href"</param>
        /// <returns></returns>
        public static async Task<string> GetElementContentAsync(this Page page, string selector, string propertyName)
        {
            var el = await page.QuerySelectorAsync(selector);
            var js = await el?.GetPropertyAsync(propertyName);
            string res = await js?.JsonValueAsync<string>();
            return res;
        }

        /// <summary>
        /// GoTo 之后等待一会儿 （ 对原有 GoTo 方法的加工 ）
        /// </summary>
        /// <param name="page"></param>
        /// <param name="url"></param>
        /// <param name="waitingTime">等待时长，单位(ms)毫秒；默认等待 2~3 秒</param>
        /// <returns></returns>
        public static async Task<Response> GotoWaitAfterAsync(this Page page, string url, int waitingTime = 0)
        {
            var res = await page.GoToAsync(url);
            if (waitingTime < 1)
                await Task.Delay(new Random().Next(2000, 3000));
            else
                await Task.Delay(waitingTime);
            return res;
        }
    }
}
