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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string InputID = textBox1.Text.Trim();
            string InputPass = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(InputID))
            {
                MessageBox.Show("لطفا نام کاربری را وارد کنید");
                return;
            }

            bool isMemberId = CheckMemberId(InputID,InputPass);
            bool isLibrarianId = CheckLibrarianId(InputID, InputPass);

            if (isMemberId)
            {
                var memberForm = new MemberForm(InputID);
                memberForm.ShowDialog();
            }
            else if (isLibrarianId)
            {
                var libForm = new LibrarianForm(InputID);
                libForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("نام کاربری یا رمز عبور نادرست است.");
            }
        }

        private bool CheckMemberId(string inputid, string inputpass)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=LENOVOLAP; Initial Catalog=Project_Database; Integrated Security = SSPI"))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Member WHERE ID=@Input1 and Membership_Number=@Input2";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Input1", inputid);
                    command.Parameters.AddWithValue("@Input2", inputpass);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private bool CheckLibrarianId(string inputid, string inputpass)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=LENOVOLAP; Initial Catalog=Project_Database; Integrated Security = SSPI"))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Librarian WHERE ID=@Input1 and Personnal_ID=@Input2";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Input1", inputid);
                    command.Parameters.AddWithValue("@Input2", inputpass);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
