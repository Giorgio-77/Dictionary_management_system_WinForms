using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dictionary
{
    public partial class Form1 : Form
    {
        public string FolderPath { get; set; } = @"Datas/";
        public string DictPath { get; set; }
        public string DictName { get; set; }
        public string DictTitle { get; set; }
        
        public string DictFileName { get; set; }

        int d_pos, w_pos, t_pos;

        //timer
        int count = 0;

        string tmpRes = "";

        // Створюємо об*єкти
        DictCollection coll;
        Log log;
        ListLang listLang;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            coll = new DictCollection(FolderPath);
            log = new Log(FolderPath);
            listLang = new ListLang();


            // Перевіряємо наявність файлів і виконуємо дії
            if (!Directory.Exists("Datas"))
            {
                Directory.CreateDirectory("Datas");
                File.Create(coll.Path).Close();
                File.Create(log.Path).Close();
            }
            else
            {
                if (!File.Exists(coll.Path))
                    File.Create(coll.Path).Close();
                else if (new FileInfo(coll.Path).Length != 0)
                    Program.BinaryRead(coll);

                if (!File.Exists(log.Path))
                    File.Create(log.Path).Close();
                else if (new FileInfo(log.Path).Length != 0)
                    log = (Log)XmlSerDeser.Deserialize("Log", log.Path);
            }

            if (coll.DictList.Count != 0)
            {
                comboBox1.Items.AddRange(coll.DictList.ToArray());
            }

            comboBox1.SelectedIndex = 0;            

            // Заповнюємо ліст мовами на вкладці "додати"
            listBox1.Items.AddRange(listLang.List.ToArray());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            DictFileName = "";
            textBox4.Text = DictFileName;
            label13.Text = "";
            if (listBox1.SelectedIndex == -1)
            {
                Button3Enable();
                return;
            }
            for (int i = 0; i < listLang.List.Count; i++)
            {
                if (i != listBox1.SelectedIndex)
                    listBox2.Items.Add(listLang.List[i]);
            }
            Button3Enable();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pos;
            if (listBox2.SelectedIndex < listBox1.SelectedIndex)
                pos = listBox2.SelectedIndex;
            else
                pos = listBox2.SelectedIndex + 1;

            DictTitle = listLang.List[listBox1.SelectedIndex].Name + " -> " + listLang.List[pos].Name;
            DictName = listLang.List[listBox1.SelectedIndex].ISO + "-" + listLang.List[pos].ISO;
            DictFileName = DictName + ".xml";

            DictPath = FolderPath + DictFileName;

            textBox4.Text = DictPath;
            label13.Text = DictTitle;
            Button3Enable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = -1;
        }

        private void Button3Enable()
        {
            if (listBox1.SelectedIndex == -1 || listBox2.SelectedIndex == -1)
                button3.Enabled = false;
            else
                button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ((coll.DictList.FindIndex(item => item.Name == DictName)) == -1 )
            {
                coll.DictList.Add(new Dictionary(DictName, DictPath, DictTitle));
                coll.DictList.Sort();
                Program.BinaryWrite(coll);
                MessageBox.Show($"Напрямок перекладу {DictTitle}\nдодано успішно!", "Tranlator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show($"Напрямок перекладу {DictTitle} вже існує!\nОперацію скасовано!", "Tranlator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                listBox1.SelectedIndex = -1;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                groupBox2.Enabled = false;
                groupBox1.Enabled = true;
                button9_Click(sender, e);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                listBox1.SelectedIndex = -1;
                groupBox2.Enabled = true;
                groupBox1.Enabled = false;
                button9_Click(sender, e);
                comboBox4.Items.Clear();
                comboBox4.Items.Add("-- Не вибрано --");
                comboBox4.Items.AddRange(coll.DictList.ToArray());
                comboBox4.SelectedIndex = 0;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("-- Не вибрано --");
                    comboBox1.Items.AddRange(coll.DictList.ToArray());
                    comboBox1.SelectedIndex = 0;
                    break;
                case 1:
                    radioButton2.Checked = true;
                    comboBox4.Items.Clear();
                    comboBox4.Items.Add("-- Не вибрано --");
                    comboBox4.Items.AddRange(coll.DictList.ToArray());
                    comboBox4.SelectedIndex = 0;
                    break;
                case 2:
                    radioButton3.Checked = true;
                    comboBox2.Items.Clear();
                    comboBox2.Items.Add("-- Не вибрано --");
                    comboBox2.Items.AddRange(coll.DictList.ToArray());
                    comboBox2.SelectedIndex = 0;

                    comboBox3.Items.Clear();
                    comboBox3.Items.Add("-- Не вибрано --");
                    comboBox3.Items.AddRange(coll.DictList.ToArray());
                    comboBox3.SelectedIndex = 0;
                    break;
                case 3:
                    listBox7.Items.Clear();
                    listBox7.Items.AddRange(log.List.ToArray());
                    break;
                default:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            d_pos = comboBox4.SelectedIndex - 1; ;

            if (d_pos < 0)
                return;

            if (coll.DictList[d_pos].ListWord.FindIndex(item => item.Key == textBox3.Text) != -1)
            {
                MessageBox.Show($"Cлово {textBox3.Text} вже у словнику.\nОперацію скасовано!", "Tranlator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                listBox1.SelectedIndex = -1;
                return;
            }

            Word tmpWord = new Word();
            tmpWord.Key = textBox3.Text;
            string[] tmpStr = textBox7.Text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            tmpWord.ListValue.AddRange(tmpStr.ToArray());

            
            coll[d_pos].AddWord(tmpWord);
            coll[d_pos].Sort();
            XmlSerDeser.Serialize(coll[d_pos], coll[d_pos].Path);

            label14.Visible = true;
            count = 0;
            timer1.Start();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            log.List.Clear();
            listBox7.Items.Clear();
            File.WriteAllText(log.Path, String.Empty);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0)
            {
                textBox1.Visible = false;
                listBox8.Visible = false;
                radioButton5.Visible = false;
                radioButton6.Visible = false;
                listBox4.Visible = false;
                label12.Visible = false;
                button10.Visible = false;
                return;
            }

            d_pos = comboBox1.SelectedIndex - 1;
            if (coll.DictList[d_pos].ListWord.Count == 0)
            {
                textBox1.Visible = false;
                listBox8.Visible = false;
                radioButton5.Visible = false;
                radioButton6.Visible = false;
                listBox4.Visible = false;
                label12.Visible = true;
                button10.Visible = false;
                return;
            }

            textBox1.Clear();
            textBox1.Visible = true;

            listBox8.Items.Clear();
            listBox8.Visible = true;

            listBox4.Items.Clear();
            listBox4.Visible = true;

            label12.Visible = false;

            radioButton5.Visible = true;
            radioButton5.Checked = true;
            radioButton6.Visible = true;

            button10.Visible = true;

            listBox4.Items.Clear();
            coll.DictList[d_pos].PrintGrouped(listBox4);
        }

        
        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox4.SelectedIndex = listBox4.FindString("  " + listBox8.SelectedItem);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                listBox8.Items.Clear();
                listBox4.SelectedIndex = -1;
                tmpRes = "";
                return;
            }
            var result = coll.DictList[d_pos].ListWord.Where(item => item.Key.Contains(textBox1.Text)).ToArray();
            if (result.Length != 1)
            {
                listBox8.Items.Clear();
                listBox8.Items.AddRange(result.ToArray());
                listBox4.SelectedIndex = -1;
            }
            else if(result.Length == 1 && tmpRes != result[0].ToString())
            {
                listBox8.Items.Clear();
                listBox8.Items.AddRange(result.ToArray());
                tmpRes = result[0].ToString();
                listBox8.SelectedIndex = 0;
                string[] tmp = listBox8.SelectedItem.ToString().Split(' ');
                log.Add(new LogString(DateTime.Now, tmp[0], coll[d_pos].Name));
                XmlSerDeser.Serialize(log, log.Path);
            }
            else
            {
                listBox8.SelectedIndex = 0;
            }

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            string str = "";
            if (radioButton5.Checked)
            {
                if(listBox4.SelectedIndex != -1)
                    str = listBox4.SelectedItem.ToString();

                listBox4.Items.Clear();
                coll.DictList[d_pos].PrintGrouped(listBox4);
                if (textBox1.Text != "" && str != "")
                    listBox4.SelectedIndex = listBox4.FindString(str);
            }

        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            string str = "";
            if (radioButton6.Checked)
            {
                if (listBox4.SelectedIndex != -1)
                    str = listBox4.SelectedItem.ToString();
                listBox4.Items.Clear();

                var result = from item in coll[d_pos].ListWord
                             orderby item descending
                             select item;

                char tmp = '0';
                foreach (var item in result)
                {
                    if (item == result.First() || tmp != item.Key[0])
                    {
                        if (item != result.First())
                            listBox4.Items.Add("\n");
                        tmp = item.Key[0];
                        listBox4.Items.Add("" + tmp.ToString().ToUpper());
                    }

                    listBox4.Items.Add("  " + item.ToString());
                }
                if (textBox1.Text != "" && str != "")
                    listBox4.SelectedIndex = listBox4.FindString(str);
            }
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void Button9Enable()
        {
            if (comboBox4.SelectedIndex == 0 || textBox3.Text == "" || textBox7.Text == "")
            {
                button1.Enabled = false;
            }
            else
                button1.Enabled = true;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex != 0)
            {
                textBox3.Enabled = true;
                textBox7.Enabled = true;
            }
            else
            {
                textBox3.Enabled = false;
                textBox7.Enabled = false;
            }
            textBox3.Clear();
            textBox7.Clear();
            Button9Enable();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            comboBox4.SelectedIndex = 0;
            textBox3.Clear();
            textBox7.Clear();
            Button9Enable();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            Button9Enable();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if(count != 20)
                ++count;
            else
            {
                timer1.Stop();
                label14.Visible = false;
                textBox3.Clear();
                textBox7.Clear();
                textBox3.Focus();
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                listBox5.Items.Clear();
                button5.Enabled = false;
                return;
            }
            var result = coll.DictList[d_pos].ListWord.Where(item => item.Key.Contains(textBox5.Text)).ToArray();
            listBox5.Items.Clear();
            listBox5.Items.AddRange(result.ToArray());
            if (listBox5.SelectedIndex == -1)
                button5.Enabled = false;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                label15.Visible = false;
                label5.Visible = false;
                textBox5.Visible = false;
                
                label2.Visible = false;
                listBox5.Visible = false;
                
                button4.Visible = false;
                button5.Visible = false;
                return;
            }
            d_pos = comboBox2.SelectedIndex - 1;
            if (coll.DictList[d_pos].ListWord.Count == 0)
            {
                label15.Visible = true;
                label5.Visible = false;
                textBox5.Visible = false;
                label2.Visible = false;
                listBox5.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                return;
            }

            label15.Visible = false;
            label5.Visible = true;
            textBox5.Visible = true;
            label2.Visible = true;
            listBox5.Visible = true;
            button4.Visible = true;
            button5.Visible = true;

            textBox5.Clear();
            listBox5.Items.Clear();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox5.Clear();
            button5.Enabled = false;
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox5.SelectedIndex != -1)
            {
                button5.Enabled = true;
            }
            else
                button5.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            d_pos = comboBox2.SelectedIndex - 1;
            string[] tmpStrArr = listBox5.SelectedItem.ToString().Split(' ');

            var result = MessageBox.Show($"Видалити слово \"{tmpStrArr[0]}\" ?", "Translator", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                w_pos = coll.DictList[d_pos].ListWord.FindIndex(item => item.Key == tmpStrArr[0]);
                coll.DictList[d_pos].DellWord(w_pos);
                XmlSerDeser.Serialize(coll[d_pos], coll[d_pos].Path);
                MessageBox.Show($"Cлово \"{tmpStrArr[0]}\" видалено!", "Translator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            string tmp = textBox5.Text;
            textBox5.Clear();
            textBox5.Text = tmp;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton3.Checked)
            {
                groupBox4.Enabled = false;
                groupBox3.Enabled = true;
                comboBox3.SelectedIndex = 0;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                groupBox3.Enabled = false;
                groupBox4.Enabled = true;
                comboBox2.SelectedIndex = 0;

            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                label9.Visible = false;
                label16.Visible = false;
                label8.Visible = false;
                label11.Visible = false;

                textBox6.Visible = false;

                listBox6.Visible = false;
                listBox3.Visible = false;

                button6.Visible = false;
                button7.Visible = false;
                return;
            }
            d_pos = comboBox3.SelectedIndex - 1;
            if (coll.DictList[d_pos].ListWord.Count == 0)
            {
                label9.Visible = false;
                label16.Visible = true;
                label8.Visible = false;
                label11.Visible = false;

                textBox6.Visible = false;

                listBox6.Visible = false;
                listBox3.Visible = false;

                button6.Visible = false;
                button7.Visible = false;
                return;
            }

            label9.Visible = true;
            label16.Visible = false;
            label8.Visible = true;
            label11.Visible = true;

            textBox6.Visible = true;

            listBox6.Visible = true;
            listBox3.Visible = true;

            button6.Visible = true;
            button7.Visible = true;

            textBox6.Clear();
            listBox6.Items.Clear();
            listBox3.Items.Clear();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                listBox6.Items.Clear();
                listBox3.Items.Clear();
                button7.Enabled = false;
                return;
            }
            var result = coll.DictList[d_pos].ListWord.Where(item => item.Key.Contains(textBox6.Text)).ToArray();
            listBox6.Items.Clear();
            listBox6.Items.AddRange(result.ToArray());
            listBox3.Items.Clear();
            button7.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox6.Clear();
            button7.Enabled = false;
        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox6.SelectedIndex == -1)
            {
                button7.Enabled = false;
                listBox3.Items.Clear();
                return;
            }

            string[] tmpStrArr = listBox6.SelectedItem.ToString().Split(' ');

            w_pos = coll.DictList[d_pos].ListWord.FindIndex(item => item.Key == tmpStrArr[0]);
            listBox3.Items.Clear();
            listBox3.Items.AddRange(coll.DictList[d_pos].ListWord[w_pos].ListValue.ToArray());

            string tmp = textBox5.Text;
            textBox5.Clear();
            textBox5.Text = tmp;
            button7.Enabled = false;
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            button7.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(coll.DictList[d_pos].ListWord[w_pos].ListValue.Count ==1)
            {
                MessageBox.Show($"У слова залишився один переклад. \nВидалити можна тільки слово вцілому!", "Translator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                listBox3.SelectedIndex = -1;
                button7.Enabled = false;
                return;
            }
            t_pos = listBox3.SelectedIndex;
            string tmp = listBox3.SelectedItem.ToString();
            var result = MessageBox.Show($"Видалити переклад \"{tmp}\" ?", "Translator", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                coll.DictList[d_pos].ListWord[w_pos].ListValue.RemoveAt(t_pos);
                XmlSerDeser.Serialize(coll[d_pos], coll[d_pos].Path);
                MessageBox.Show($"Cлово \"{tmp}\" видалено!", "Translator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            listBox3.Items.Clear();
            listBox3.Items.AddRange(coll.DictList[d_pos].ListWord[w_pos].ListValue.ToArray());
            listBox3.SelectedIndex = -1;
            button7.Enabled = false;
        }
    }
}
