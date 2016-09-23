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

			Settings settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(@"C:\HardPrint\SettingsRCCclient.json"));
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
				string code = "";
				string msg = "";
				JObject jResp=new JObject();
				try
				{
					jResp = JObject.Parse(l.responceTask.Result);
					code = jResp["code"].Value<string>();
					msg = jResp["msg"].Value<string>() != null ? jResp["msg"].Value<string>() : "null";
				}
				catch (Exception)
				{
					msg = "Error!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!";//l.responceTask.Result.ToString();
				}
				//var jResp = JObject.Parse(l.responceTask.Result);
				//string code = jResp["code"].Value<string>();
				//string msg = jResp["msg"].Value<string>()!=null?jResp["msg"].Value<string>():"null";
				if (code.Equals("400"))
				{
					Console.WriteLine(string.Format(" code:{0} msg:{1} ----------------------------------------------------------------------------", code, msg));
				}

				if (code.Equals("200"))
				{
					Console.Write(string.Format(" code:{0} msg:{1} ", code, msg));
				}
				//Console.WriteLine(l.responceTask.Result);
				log.Add(l.responceTask.Result);
			}
		}

		private static List<RestTask> RESTprocess(Settings settings)
		{
			List<RestTask> result=new List<RestTask>();
			string data = File.ReadAllText(settings.DataFile);
			JArray dataJArray = JObject.Parse(data)["result"] as JArray;
			JObject pattern=JObject.Parse(File.ReadAllText(settings.Pattern));

			for (int q=0;q<199;q++)//var target in dataJArray)
			{
				var temp = pattern;//SetToPattern((JObject)target,pattern);
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
