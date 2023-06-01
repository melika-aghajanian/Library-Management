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
    public partial class LibrarianForm : Form
    {
        private string librarianid;
        public DataTable memberDataTable = new DataTable();
        public DataTable requestDataTable = new DataTable();

        public LibrarianForm(string InputID)
        {
            InitializeComponent();
            librarianid = InputID;
        }

        private void LibrarianForm_Load(object sender, EventArgs e)
        {
            textBox6.Text = DateTime.Now.ToString();
            using (SqlConnection connection = new SqlConnection("Data Source = LENOVOLAP; Initial Catalog = Project_Database; Integrated Security = SSPI"))
            {
                connection.Open();

                string query = "SELECT Personnal_ID, L_Name, L_Phone, Head_ID FROM Librarian Where ID = " + librarianid + "";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            textBox9.Text = reader[0].ToString();
                            textBox8.Text = reader[1].ToString();
                            textBox10.Text = reader[2].ToString();
                            if (reader[3] == reader[0])
                                textBox7.Text = "مدیر";
                            else textBox7.Text = "مسئول";
                            //string librarianInfo = $"Name: {name}\n Phone: {phone}\n Personnal ID: {PerID}\n Head: {head}";
                            //listBox1.Items.Add(librarianInfo);
                        }
                    }
                }
                query = "SELECT ID, M_Name, M_Address, M_Phone, Membership_Number FROM Member";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(memberDataTable);
                }
                dataGridView1.DataSource = memberDataTable;
                dataGridView1.Columns[0].HeaderText = "سریال";
                dataGridView1.Columns[1].HeaderText = "نام ";
                dataGridView1.Columns[2].HeaderText = "آدرس";
                dataGridView1.Columns[3].HeaderText = "تلفن";
                dataGridView1.Columns[4].HeaderText = "کد عضویت";

                query = "SELECT Request.ID, Request.Member_ID, Request.Book_ID, Book.B_Status FROM Request INNER JOIN Book ON Request.Book_ID = Book.ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(requestDataTable);
                }

                dataGridView2.DataSource = requestDataTable;
                dataGridView2.Columns[0].HeaderText = "سریال";
                dataGridView2.Columns[1].HeaderText = "کد عضویت";
                dataGridView2.Columns[2].HeaderText = "سریال کتاب";
                dataGridView2.Columns[3].HeaderText = "وضعیت";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                int memberId = Convert.ToInt32(dataGridView1.Rows[selectedIndex].Cells[0].Value);

                if (MessageBox.Show("آیا از حذف کاربر مورد نظر اطمینان دارید؟", "حذف کاربر", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeleteMember(memberId);
                    memberDataTable.Rows.RemoveAt(selectedIndex);
                }
            }
            else
            {
                MessageBox.Show("لطفا حداقل یک کاربر را برای حذف انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteMember(int memberId)
        {
            using (SqlConnection connection = new SqlConnection("Data Source = LENOVOLAP; Initial Catalog = Project_Database; Integrated Security = SSPI"))
            {
                connection.Open();

                string query = "DELETE FROM Member WHERE ID = @MemberId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MemberId", memberId);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void DeleteRequest(int requestId)
        {
            using (SqlConnection connection = new SqlConnection("Data Source = LENOVOLAP; Initial Catalog = Project_Database; Integrated Security = SSPI"))
            {
                connection.Open();

                string query = "DELETE FROM Request WHERE ID = @RequestId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RequestId", requestId);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void ConfirmRequest(int requestId, int memberId, int bookId, int librarianId)
        {
            using (SqlConnection connection = new SqlConnection("Data Source = LENOVOLAP; Initial Catalog = Project_Database; Integrated Security = SSPI"))
            {
                connection.Open();

                // Retrieve member information
                string memberQuery = "SELECT M_Name, M_Phone, M_Address FROM Member WHERE ID = @MemberId";
                string borrowQuery = "INSERT INTO Borrow (Librarian_ID, Member_ID, Book_ID, Borrow_Date) VALUES (@LibrarianId, @MemberId, @BookId, @BorrowDate)";
                using (SqlCommand borrowCommand = new SqlCommand(borrowQuery, connection))
                {
                    borrowCommand.Parameters.AddWithValue("@LibrarianId", librarianId);
                    borrowCommand.Parameters.AddWithValue("@MemberId", memberId);
                    borrowCommand.Parameters.AddWithValue("@BookId", bookId);
                    borrowCommand.Parameters.AddWithValue("@BorrowDate", DateTime.Now);

                    borrowCommand.ExecuteNonQuery();
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView2.SelectedRows[0].Index;
                int requestId = Convert.ToInt32(dataGridView2.Rows[selectedIndex].Cells[0].Value);

                if (MessageBox.Show("آیا از رد درخواست انتخابی اطمینان دارید؟", "رد درخواست", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeleteRequest(requestId);
                    requestDataTable.Rows.RemoveAt(selectedIndex);
                }
            }
            else
            {
                MessageBox.Show("لطفا یک درخواست جهت رد کردن آن انتخاب کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView2.SelectedRows[0].Index;
                int requestId = Convert.ToInt32(dataGridView2.Rows[selectedIndex].Cells[0].Value);
                int memberId = Convert.ToInt32(dataGridView2.Rows[selectedIndex].Cells[1].Value);
                int bookId = Convert.ToInt32(dataGridView2.Rows[selectedIndex].Cells[2].Value);
                int librarianId = Convert.ToInt32(librarianid);
                if (MessageBox.Show("آیا از تایید درخواست انتخابی اطمینان دارید؟", "تایید درخواست", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ConfirmRequest(requestId, memberId, bookId, librarianId);
                    DeleteRequest(requestId);
                    requestDataTable.Rows.RemoveAt(selectedIndex);
                }
                else
                {
                    MessageBox.Show("لطفا درخواستی را جهت تایید آن انتخاب کنید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection("Data Source = LENOVOLAP; Initial Catalog = Project_Database; Integrated Security = SSPI"))
            {
                connection.Open();
                string borrowQuery = "INSERT INTO Member (ID, M_Name, M_Phone, M_Address, Membership_Number, Registery_Date) VALUES (@memberID, @memberName, @memberPhone, @memberAdd, @memberNumber, @registerDate)";
                using (SqlCommand borrowCommand = new SqlCommand(borrowQuery, connection))
                {
                    borrowCommand.Parameters.AddWithValue("@memberID", Convert.ToInt32(textBox1.Text));
                    borrowCommand.Parameters.AddWithValue("@memberName", textBox2.Text.ToString());
                    borrowCommand.Parameters.AddWithValue("@memberPhone", textBox3.Text.ToString());
                    borrowCommand.Parameters.AddWithValue("@memberAdd", textBox4.Text.ToString());
                    borrowCommand.Parameters.AddWithValue("@memberNumber", textBox5.Text.ToString());
                    borrowCommand.Parameters.AddWithValue("@registerDate", DateTime.Parse(textBox6.Text));
                    borrowCommand.ExecuteNonQuery();
                }
                MessageBox.Show("کاربر با موفقیت افزوده شد.");
                memberDataTable.Rows.Add(Convert.ToInt32(textBox1.Text), textBox2.Text.ToString(), textBox4.Text.ToString(), textBox3.Text.ToString(), textBox5.Text.ToString());
            }
            if (pictureBox2.Image == null)
            {
                MessageBox.Show("Please select an image first.");
                return;
            }

            byte[] imageData = ImageToByteArray(pictureBox2.Image);
            SaveImageToDatabase(imageData);
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, image.RawFormat);
                return memoryStream.ToArray();
            }
        }

        private void SaveImageToDatabase(byte[] imageData)
        {
            using (SqlConnection connection = new SqlConnection("Data Source = LENOVOLAP; Initial Catalog = Project_Database; Integrated Security = SSPI"))
            {
                connection.Open();

                try
                {
                    string query = "UPDATE Member SET M_Image = @ImageData Where ID = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ImageData", imageData);
                        command.Parameters.AddWithValue("@id", Convert.ToInt32(textBox1.Text));
                        command.ExecuteNonQuery();
                        MessageBox.Show("تصویر با موفقیت ذخیره شد");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطا: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.png, *.gif)|*.jpg;*.png;*.gif";
            openFileDialog.Title = "Select an Image File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string imagePath = openFileDialog.FileName;
                pictureBox2.Image = Image.FromFile(imagePath);
            }
        }
    }
}
