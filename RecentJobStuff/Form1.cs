using Eframework.Models;
using Microsoft.Exchange.WebServices.Data;
using RecentJobStuff._2nd_form;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace RecentJobStuff
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public List<SitePair> sitePairs = new List<SitePair>();
        public List<string> urlList = new List<string>();
        private void Form1_Load(object sender, EventArgs e)
        {

            List<string> AllSites = new List<string>();
            using (var db = new DemoContext())
            {
                foreach (SiteModel item in db.Sites)
                {
                    AllSites.Add(item.Title);
                    sitePairs.Add(new SitePair { Url = item.Url, Title = item.Title });
                }
                foreach (string title in AllSites)
                {
                    treeView2.Nodes.Add(String.Format(title));
                }



            }
            textBox4.Text = "trevor.magruder@albionnet.com";
            textBox5.Text = "B0bth3Guy";
            textBox3.Text = "RealTestFolder";
        }

        public int mycount2 = 0;
        public void useUrl(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse response;

            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }
            StreamReader sr = new StreamReader(response.GetResponseStream());
            while (1 == 1)
            {
                string text = sr.ReadLine();
                if (text == null)
                {
                    break;
                }
                if (text.Contains("<title"))
                {
                    string regex = @"(?<=<title.*>)([\s\S]*)(?=</title>)";
                    System.Text.RegularExpressions.Regex ex = new System.Text.RegularExpressions.Regex(regex, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    string title = ex.Match(text).Value.Trim();
                    if (title == "")
                    {
                        title = sr.ReadLine();
                    }
                    AddNodes(url, title);
                    mycount2++;
                    urlList.Add(url);
                    sitePairs.Add(new SitePair { Url = url, Title = System.Web.HttpUtility.HtmlDecode(title) });
                    break;

                }

            }
            sr.Close();
        }

        public void AddNodes(string url, string title)
        {
            TreeNode node;
            node = treeView1.Nodes.Add(System.Web.HttpUtility.HtmlDecode(string.Format(title)));
            node.Nodes.Add(url);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            string nodeText = e.Node.Text;

            foreach (string match in urlList)
            {
                if (match == nodeText)
                {
                    Process.Start(new ProcessStartInfo(nodeText) { UseShellExecute = true });
                    break;

                }
            }
            foreach (SitePair sitePair in sitePairs)
            {
                if (sitePair.Title.Equals(nodeText))
                {
                    TitleTxt.Text = sitePair.Title;
                    UrlTxt.Text = sitePair.Url;
                    break;
                }
            }


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TXT Files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = @"C:\\Work\\sciSrc\\alldata";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != null && openFileDialog.FileName != "")
            {
                fileName = openFileDialog.FileName;
                textBox1.Text = openFileDialog.FileName;
            }

            if (fileName != null)
            {
                button1.Enabled = true;
                urlList.Clear();
                displayFiles();
            }


        }
        public string fileName;
        public void displayFiles()
        {
            treeView1.Nodes.Clear();

            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    useUrl(line);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();

            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    useUrl(line);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //saveSite(TitleTxt.Text, UrlTxt.Text); 
            foreach (TreeNode item in treeView1.Nodes)
            {
                if (item.Checked == true)
                {
                    foreach (SitePair sitePair in sitePairs)
                    {
                        if (sitePair.Title.Equals(item.Text))
                        {
                            saveSite(sitePair.Title, sitePair.Url);
                            break;
                        }
                    }
                }
            }

        }

        private void saveSite(string title, string url)
        {
            bool duplicate = false;
            using (var db = new DemoContext())
            {

                SiteModel site = new SiteModel();

                site.Title = title;
                site.Url = url;
                foreach (var item in db.Sites)
                {
                    if (item.Url == url)
                    {
                        duplicate = true;
                    }

                }
                if (duplicate == true)
                {
                    //MessageBox.Show("This site has already been added to the database.");
                }
                else
                {
                    db.Add(site);
                    db.SaveChanges();
                    TreeNode node;

                    node = treeView2.Nodes.Add(String.Format(site.Title));

                }

            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {


        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
            newForm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (TreeNode item in treeView1.Nodes)
            {
                foreach (SitePair sitePair in sitePairs)
                {
                    if (sitePair.Title.Equals(item.Text))
                    {
                        saveSite(sitePair.Title, sitePair.Url);
                        break;
                    }
                }

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            List<TreeNode> nodesToRemove = new List<TreeNode>();
            List<string> list2 = new List<string>();
            foreach (TreeNode item in treeView2.Nodes)
            {
                if (item.Checked == true)
                {
                    list.Add(item.Text);
                    nodesToRemove.Add(item);

                    foreach (SitePair sitePair in sitePairs)
                    {
                        if (sitePair.Title == item.Text)
                        {
                            list2.Add(sitePair.Url);
                        }
                    }
                }

            }

            using (var db = new DemoContext())
            {
                foreach (SiteModel item in db.Sites)
                {
                    if (list.Contains(item.Title))
                    {
                        db.Sites.Remove(item);

                    }
                }
                db.SaveChanges();
            }
            foreach (TreeNode item in nodesToRemove)
            {
                item.Remove();
            }






            using (var db = new CategoryContext())
            {
                foreach (CategoryModel categoryModel in db.Categories)
                {
                    foreach (string pair in list2)
                    {
                        if (pair == categoryModel.Url)
                        {
                            db.Categories.Remove(categoryModel);
                        }
                    }
                }
                db.SaveChanges();
            }
        }







        private void button6_Click(object sender, EventArgs e)
        {
            List<Item> items = new List<Item>();
            Folder myFolder;
            ExchangeService _service = new ExchangeService
            {
                Credentials = new WebCredentials(textBox4.Text, textBox5.Text)
            };


            _service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");

            Folder msgF = Folder.Bind(_service, WellKnownFolderName.MsgFolderRoot);
            FolderView folderView = new FolderView(int.MaxValue);
            folderView.OffsetBasePoint = OffsetBasePoint.Beginning;
            folderView.PropertySet = new PropertySet(FolderSchema.DisplayName, FolderSchema.Id);

            FindFoldersResults folderResults = _service.FindFolders(msgF.Id, folderView);
            foreach (Folder folder in folderResults)
            {
                //if (string.Compare(folder.DisplayName, textBox3.Text, StringComparison.OrdinalIgnoreCase) == 0)
                if (folder.DisplayName == textBox3.Text)
                {
                    textBox2.Text = folder.DisplayName;
                    myFolder = folder;
                    ItemView itemView = new ItemView(int.MaxValue, 0);
                    foreach (Item item in myFolder.FindItems(itemView))
                    {
                        items.Add(item);
                        item.Load();
                    }
                    PropertySet _customPropertySet = new PropertySet(BasePropertySet.FirstClassProperties);
                    _customPropertySet.RequestedBodyType = BodyType.Text;
                    _customPropertySet.Add(ItemSchema.TextBody);



                    _service.LoadPropertiesForItems(items, _customPropertySet);

                    foreach (Item item in items)
                    {
                        StringReader sr = new StringReader(item.TextBody.ToString());
                        string finalAnswer = sr.ReadLine();
                        while (finalAnswer != null && finalAnswer != "")
                        {
                            if (finalAnswer.Length > 5)
                            {
                                if (finalAnswer[..4] == "http")
                                {
                                    textBox2.Text += finalAnswer;
                                    useUrl(finalAnswer);
                                }
                            }
                            finalAnswer = sr.ReadLine();
                        }




                    }




                }
            }



        }

        private void button7_Click(object sender, EventArgs e)
        {
            List<TreeNode> toDelete = new List<TreeNode>();
            foreach (TreeNode item in treeView1.Nodes)
            {
                if (item.Checked == true)
                {
                    toDelete.Add(item);
                }
            }
            foreach (TreeNode item in toDelete)
            {
                item.Remove();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.wsj.com");
            HttpWebResponse response;
            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string text = sr.ReadToEnd();
            string text2;
            string firstUrl;
            bool newfirst = true;
            text2 = text.Split("https://www.wsj.com/articles/")[1];
            firstUrl = "https://www.wsj.com/articles/" + text2.Split("\"")[0];
            foreach (string s in text.Split(firstUrl + "\",\"wordCount"))
            {
                if (newfirst == true)
                {
                    newfirst = false;
                    text = "";
                }
                else
                {
                    text = text + s;
                }
            }
            string[] uncutUrls = text.Split("url\":\"https://www.wsj.com/articles/");
            List<string> urls = new List<string>();
            urls.Add(firstUrl);
            bool first = true;

            foreach (string v in uncutUrls)
            {
                if (first == true)
                {
                    first = false;
                }
                else
                {
                    string stringToAdd = v.Split('\"')[0];

                    if (urls.Contains("https://www.wsj.com/articles/" + stringToAdd) == false)
                    {
                        urls.Add("https://www.wsj.com/articles/" + stringToAdd);
                    }
                }
            }
            sr.Close();
            foreach (string url in urls)
            {
                useUrl(url);

            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.wsj.com/news/opinion");
            HttpWebResponse response;
            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string text = sr.ReadToEnd();

            string[] uncutUrls = text.Split("url\":\"https://www.wsj.com/articles/");
            List<string> urls = new List<string>();
            bool first = true;

            foreach (string v in uncutUrls)
            {
                if (first == true)
                {
                    first = false;
                }
                else
                {
                    string stringToAdd = v.Split('\"')[0];

                    if (urls.Contains("https://www.wsj.com/articles/" + stringToAdd) == false)
                    {
                        urls.Add("https://www.wsj.com/articles/" + stringToAdd);
                    }
                }
            }
            sr.Close();
            
            foreach (string url in urls)
            {
                useUrl(url);

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear(); 
        }
    }

}

