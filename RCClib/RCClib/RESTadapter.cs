using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RCClib
{
	public class RESTadapter
	{
		public static async Task<string> GetData(string body, string session, string a, string b)
		{
			Uri mPortalUrl = new Uri(a);
			Uri url = new Uri(string.Concat(a, b));
			var cookieContainer = new CookieContainer();
			var cookie = new Cookie("SSID", session);
			cookieContainer.Add(mPortalUrl, cookie);
			cookieContainer.Add(mPortalUrl, new Cookie("PHPSESSID", session));
			using (HttpClient client = new HttpClient(new HttpClientHandler() { CookieContainer = cookieContainer }))
			{
				client.BaseAddress = mPortalUrl;
				var response = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
				response.EnsureSuccessStatusCode();
				var result = await response.Content.ReadAsStringAsync();
				return result;
			}
		}

	}
}
