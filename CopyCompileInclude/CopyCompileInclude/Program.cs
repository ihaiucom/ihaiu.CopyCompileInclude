using System;
using System.IO;
using System.Xml;

namespace CopyCompileInclude
{
    class Program
    {
        static void Main(string[] args)
        {
            ProgramSetting setting = ProgramSetting.Load();
            if (!ProgramSetting.IsExists())
                setting.Generate();

            if(!File.Exists(setting.csproj))
            {
                ErrorFormat("no find file!! setting.csproj={0}", setting.csproj);
                Console.ReadLine();
                Console.WriteLine("Press 'Enter' key To End");
                return;
            }

            int index = 0;
            XmlDocument doc=new XmlDocument(); 
            doc.Load(setting.csproj); 


            XmlNodeList xnl = doc.ChildNodes[0].ChildNodes;
            if (doc.ChildNodes[0].Name.ToLower() != "project")
                xnl = doc.ChildNodes[1].ChildNodes;

            foreach(XmlNode xn in xnl)
            {
                if (xn.ChildNodes.Count > 0 && xn.Name.ToLower() == "itemgroup")
                {
                    foreach (XmlNode cxn in xn.ChildNodes)
                    {
                        if(cxn.Name.ToLower() == "compile")
                        {
                            XmlElement compile = (XmlElement) cxn;
                            if(compile.HasAttribute("Include") && compile.HasAttribute("Link"))
                            {
                                string include = compile.GetAttribute("Include");
                                string link = compile.GetAttribute("Link");

                                string src = Path.Combine(setting.rootSrc, include);
                                string dest = Path.Combine(setting.rootDest, link);

                                src = src.Replace("\\", "/");
                                dest = dest.Replace("\\", "/");

                                if (!File.Exists(src))
                                {
                                    ErrorFormat("No Find File: {0}", src);
                                    continue;
                                }

                                if(setting.replace == false && File.Exists(dest))
                                {
                                    LogFormat("Skip File: {0}", src);
                                }

                                string destDir = Path.GetDirectoryName(dest);
                                if (!Directory.Exists(destDir))
                                    Directory.CreateDirectory(destDir);

                                File.Copy(src, dest, setting.replace);

                            }
                        }
                    }
                }
            }




        }

        static ConsoleColor tmpColor;

        static void ErrorFormat(string format, params object[] args)
        {
            Error(string.Format(format, args));
        }

        static void Error(object msg)
        {
            tmpColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("" );
            Console.WriteLine("[Error]: "+ msg);
            Console.WriteLine("");
            Console.ForegroundColor = tmpColor;
        }


        static void LogFormat(string format, params object[] args)
        {
            Log(string.Format(format, args));
        }

        static void Log(object msg)
        {
            tmpColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[Log]: " + msg);
            Console.ForegroundColor = tmpColor;
        }

    }
}
