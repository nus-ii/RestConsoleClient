using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using RCClib;

namespace RestConsoleClient
{
	class Program
	{
		static void Main(string[] args)
		{
			Settings settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(@"C:\PrivateData\SettingsRCCA.json"));
			string result=RESTprocess(settings);
			Console.WriteLine(result);
			Console.ReadLine();
			
		}

		private static string RESTprocess(Settings settings)
		{
			string data = File.ReadAllText(settings.DataFile);
			if (!string.IsNullOrEmpty(settings.ReplaceFlag))
			{
				data = data.Replace(settings.ReplaceFlag, settings.ReplaceValue);
			}
			string result = RESTadapter.GetData(data, settings.Id, settings.Portal, settings.Product).Result;
			return result;
		}
	}
}
