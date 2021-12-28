using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    [Serializable]
    public class Log
    {
        public string Path { get; set; }

        public string FileName { get; set; } = @"logs.xml";

        public List<LogString> List { get; set; }

        public Log()
        {
            List = new List<LogString>();
        }

        public Log(string folderPath)
        {
            List = new List<LogString>();
            Path = folderPath + FileName;
        }

        public void Add(LogString str)
        {
            List.Add(str);
        }

        public void ClearList()
        {
            List.Clear();
        }

        public void Print()
        {
            Console.WriteLine("\tІсторія пошуку\n\t" + new string('-', 30));
            foreach (var item in List)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();
        }
    }
}
