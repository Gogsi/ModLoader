using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO.Compression;
using System.IO;


namespace GogsiModLoader
{
    public partial class DownloadForm : Form
    {
        public DownloadForm()
        {
            InitializeComponent();
            LoadOnlineMods();
        }

        private void LoadOnlineMods()
        {
            using (var client = new WebClient())
            {
                client.DownloadFile("https://raw.githubusercontent.com/Gogsi/ModLoader/master/ModDB/mods.info", "./ModLoader/onlinemods.info");

            }
            using (StreamReader sr = new StreamReader("./ModLoader/onlinemods.info"))
            {
                string line;

                string modName;
                string fileName;
                string modType;
                string modPure;
                string modLink;

                while ((line = sr.ReadLine()) != null)
                {
                    if (!line.StartsWith("//") && line.Length > 0)
                    {
                        //We have a mod
                        modName = line;
                        fileName = sr.ReadLine();
                        modType = sr.ReadLine();
                        modPure = sr.ReadLine();
                        modLink = sr.ReadLine();

                        ListViewItem item1 = new ListViewItem(modName);
                        item1.SubItems.Add(fileName);
                        item1.SubItems.Add(modType);
                        item1.SubItems.Add(modPure);
                        item1.SubItems.Add(modLink);

                        listView1.Items.Add(item1);
                    }
                }

            }
            label2.Visible = false;

        }
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                string downloadLink = item.SubItems[4].Text;
                string fileName = item.SubItems[1].Text;
                string modType = item.SubItems[2].Text;
                string modPure = item.SubItems[3].Text;

                string modName = item.SubItems[0].Text;
                using (var client = new WebClient())
                {
                    client.DownloadFile(downloadLink, "./ModLoader/" + fileName + ".zip");
                }
                ZipFile.ExtractToDirectory("./ModLoader/" + fileName + ".zip", "./ModLoader");

                File.Delete("./ModLoader/" + fileName + ".zip");

                using (StreamWriter sw = new StreamWriter("./ModLoader/mods.info", true))
                {
                    sw.WriteLine(modName);
                    sw.WriteLine(modType);
                    sw.WriteLine(modPure);
                    sw.WriteLine("/ModLoader/" + fileName);
                    sw.WriteLine("No");
                    sw.WriteLine();

                    sw.Close();
                }
                MessageBox.Show("Mod successfully downloaded.");
            }
        }
    }
}
