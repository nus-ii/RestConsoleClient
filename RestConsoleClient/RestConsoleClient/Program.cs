using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RCClib;

namespace RestConsoleClient
{
	class Program
	{
		static void Main(string[] args)
		{
			Settings settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(@"C:\ClientImport\SettingsRCCclient.json"));
			var result=RESTprocess(settings);
			PrintConsole(result);
			Console.ReadLine();
			
		}

		private static void PrintConsole(List<string> result)
		{
			foreach (var l in result)
			{
				Console.WriteLine(l);
				var t = JObject.Parse(l);
			}
		}

		private static List<string> RESTprocess(Settings settings)
		{
			List<string> result=new List<string>();
			string data = File.ReadAllText(settings.DataFile);
			JArray dataJArray = JObject.Parse(data)["result"] as JArray;
			JObject pattern=JObject.Parse(File.ReadAllText(settings.Pattern));
			foreach (var target in dataJArray)
			{
				var temp = SetToPattern((JObject)target,pattern);
				string resp = RESTadapter.GetData(temp.ToString(), settings.Id, settings.Portal, settings.Product).Result;
			    result.Add(resp);
			}
			//if (!string.IsNullOrEmpty(settings.ReplaceFlag))
			//{
			//	data = data.Replace(settings.ReplaceFlag, settings.ReplaceValue);
			//}
			//RESTadapter.GetData(data, settings.Id, settings.Portal, settings.Product).Result;
			return result;
		}

		private static JObject SetToPattern(JObject target, JObject pattern)
		{
			pattern["request"]["card"] = target;
			return pattern;
		}
	}
}
