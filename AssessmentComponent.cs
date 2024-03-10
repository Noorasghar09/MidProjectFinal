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

namespace ProjectB
{
    public partial class AssessmentComponent : UserControl
    {
        public AssessmentComponent()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();

            // Validate that the Id textbox is empty
            if (!string.IsNullOrEmpty(textBox5.Text.Trim()))
            {
                MessageBox.Show("Invalid Id. The Id textbox should be empty for a new entry.");
                return; // Exit the method if validation fails
            }
            // Validate RubricId as Integer
            if (!int.TryParse(textBox7.Text, out int rubricId))
            {
                MessageBox.Show("Invalid RubricId format. Please enter a valid integer for the RubricId.");
                return; // Exit the method if validation fails
            }
            // Validate TotalMarks as Integer
            if (!int.TryParse(textBox3.Text, out int totalMarks))
            {
                MessageBox.Show("Invalid Total Marks format. Please enter a valid integer for the Total Marks.");
                return; // Exit the method if validation fails
            }
            // Validate AssessmentId as Integer
            if (!int.TryParse(textBox9.Text, out int assessmentId))
            {
                MessageBox.Show("Invalid AssessmentId format. Please enter a valid integer for the AssessmentId.");
                return; // Exit the method if validation fails
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[AssessmentComponent] VALUES (@Name, @RubricId, @TotalMarks, @DateCreated, @DateUpdated, @AssessmentId)", con);
            cmd.Parameters.AddWithValue("@Name", textBox6.Text);
            cmd.Parameters.AddWithValue("@RubricId", rubricId);
            cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
            cmd.Parameters.AddWithValue("@DateCreated", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@DateUpdated", dateTimePicker2.Value);
            cmd.Parameters.AddWithValue("@AssessmentId", assessmentId);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully saved");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from dbo.AssessmentComponent", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();

            // Validate Id as Integer
            if (!int.TryParse(textBox5.Text, out int id))
            {
                MessageBox.Show("Invalid Id format. Please enter a valid integer for the Id.");
                return; // Exit the method if validation fails
            }
            // Validate RubricId as Integer
            if (!int.TryParse(textBox7.Text, out int rubricId))
            {
                MessageBox.Show("Invalid RubricId format. Please enter a valid integer for the RubricId.");
                return; // Exit the method if validation fails
            }
            // Validate TotalMarks as Integer
            if (!int.TryParse(textBox3.Text, out int totalMarks))
            {
                MessageBox.Show("Invalid Total Marks format. Please enter a valid integer for the Total Marks.");
                return; // Exit the method if validation fails
            }
            // Validate AssessmentId as Integer
            if (!int.TryParse(textBox9.Text, out int assessmentId))
            {
                MessageBox.Show("Invalid AssessmentId format. Please enter a valid integer for the AssessmentId.");
                return; // Exit the method if validation fails
            }
            SqlCommand cmd = new SqlCommand("UPDATE Clo SET Name = @Name, RubricId = @RubricId, TotalMarks = @TotalMarks, DateCreated = @DateCreated, DateUpdated = @DateUpdated, AssessmentId = @AssessmentId WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Name", textBox6.Text);
            cmd.Parameters.AddWithValue("@RubricId", rubricId);
            cmd.Parameters.AddWithValue("@TotalMarks", totalMarks);
            cmd.Parameters.AddWithValue("@DateCreated", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@DateUpdated", dateTimePicker2.Value);
            cmd.Parameters.AddWithValue("@AssessmentId", assessmentId);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully updated");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            // Validate Id as Integer
            if (!int.TryParse(textBox5.Text, out int id))
            {
                MessageBox.Show("Invalid Id format. Please enter a valid integer for the Id.");
                return; // Exit the method if validation fails
            }
            // Get the current maximum identity value before deleting
            SqlCommand getMaxIdCmd = new SqlCommand("SELECT MAX(Id) FROM AssessmentComponent", con);
            int maxIdBeforeDelete = Convert.ToInt32(getMaxIdCmd.ExecuteScalar());
            SqlCommand deleteCmd = new SqlCommand("DELETE FROM AssessmentComponent WHERE Id = @Id", con);
            deleteCmd.Parameters.AddWithValue("@Id", int.Parse(textBox5.Text));
            deleteCmd.ExecuteNonQuery();
            // Get the new maximum identity value after deleting
            int maxIdAfterDelete = maxIdBeforeDelete - 1;
            // Reset the identity column to the new maximum value
            SqlCommand resetIdentityCmd = new SqlCommand($"DBCC CHECKIDENT ('AssessmentComponent', RESEED, {maxIdAfterDelete})", con);
            resetIdentityCmd.ExecuteNonQuery();
            MessageBox.Show("Successfully deleted and reset identity");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            // Validate Id as Integer
            if (!int.TryParse(textBox5.Text, out int id))
            {
                MessageBox.Show("Invalid Id format. Please enter a valid integer for the Id.");
                return; // Exit the method if validation fails
            }
            SqlCommand cmd = new SqlCommand("Select RubricId, Details, MeasurementLevel FROM RubricLevel WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", int.Parse(textBox5.Text));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
