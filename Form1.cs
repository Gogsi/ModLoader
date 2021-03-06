﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace GogsiModLoader
{
    public partial class Form1 : Form
    {

       // private StreamReader sr = new StreamReader("./ModLoader/mods.info");
       // private StreamWriter sw = new StreamWriter("./ModLoader/mods.info");

        private bool LoadingComplete = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("./ModLoader"))
            {

            }
            LoadMods();
        }

        private void LoadMods()
        {
            LoadingComplete = false;
            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();

            System.Threading.Thread.Sleep(500);

            List<ListViewItem> scriptItems = new List<ListViewItem>();
            List<ListViewItem> interfaceItems = new List<ListViewItem>();
            List<ListViewItem> fontItems = new List<ListViewItem>();

            using (StreamReader sr = new StreamReader("./ModLoader/mods.info"))
            {
                string line;

                string modName;
                string modType;
                string modPure;
                string modPath;
                string modEnabled;

                

                while ((line = sr.ReadLine()) != null)
                {
                    if (!line.StartsWith("//") && line.Length > 0)
                    {
                        //We have a mod
                        modName = line;
                        modType = sr.ReadLine();
                        modPure = sr.ReadLine();
                        modPath = sr.ReadLine();
                        modEnabled = sr.ReadLine();

                        ListViewItem item1 = new ListViewItem(modName);
                        item1.SubItems.Add(modPure);
                        item1.SubItems.Add(modPath);
                        item1.Tag = "first";

                        if (modEnabled == "Yes")
                        { item1.Checked = true; }
                        if (item1 != null)
                        {
                            if (modType == "script")
                                scriptItems.Add(item1);
                            if (modType == "interface")
                                interfaceItems.Add(item1);
                            if (modType == "font")
                                fontItems.Add(item1);
                        }

                       
                    }

                }               
                sr.Close();


          }
            listView1.Items.AddRange(scriptItems.ToArray());
            listView2.Items.AddRange(interfaceItems.ToArray());
            listView3.Items.AddRange(fontItems.ToArray());

            tabControl1.TabIndex = 1;
            System.Threading.Thread.Sleep(5);
            tabControl1.TabIndex = 2;
            System.Threading.Thread.Sleep(5);
            tabControl1.TabIndex = 0;


            LoadingComplete = true;
        }

        private void SaveMods()
        {
            using (StreamWriter sw = new StreamWriter("./ModLoader/mods.info"))
            {
               
                foreach (ListViewItem item in listView1.Items)
                {
      //              if (item == null) return;

                    sw.WriteLine(item.Text);
                    sw.WriteLine("script");
                    sw.WriteLine(item.SubItems[1].Text);
                    sw.WriteLine(item.SubItems[2].Text);
                    if (item.Checked)
                        sw.WriteLine("Yes");
                    else
                        sw.WriteLine("No");

                    sw.WriteLine();

                }
               
                foreach (ListViewItem item in listView2.Items)
                {
                    sw.WriteLine(item.Text);
                    sw.WriteLine("interface");
                    sw.WriteLine(item.SubItems[1].Text);
                    sw.WriteLine(item.SubItems[2].Text);
                    if (item.Checked)
                        sw.WriteLine("Yes");
                    else
                        sw.WriteLine("No");

                    sw.WriteLine();

                }

                foreach (ListViewItem item in listView3.Items)
                {
                    sw.WriteLine(item.Text);
                    sw.WriteLine("font");
                    sw.WriteLine(item.SubItems[1].Text);
                    sw.WriteLine(item.SubItems[2].Text);
           
                    
                    
                    
                    
                    if (item.Checked)
                        sw.WriteLine("Yes");
                    else
                        sw.WriteLine("No");

                    sw.WriteLine();

                }
                sw.Close();
            }

        }
     
        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            
            ListViewItem item = e.Item as ListViewItem;
            if (item.Tag == "first") { item.Tag = "not"; return; }

            string sourcePath = @"." + item.SubItems[2].Text;
            string targetPath = @"./csgo";

            if (!item.Checked)
            {
                UnloadMod(sourcePath, "");
                MessageBox.Show("Unloaded " + item.Text + ".");

            }
            else
            {
                LoadMod(sourcePath);            
                MessageBox.Show("Loaded " + item.Text + ".");
            }

            if (LoadingComplete) SaveMods();

        }

        private void listView2_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ListViewItem item = e.Item as ListViewItem;
            if (item.Tag == "first") { item.Tag = "not"; return; }

            string sourcePath = @"." + item.SubItems[2].Text;
            string targetPath = @"./csgo";

            if (!item.Checked)
            {
                UnloadMod(sourcePath, "");
                MessageBox.Show("Unloaded " + item.Text + ".");

            }
            else
            {
                LoadMod(sourcePath);
                MessageBox.Show("Loaded " + item.Text + ".");
            }

            if (LoadingComplete) SaveMods();
        }

        private void listView3_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ListViewItem item = e.Item as ListViewItem;
            if (item.Tag == "first") { item.Tag = "not"; return; }

            string sourcePath = @"." + item.SubItems[2].Text;
            string targetPath = @"./csgo";

            if (!item.Checked)
            {
                UnloadMod(sourcePath, "");
                MessageBox.Show("Unloaded " + item.Text + ".");

            }
            else
            {
                LoadMod(sourcePath);            
                MessageBox.Show("Loaded " + item.Text + ".");
            }

            if (LoadingComplete) SaveMods();
        }

        public static void LoadMod(string modFolder)
        {
            string targetPath = @"./csgo";

            DirectoryInfo di1 = new DirectoryInfo(modFolder);
            DirectoryInfo di2 = new DirectoryInfo(targetPath);

            CopyFolder(di1, di2);
        }

        public static void UnloadMod(string modFolder, string additional)
        {
            string defaultsFolder = "ModLoader/Defaults";
            string actionFoler = "csgo";
            DirectoryInfo current = new DirectoryInfo(modFolder +  additional);
            DirectoryInfo original = new DirectoryInfo(defaultsFolder + additional);

            foreach (DirectoryInfo dir in current.GetDirectories())
            {
                UnloadMod(modFolder, additional + "/" + dir.Name);
            }
            foreach (FileInfo file in current.GetFiles())
            {
                FileInfo originalFile = new FileInfo(defaultsFolder + additional + "/" + file.Name);

                if (originalFile.Exists)
                {
                    originalFile.CopyTo(actionFoler +  additional + "/" + file.Name, true);

                }else{
                    new FileInfo(@actionFoler +  additional + "/" + file.Name).Delete();
                }
            }
        }

        public static void CopyFolder(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFolder(dir, target.CreateSubdirectory(dir.Name));

            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name),true);
        }

       


        private void button1_Click(object sender, EventArgs e)
        {
             FolderBrowserDialog fbd = new FolderBrowserDialog();
             if (fbd.ShowDialog() == DialogResult.OK)
             {
                 string sourcePath = @fbd.SelectedPath;
                 string targetPath = @"./ModLoader";

                 DirectoryInfo di1 = new DirectoryInfo(sourcePath);

                 string newFolder = "/ModLoader/" + Path.GetFileName(fbd.SelectedPath);

                 System.IO.Directory.CreateDirectory("." + newFolder);
                 DirectoryInfo di2 = new DirectoryInfo("." + newFolder);

                 CopyFolder(di1, di2);

                 string input = Microsoft.VisualBasic.Interaction.InputBox("Choose Name", "Choose a name for this mod", "CSGOMOD", 0, 0);

                 ListViewItem item1 = new ListViewItem(input);
                 item1.SubItems.Add("???");
                 item1.SubItems.Add(newFolder);

                listView1.Items.Add(item1);
                SaveMods();

             }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string sourcePath = @fbd.SelectedPath;
                string targetPath = @"./ModLoader";

                DirectoryInfo di1 = new DirectoryInfo(sourcePath);

                string newFolder = "/ModLoader/" + Path.GetFileName(fbd.SelectedPath);

                System.IO.Directory.CreateDirectory("." + newFolder);
                DirectoryInfo di2 = new DirectoryInfo("." + newFolder);

                CopyFolder(di1, di2);

                string input = Microsoft.VisualBasic.Interaction.InputBox("Choose Name", "Choose a name for this mod", "CSGOMOD", 0, 0);

                ListViewItem item1 = new ListViewItem(input);
                item1.SubItems.Add("Yes");
                item1.SubItems.Add(newFolder);

                listView2.Items.Add(item1);
                SaveMods();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string sourcePath = @fbd.SelectedPath;
                string targetPath = @"./ModLoader";

                DirectoryInfo di1 = new DirectoryInfo(sourcePath);

                string newFolder = "/ModLoader/" + Path.GetFileName(fbd.SelectedPath);

                System.IO.Directory.CreateDirectory("." + newFolder);
                DirectoryInfo di2 = new DirectoryInfo("." + newFolder);

                CopyFolder(di1, di2);

                string input = Microsoft.VisualBasic.Interaction.InputBox("Choose Name", "Choose a name for this mod", "CSGOMOD", 0, 0);

                ListViewItem item1 = new ListViewItem(input);
                item1.SubItems.Add("Yes");
                item1.SubItems.Add(newFolder);

                listView3.Items.Add(item1);
                SaveMods();

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("steam://rungameid/730");
         
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("steam://validate/730");

        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Script modifications alter gameplay elements and often don't work on official servers.", "About script modifications");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Interface modifications change the look of the game and work on official servers.", "About interface modifications");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Custom fonts make the game more appealing and work on official servers.", "About custom fonts");

        }

        private void button6_Click(object sender, EventArgs e)
        {
            new HelpForm().ShowDialog();

        }

        private void button10_Click(object sender, EventArgs e)
        {
            new DownloadForm().ShowDialog();
            File.Delete("./ModLoader/onlinemods.info");

            LoadMods();

        }

  
      
    }
}
