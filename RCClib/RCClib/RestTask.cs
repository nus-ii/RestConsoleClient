using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RCClib
{
	public class RestTask
	{
		public Task<string> responceTask;
		public JObject request;

		public RestTask(JObject req,Task<string> res)
		{
			this.responceTask = res;
			this.request = req;
		}
	}
}
