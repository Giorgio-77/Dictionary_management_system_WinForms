using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dictionary
{
    [Serializable]
    public class Dictionary : IComparable<Dictionary>
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string Title { get; set; }

        public List<Word> ListWord { get; set; }

        public Dictionary()
        {
            ListWord = new List<Word>();
        }

        public Dictionary(string name, string path, string title)
        {
            Name = name;
            Path = path;
            Title = title;
            ListWord = new List<Word>();
            if (!File.Exists(Path))
                File.Create(Path).Close();
        }

        public void AddWord(Word word)
        {
            ListWord.Add(word);
        }

        public void DellWord(int pos)
        {
            ListWord[pos].DelAllValues();
            ListWord.RemoveAt(pos);
        }

        public Word FindWord(string word)
        {
            return ListWord.Find(item => item.Key == word);
        }

        public void PrintGrouped(ListBox listBox)
        {
            char tmp = '0';
            foreach (var item in ListWord)
            {
                if (item == ListWord.First() || tmp != item.Key[0])
                {
                    if (item != ListWord.First())
                        listBox.Items.Add("\n");
                    tmp = item.Key[0];
                    listBox.Items.Add("" + tmp.ToString().ToUpper());
                }

                listBox.Items.Add("  " + item.ToString());
            }
        }

        public void Print()
        {
            Console.WriteLine($"\tDictionary {Name}\n" + new string('-', 40));
            foreach (var item in ListWord)
            {
                Console.WriteLine(item.ToString());
            }
        }

        public void Sort()
        {
            ListWord.Sort();
        }

        public override string ToString()
        {
            return Title;
        }

        public int CompareTo(Dictionary other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
