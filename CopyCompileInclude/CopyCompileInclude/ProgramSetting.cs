using System;
using System.Collections.Generic;
using System.IO;
/** 
* ==============================================================================
*  @Author          曾峰(zengfeng75@qq.com) 
*  @Web             http://blog.ihaiu.com
*  @CreateTime      3/19/2018 4:03:16 PM
*  @Description:    
* ==============================================================================
*/
namespace CopyCompileInclude
{
    public class ProgramSetting
    {
        public string   csproj = "/Users/zengfeng/workspaces/www/githubpages/learn.ET/learnET3/ETServer/Server/Model/Server.Model.csproj";
        public string   rootSrc = "/Users/zengfeng/workspaces/www/githubpages/learn.ET/ET/Unity/Assets/";
        public string   rootDest = "../../Server/Model";
        public bool     replace = false;


        public void Print()
        {
            Console.WriteLine("csproj=" + csproj);
            Console.WriteLine("rootSrc=" + rootSrc);
            Console.WriteLine("rootDest=" + rootDest);
            Console.WriteLine("replace=" + replace);
            Console.WriteLine("");
        }

        public static string path = "setting.txt";
        public static bool IsExists()
        {
            return File.Exists(path);
        }

        public void Generate()
        {
            StringWriter sw = new StringWriter();

            sw.WriteLine("csproj=" + csproj);
            sw.WriteLine("rootSrc=" + rootSrc);
            sw.WriteLine("rootDest=" + rootDest);
            sw.WriteLine("replace=" + replace);
            sw.WriteLine("");

            File.WriteAllText(path, sw.ToString());
        }


        public static ProgramSetting Load()
        {
            return Load(path);
        }

        public static ProgramSetting Load(string path)
        {
            ProgramSetting o = new ProgramSetting();

            Dictionary<string, string> settingDict = new Dictionary<string, string>();
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] arr = line.Split('=');
                    if (arr.Length == 2)
                    {
                        string key = arr[0].Trim();
                        string val = arr[1].Trim();
                        settingDict[key] = val;
                    }
                }
            }

            if (settingDict.ContainsKey("csproj"))
                o.csproj = settingDict["csproj"];

            if (settingDict.ContainsKey("rootSrc"))
                o.rootSrc = settingDict["rootSrc"];
            
            if (settingDict.ContainsKey("rootDest"))
                o.rootDest = settingDict["rootDest"];

            if (settingDict.ContainsKey("replace"))
                o.replace = settingDict["replace"] == "true" || settingDict["replace"] == "1";
            

            return o;
        }
    }
}
