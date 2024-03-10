using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectB
{
    public partial class ManageStudent : UserControl
    {
        public ManageStudent()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var con = Configuration.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("Insert into [dbo].[Student] values (@FirstName, @LastName, @Contact, @Email, @RegistrationNumber, @Status)", con);
            ////cmd.Parameters.AddWithValue("@Id", textBox7.Text);
            //cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
            //cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
            //cmd.Parameters.AddWithValue("@Contact", textBox3.Text);
            //cmd.Parameters.AddWithValue("@Email", textBox4.Text);
            //cmd.Parameters.AddWithValue("@RegistrationNumber", textBox5.Text);
            //cmd.Parameters.AddWithValue("@Status", int.Parse(textBox6.Text));
            //cmd.ExecuteNonQuery();
            //MessageBox.Show("Successfully saved");

            var con = Configuration.getInstance().getConnection();
            if (!string.IsNullOrEmpty(textBox7.Text.Trim()))
            {
                MessageBox.Show("Invalid Id. The Id textbox should be empty for a new entry.");
                return; // Exit the method if validation fails
            }
            // Validate Contact Number Format
            string contactNumber = textBox3.Text.Trim();
            if (!IsValidContactNumber(contactNumber))
            {
                MessageBox.Show("Invalid contact number format. Please enter a valid contact number starting with +(92) and followed by 10 digits.");
                return; // Exit the method if validation fails
            }

            // Validate Status as Integer
            if (!int.TryParse(textBox6.Text, out int status))
            {
                MessageBox.Show("Invalid status format. Please enter a valid integer for the status.");
                return; // Exit the method if validation fails
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[Student] VALUES (@FirstName, @LastName, @Contact, @Email, @RegistrationNumber, @Status)", con);
            cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
            cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
            cmd.Parameters.AddWithValue("@Contact", contactNumber);
            cmd.Parameters.AddWithValue("@Email", textBox4.Text);
            cmd.Parameters.AddWithValue("@RegistrationNumber", textBox5.Text);
            cmd.Parameters.AddWithValue("@Status", status);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully saved");
        }

        // Function to validate the contact number format
        private bool IsValidContactNumber(string contactNumber)
        {
            // Example format: +(92) 3*********
            return Regex.IsMatch(contactNumber, @"^\+\(92\) \d{10}$");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from dbo.Student", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //var con = Configuration.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("UPDATE Student SET FirstName = @FirstName,LastName = @LastName, Contact = @Contact, Email = @Email, RegistrationNumber = @RegistrationNumber, Status = @Status WHERE Id = @Id", con);
            //cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
            //cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
            //cmd.Parameters.AddWithValue("@Contact", textBox3.Text);
            //cmd.Parameters.AddWithValue("@Email", textBox4.Text);
            //cmd.Parameters.AddWithValue("@RegistrationNumber", int.Parse(textBox5.Text));
            //cmd.Parameters.AddWithValue("@Status", int.Parse(textBox6.Text));
            //cmd.Parameters.AddWithValue("@Id", int.Parse(textBox7.Text));
            //cmd.ExecuteNonQuery();
            //MessageBox.Show("Successfully updated");

            var con = Configuration.getInstance().getConnection();

            // Validate ID as Integer
            if (!int.TryParse(textBox7.Text, out int id))
            {
                MessageBox.Show("Invalid ID format. Please enter a valid integer for the ID.");
                return; // Exit the method if validation fails
            }

            // Validate Contact Number Format
            string contactNumber = textBox3.Text.Trim();
            if (!IsValidContactNumber(contactNumber))
            {
                MessageBox.Show("Invalid contact number format. Please enter a valid contact number starting with +(92) and followed by 10 digits.");
                return; // Exit the method if validation fails
            }

            // Validate Status as Integer
            if (!int.TryParse(textBox6.Text, out int status))
            {
                MessageBox.Show("Invalid status format. Please enter a valid integer for the status.");
                return; // Exit the method if validation fails
            }

            SqlCommand cmd = new SqlCommand("UPDATE Student SET FirstName = @FirstName, LastName = @LastName, Contact = @Contact, Email = @Email, RegistrationNumber = @RegistrationNumber, Status = @Status WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
            cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
            cmd.Parameters.AddWithValue("@Contact", contactNumber);
            cmd.Parameters.AddWithValue("@Email", textBox4.Text);
            cmd.Parameters.AddWithValue("@RegistrationNumber", int.Parse(textBox5.Text));
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully updated");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //var con = Configuration.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("DELETE FROM Student WHERE Id = @Id", con);
            //cmd.Parameters.AddWithValue("@Id", int.Parse(textBox7.Text));

            //cmd.ExecuteNonQuery();
            //MessageBox.Show("Successfully deleted");

            var con = Configuration.getInstance().getConnection();
            // Validate ID as Integer
            if (!int.TryParse(textBox7.Text, out int id))
            {
                MessageBox.Show("Invalid ID format. Please enter a valid integer for the ID.");
                return; // Exit the method if validation fails
            }
            // Get the current maximum identity value before deleting
            SqlCommand getMaxIdCmd = new SqlCommand("SELECT MAX(Id) FROM Student", con);
            int maxIdBeforeDelete = Convert.ToInt32(getMaxIdCmd.ExecuteScalar());
            SqlCommand cmd = new SqlCommand("DELETE FROM Student WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", int.Parse(textBox7.Text));
            cmd.ExecuteNonQuery();
            // Get the new maximum identity value after deleting
            int maxIdAfterDelete = maxIdBeforeDelete - 1;
            // Reset the identity column to the new maximum value
            SqlCommand resetIdentityCmd = new SqlCommand($"DBCC CHECKIDENT ('Student', RESEED, {maxIdAfterDelete})", con);
            resetIdentityCmd.ExecuteNonQuery();
            MessageBox.Show("Successfully deleted and reset identity");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select FirstName, LastName, Contact, Email, RegistrationNumber, Status FROM Student WHERE Id = @Id", con);
            // Validate ID as Integer
            if (!int.TryParse(textBox7.Text, out int id))
            {
                MessageBox.Show("Invalid ID format. Please enter a valid integer for the ID.");
                return; // Exit the method if validation fails
            }
            cmd.Parameters.AddWithValue("@Id", int.Parse(textBox7.Text));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
