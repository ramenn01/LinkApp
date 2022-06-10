using Eframework.Models;
using RecentJobStuff._2nd_form;
using RecentJobStuff.CategoryKey;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


//Why not have the category name in the Categories database? Gets rid of needing to convert a Category to a CategoryId with a database access. 
//REMEMBER .CONTAINS!!!!!
namespace RecentJobStuff
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }


        List<string> categoryList = new List<string>();


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void button2_Click(object sender, EventArgs e)
        {
            bool alreadyExists;
            using (var db = new CategoryContext())
            {
                foreach (object cb in checkedListBox1.Items)
                {

                    if (!checkedListBox1.CheckedItems.Contains(cb))
                    {
                        foreach (CategoryModel item in db.Categories)
                        {
                            if (item.CategoryId == CategoryToId(cb.ToString()) && item.Url == textBox1.Text)
                            {
                                db.Categories.Remove(item);
                            }
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        alreadyExists = false;
                        CategoryModel ctg = new CategoryModel();
                        ctg.CategoryId = CategoryToId(cb.ToString());
                        ctg.Url = textBox1.Text;
                        ctg.UrlId = UrlToId(textBox1.Text);
                        foreach (CategoryModel item in db.Categories)
                        {
                            if (item.CategoryId == ctg.CategoryId && item.UrlId == ctg.UrlId)
                            {
                                alreadyExists = true;
                                break; 
                            }
                        }
                        if (!alreadyExists)
                        {
                            db.Categories.Add(ctg);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }
        public int CategoryToId(string categoryString)
        {

            using (var db = new CategoryKeyContext())
            {
                foreach (CategoryKeyModel item in db.CategoryKeys)
                {
                    if (categoryString == item.CategoryName)
                    {
                        return item.CategoryId;
                    }
                }
                return 0;

            }
        }
        public int UrlToId(string UrlString)
        {
            int newUrlId = 1;

            using (var db = new CategoryContext())
            {

                foreach (CategoryModel item in db.Categories)
                {
                    if (item.UrlId > newUrlId)
                    {
                        newUrlId = item.UrlId++;
                    }
                    else if (item.UrlId == newUrlId)
                    {
                        if (UrlString == item.Url)
                        {
                            return item.UrlId;
                        }
                        else
                        {
                            newUrlId = item.UrlId + 1;
                        }
                    }
                    if (UrlString == item.Url)
                    {
                        return item.UrlId;
                    }
                }


            }

            return newUrlId;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            List<SiteModel> AllSites = new List<SiteModel>();
            using (var db = new DemoContext())
            {
                foreach (SiteModel item in db.Sites)
                {
                    //textBox1.Text = item.Url; 
                    AllSites.Add(item);

                }
                dataGridView1.DataSource = AllSites;



            }
            using (var db = new CategoryKeyContext())
            {
                foreach (CategoryKeyModel item in db.CategoryKeys)
                {
                    categoryList.Add(item.CategoryName);
                }

            }
            foreach (string category in categoryList)
            {
                checkedListBox1.Items.Add(category);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            string rowUrl = dataGridView1[2, rowIndex].Value.ToString();
            string rowTitle = dataGridView1[1, rowIndex].Value.ToString();
            textBox1.Text = rowUrl;
            textBox2.Text = rowTitle;
            checkedListBox1.Items.Clear();
            foreach (string category in categoryList)
            {
                checkedListBox1.Items.Add(category);
            }
            using (var db = new CategoryContext())
            {
                foreach (var item in db.Categories)
                {
                    if (item.Url == rowUrl)
                    {
                        using (var db2 = new CategoryKeyContext())
                        {
                            foreach (var item2 in db2.CategoryKeys)
                            {
                                if (item2.CategoryId == item.CategoryId)
                                {
                                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                                    {
                                        if (item2.CategoryName.Equals(checkedListBox1.Items[i].ToString()))
                                        {
                                            checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("Are you sure you want to add the category -" + textBox3.Text + "- ?", "Confirmation", MessageBoxButtons.YesNo);

            if (dr == DialogResult.Yes)
            {
                checkedListBox1.Items.Add(textBox3.Text);
                categoryList.Add((string)textBox3.Text);
                using (var db = new CategoryKeyContext())
                {

                    CategoryKeyModel CategoryKeyItem = new CategoryKeyModel();

                    CategoryKeyItem.CategoryName = textBox3.Text;
                    CategoryKeyItem.CategoryId = GetCategoryId(textBox3.Text);



                    db.Add(CategoryKeyItem);
                    db.SaveChanges();


                }
            }


        }

        public int GetCategoryId(string CategoryString)
        {
            int newCategoryId = 1;

            using (var db = new CategoryKeyContext())
            {

                foreach (CategoryKeyModel item in db.CategoryKeys)
                {
                    if (item.CategoryId > newCategoryId)
                    {
                        newCategoryId = item.CategoryId++;
                    }
                    else if (item.CategoryId == newCategoryId)
                    {
                        if (CategoryString == item.CategoryName)
                        {
                            return item.CategoryId;
                        }
                        else
                        {
                            newCategoryId = item.CategoryId + 1;
                        }
                    }
                    if (CategoryString == item.CategoryName)
                    {
                        return item.CategoryId;
                    }
                }


            }

            return newCategoryId;

        }

    }
}
