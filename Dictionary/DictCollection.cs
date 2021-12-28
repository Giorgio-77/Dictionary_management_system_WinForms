using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    [Serializable]
    public class DictCollection
    {
        public string Path { get; set; }
        public string FileName { get; set; } = @"dict_list.dat";
        public List<Dictionary> DictList { get; set; }

        public DictCollection(string folderPath)
        {
            Path = folderPath + FileName;
            DictList = new List<Dictionary>();
        }

        public void Add(Dictionary dictionary)
        {
            DictList.Add(dictionary);
        }

        public int Size
        {
            get
            {
                return DictList.Count();
            }
        }

        public Dictionary this[int pos]
        {
            get => DictList[pos];
        }
    }
}
