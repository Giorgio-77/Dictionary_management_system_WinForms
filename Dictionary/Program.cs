using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dictionary
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 

        public static void BinaryRead(DictCollection dictCollection)
        {
            using (FileStream fileStream = new FileStream(dictCollection.Path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    while (binaryReader.PeekChar() > -1)
                    {
                        string tmp_name = binaryReader.ReadString();
                        string tmp_path = binaryReader.ReadString();
                        string tmp_title = binaryReader.ReadString();
                        if (File.Exists(tmp_path) && (new FileInfo(tmp_path).Length != 0))
                        {
                            Dictionary tmpDict = (Dictionary)XmlSerDeser.Deserialize("Dictionary", tmp_path);
                            tmpDict.Sort();
                            dictCollection.Add(tmpDict);
                        }
                        else
                            dictCollection.Add(new Dictionary(tmp_name, tmp_path, tmp_title));
                    }
                }
            }
        }

        public static void BinaryWrite(DictCollection dictCollection)
        {
            using (FileStream fileStream = new FileStream(dictCollection.Path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    foreach (var item in dictCollection.DictList)
                    {
                        binaryWriter.Write(item.Name);
                        binaryWriter.Write(item.Path);
                        binaryWriter.Write(item.Title);
                    }

                }
            }
        }


        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
