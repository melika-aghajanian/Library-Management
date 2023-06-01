using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PaigahP3
{
    public partial class MemberForm : Form
    {
        private string memberid;
        public MemberForm(string InputID)
        {
            InitializeComponent();
            memberid = InputID;
        }

        private void MemberForm_Load(object sender, EventArgs e)
        {
            DataTable borrowDataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection("Data Source=LENOVOLAP; Initial Catalog=Project_Database; Integrated Security = SSPI"))
            {
                connection.Open();

                string query = "SELECT M_Name, M_Phone, M_Address, Membership_Number, Registery_Date, Reserved_Book FROM Member Where ID="+memberid+"";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            textBox3.Text = reader[0].ToString();
                            textBox4.Text = reader[1].ToString();
                            textBox7.Text = reader[2].ToString();
                            textBox5.Text = reader[3].ToString();
                            textBox6.Text = reader[4].ToString();
                        }
                    }
                }
                query = "SELECT M_Image FROM Member Where ID=" + memberid + "";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        Byte[] data = new Byte[0];
                        data = (Byte[])(ds.Tables[0].Rows[0]["M_Image"]);
                        MemoryStream mem = new MemoryStream(data);
                        pictureBox1.Image = Image.FromStream(mem);
                    }
                }
                query = "SELECT b.ID AS BookID, b.B_Name AS BookName, br.Forfeit FROM Borrow br INNER JOIN Book b ON br.Book_ID = b.ID Where br.Member_ID = "+ memberid + "";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(borrowDataTable);
                }

                dataGridView1.DataSource = borrowDataTable;
                dataGridView1.Columns[0].HeaderText = "سریال کتاب";
                dataGridView1.Columns[1].HeaderText = "نام کتاب";
                dataGridView1.Columns[2].HeaderText = "جریمه";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string bookName = textBox2.Text;
            string writerName = textBox1.Text;

            SearchResult resultForm = new SearchResult(bookName, writerName, memberid);
            resultForm.Show();
        }
    }
}