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
			List<string> log=new List<string>();
			PrintConsole(result,ref log);
			WriteLog(log,settings);
			Console.ReadLine();
		}

		private static void WriteLog(List<string> log, Settings settings)
		{
			string result = "";

			foreach (var l in log)
			{
				result = result + l + "\r\n";
			}

			File.WriteAllText(settings.LogFile,result);
		}

		private static void PrintConsole(List<RestTask> result, ref List<string> log)
		{
			foreach (var l in result)
			{
				Console.WriteLine(l.responceTask.Result);
				log.Add(l.responceTask.Result);
			}
		}

		private static List<RestTask> RESTprocess(Settings settings)
		{
			List<RestTask> result=new List<RestTask>();
			string data = File.ReadAllText(settings.DataFile);
			JArray dataJArray = JObject.Parse(data)["result"] as JArray;
			JObject pattern=JObject.Parse(File.ReadAllText(settings.Pattern));

			foreach (var target in dataJArray)
			{
				var temp = SetToPattern((JObject)target,pattern);
				var resp = RESTadapter.GetData(temp.ToString(), settings.Id, settings.Portal, settings.Product);
			    result.Add(new RestTask(temp,resp));
			}

			return result;
		}

		private static JObject SetToPattern(JObject target, JObject pattern)
		{
			pattern["request"]["card"] = target;
			return pattern;
		}
	}
}
