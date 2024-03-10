using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProjectB
{
    public partial class Assessments : UserControl
    {
        public Assessments()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var con = Configuration.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("Insert into [dbo].[Assessment] values (@Title, @DateCreated, @TotalMarks, @TotalWeightage)", con);
            ////cmd.Parameters.AddWithValue("@Id", textBox1.Text);
            //cmd.Parameters.AddWithValue("@Title", textBox2.Text);
            //cmd.Parameters.AddWithValue("@DateCreated", dateTimePicker1.Value);
            //cmd.Parameters.AddWithValue("@TotalMarks", int.Parse(textBox4.Text));
            //cmd.Parameters.AddWithValue("@TotalWeightage", int.Parse(textBox5.Text));

            //cmd.ExecuteNonQuery();
            //MessageBox.Show("Successfully saved");

            var con = Configuration.getInstance().getConnection();
            if (!string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                MessageBox.Show("Invalid Id. The Id textbox should be empty for a new entry.");
                return; // Exit the method if validation fails
            }
            // Validate TotalMarks as Integer
            if (!int.TryParse(textBox4.Text, out int totalMarks))
            {
                MessageBox.Show("Invalid Total Marks format. Please enter a valid integer for the Total Marks.");
                return; // Exit the method if validation fails
            }
            // Validate TotalWeightage as Integer
            if (!int.TryParse(textBox5.Text, out int totalWeightage))
            {
                MessageBox.Show("Invalid Total Weightage format. Please enter a valid integer for the Total Weightage.");
                return; // Exit the method if validation fails
            }
            SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[Assessment] VALUES (@Id, @Title, @DateCreated, @TotalMarks, @TotalWeightage)", con);
            //cmd.Parameters.AddWithValue("@Id", textid);
            cmd.Parameters.AddWithValue("@Title", textBox2.Text);
            cmd.Parameters.AddWithValue("@DateCreated", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
            cmd.Parameters.AddWithValue("@TotalWeightage", totalWeightage);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully saved");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from dbo.Assessment", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //var con = Configuration.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("UPDATE Assessment SET Title = @title,DateCreated = @DateCreated, TotalMarks = @TotalMarks, TotalWeightage = @TotalWeightage WHERE Id = @Id", con);
            //cmd.Parameters.AddWithValue("@Title", textBox2.Text);
            //cmd.Parameters.AddWithValue("@DateCreated", dateTimePicker1.Value);
            //cmd.Parameters.AddWithValue("@TotalMarks", int.Parse(textBox4.Text));
            //cmd.Parameters.AddWithValue("@TotalWeightage", int.Parse(textBox5.Text));
            //cmd.Parameters.AddWithValue("@Id", int.Parse(textBox1.Text));
            //cmd.ExecuteNonQuery();
            //MessageBox.Show("Successfully updated");
            var con = Configuration.getInstance().getConnection();
            // Validate Id as Integer
            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("Invalid Id format. Please enter a valid integer for the Id.");
                return; // Exit the method if validation fails
            }
            // Validate TotalMarks as Integer
            if (!int.TryParse(textBox4.Text, out int totalMarks))
            {
                MessageBox.Show("Invalid Total Marks format. Please enter a valid integer for the Total Marks.");
                return; // Exit the method if validation fails
            }
            // Validate TotalWeightage as Integer
            if (!int.TryParse(textBox5.Text, out int totalWeightage))
            {
                MessageBox.Show("Invalid Total Weightage format. Please enter a valid integer for the Total Weightage.");
                return; // Exit the method if validation fails
            }
            SqlCommand cmd = new SqlCommand("UPDATE Assessment SET Title = @Title, DateCreated = @DateCreated, TotalMarks = @TotalMarks, TotalWeightage = @TotalWeightage WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Title", textBox2.Text);
            cmd.Parameters.AddWithValue("@DateCreated", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
            cmd.Parameters.AddWithValue("@TotalWeightage", totalWeightage);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully updated");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //var con = Configuration.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("DELETE FROM Assessment WHERE Id = @Id", con);
            //cmd.Parameters.AddWithValue("@Id", int.Parse(textBox1.Text));
            //cmd.ExecuteNonQuery();
            //MessageBox.Show("Successfully deleted");

            var con = Configuration.getInstance().getConnection();
            // Validate ID as Integer
            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("Invalid ID format. Please enter a valid integer for the ID.");
                return; // Exit the method if validation fails
            }
            // Get the current maximum identity value before deleting
            SqlCommand getMaxIdCmd = new SqlCommand("SELECT MAX(Id) FROM Assessment", con);
            int maxIdBeforeDelete = Convert.ToInt32(getMaxIdCmd.ExecuteScalar());
            SqlCommand cmd = new SqlCommand("DELETE FROM Assessment WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", int.Parse(textBox1.Text));
            cmd.ExecuteNonQuery();
            // Get the new maximum identity value after deleting
            int maxIdAfterDelete = maxIdBeforeDelete - 1;
            // Reset the identity column to the new maximum value
            SqlCommand resetIdentityCmd = new SqlCommand($"DBCC CHECKIDENT ('Assessment', RESEED, {maxIdAfterDelete})", con);
            resetIdentityCmd.ExecuteNonQuery();
            MessageBox.Show("Successfully deleted and reset identity");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Title, DateCreated, TotalMarks, TotalWeightage FROM Assessment WHERE Id = @Id", con);
            // Validate ID as Integer
            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("Invalid ID format. Please enter a valid integer for the ID.");
                return; // Exit the method if validation fails
            }
            cmd.Parameters.AddWithValue("@Id", int.Parse(textBox1.Text));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
