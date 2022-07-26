using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutoCrawlerTool.Common
{
    /// <summary>
    /// 检查互联网 工具类
    /// </summary>
    public static class CheckInternetTool
    {
        private static readonly HttpClient _http = new HttpClient();

        /// <summary>
        /// 检查互联网是否可用
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> InternetIsUseableAsync()
        {
            try
            {
                var response = await _http.GetAsync("https://www.baidu.com/");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
