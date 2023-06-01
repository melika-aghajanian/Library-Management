using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PaigahP3
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var authForm = new LoginForm();
            authForm.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            DataTable bookDataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection("Data Source = LENOVOLAP; Initial Catalog = Project_Database; Integrated Security = SSPI"))
            {
                connection.Open();

                string query = "SELECT b.ID AS BookID, b.B_Name AS BookName, b.Publisher_Name, b.Publish_Date, b.B_Status, s.S_Name AS SectionName, w.W_Name AS WriterName, t.T_Name AS TranslatorName FROM Book b INNER JOIN Section s ON b.Section_ID = s.ID INNER JOIN Writer w ON b.Writer_ID = w.ID INNER JOIN Translator t ON b.Translator_ID = t.ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(bookDataTable);
                }
            }

            dataGridView1.DataSource = bookDataTable;
            dataGridView1.Columns[0].HeaderText = "سریال";
            dataGridView1.Columns[1].HeaderText = "نام کتاب";
            dataGridView1.Columns[2].HeaderText = "انتشارات";
            dataGridView1.Columns[3].HeaderText = "تاریخ انتشار";
            dataGridView1.Columns[4].HeaderText = "وضعیت";
            dataGridView1.Columns[5].HeaderText = "نام بخش";
            dataGridView1.Columns[6].HeaderText = "نام نویسنده";
            dataGridView1.Columns[7].HeaderText = "نام مترجم";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("برای جستجو لازم است ابتدا به عنوان عضو کتابخانه وارد شوید.");
        }
    }
}
