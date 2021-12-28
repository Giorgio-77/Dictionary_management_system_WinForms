using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    public class ListLang
    {
        public class Languages : IComparable<Languages>
        {
            public string Name { get; set; }
            public string ISO { get; set; }

            public Languages() {}
            public Languages(string name, string iso)
                => (Name, ISO) = (name, iso);

            public override string ToString()
            {
                return $"{Name} [{ISO}]";
            }

            public int CompareTo(Languages other)
            {
                return this.Name.CompareTo(other.Name);
            }
        }

        public List<Languages> List { get; set; }

        public ListLang()
        {
            List = new List<Languages>();
            List.Add(new Languages("Арабська", "ara"));
            List.Add(new Languages("Білоруська", "bel"));
            List.Add(new Languages("Болгарська", "bul"));
            List.Add(new Languages("Німецька", "deu"));
            List.Add(new Languages("Англійська", "eng"));
            List.Add(new Languages("Есперанто", "epo"));
            List.Add(new Languages("Іспанська", "spa"));
            List.Add(new Languages("Французька", "fra"));
            List.Add(new Languages("Іврит", "heb"));
            List.Add(new Languages("Італійська", "ita"));
            List.Add(new Languages("Латинська", "lat"));
            List.Add(new Languages("Польська", "pol"));
            List.Add(new Languages("Українська", "ukr"));
            List.Add(new Languages("Китайська", "zho"));
            List.Sort();
        }

    }
}
