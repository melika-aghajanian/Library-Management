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
    public partial class SearchResult : Form
    {
        public string writerName, bookName, memberID;

        public SearchResult(string bname, string wname, string memberid)
        {
            InitializeComponent();
            bookName = bname;
            writerName = wname;
            memberID = memberid;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection("Data Source = LENOVOLAP; Initial Catalog = Project_Database; Integrated Security = SSPI"))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Request (Book_ID, Member_ID) VALUES (@BookID, @MemberID)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BookID", Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value));
                        command.Parameters.AddWithValue("@MemberID", Convert.ToInt32(memberID));
                        MessageBox.Show("درخواست با موفقیت ایجاد شد.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void SearchResult_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection("Data Source = LENOVOLAP; Initial Catalog = Project_Database; Integrated Security = SSPI"))
            {
                connection.Open();

                string query = "SELECT Book.ID, B_Name, Publisher_Name, B_Status, Publish_Date, Writer.W_Name , Translator.T_Name AS Translator_Name FROM Book JOIN Writer ON Book.Writer_ID = Writer.ID LEFT JOIN Translator ON Book.Translator_ID = Translator.ID WHERE B_Name = @BookName AND Writer.W_Name = @WriterName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookName", bookName);
                    command.Parameters.AddWithValue("@WriterName", writerName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader[0].ToString();
                            string name = reader[1].ToString();
                            string writer = reader[5].ToString();
                            string translator = reader[6].ToString();
                            string publisher = reader[2].ToString();
                            string status = reader[3].ToString();
                            string publishdate = reader[4].ToString();

                            dataGridView1.Rows.Add(id, name, writer, translator, publisher, publishdate, status);
                        }
                    }
                }
            }
        }
       
    }
}
