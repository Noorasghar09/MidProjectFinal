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
    public partial class RubricLevel : UserControl
    {
        public RubricLevel()
        {
            InitializeComponent();
        }

        private void RubricLevel_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            // Validate that the Id textbox is empty
            if (!string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                MessageBox.Show("Invalid Id. The Id textbox should be empty for a new entry.");
                return; // Exit the method if validation fails
            }
            // Validate RubricId as Integer
            if (!int.TryParse(textBox2.Text, out int rubricId))
            {
                MessageBox.Show("Invalid RubricId format. Please enter a valid integer for the RubricId.");
                return; // Exit the method if validation fails
            }
            // Validate MeasurementLevel as Integer
            if (!int.TryParse(textBox4.Text, out int measurementLevel))
            {
                MessageBox.Show("Invalid MeasurementLevel format. Please enter a valid integer for the MeasurementLevel.");
                return; // Exit the method if validation fails
            }
            SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[RubricLevel] VALUES (@RubricId, @Details, @MeasurementLevel)", con);
            cmd.Parameters.AddWithValue("@RubricId", rubricId);
            cmd.Parameters.AddWithValue("@Details", textBox3.Text);
            cmd.Parameters.AddWithValue("@MeasurementLevel", measurementLevel);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully saved");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from dbo.RubricLevel", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            // Validate Id as Integer
            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("Invalid Id format. Please enter a valid integer for the Id.");
                return; // Exit the method if validation fails
            }
            // Validate RubricId as Integer
            if (!int.TryParse(textBox2.Text, out int rubricId))
            {
                MessageBox.Show("Invalid RubricId format. Please enter a valid integer for the RubricId.");
                return; // Exit the method if validation fails
            }
            // Validate MeasurementLevel as Integer
            if (!int.TryParse(textBox4.Text, out int measurementLevel))
            {
                MessageBox.Show("Invalid MeasurementLevel format. Please enter a valid integer for the MeasurementLevel.");
                return; // Exit the method if validation fails
            }
            SqlCommand cmd = new SqlCommand("UPDATE RubricLevel SET RubricId=@RubricId, Details=@Details, MeasurementLevel=@MeasurementLevel WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@RubricId", rubricId);
            cmd.Parameters.AddWithValue("@Details", textBox3.Text);
            cmd.Parameters.AddWithValue("@MeasurementLevel", measurementLevel);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully updated");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("Invalid Id format. Please enter a valid integer for the Id.");
                return; // Exit the method if validation fails
            }
            // Get the current maximum identity value before deleting
            SqlCommand getMaxIdCmd = new SqlCommand("SELECT MAX(Id) FROM RubricLevel", con);
            int maxIdBeforeDelete = Convert.ToInt32(getMaxIdCmd.ExecuteScalar());
            SqlCommand deleteCmd = new SqlCommand("DELETE FROM RubricLevel WHERE Id = @Id", con);
            deleteCmd.Parameters.AddWithValue("@Id", int.Parse(textBox1.Text));
            deleteCmd.ExecuteNonQuery();
            // Get the new maximum identity value after deleting
            int maxIdAfterDelete = maxIdBeforeDelete - 1;
            // Reset the identity column to the new maximum value
            SqlCommand resetIdentityCmd = new SqlCommand($"DBCC CHECKIDENT ('RubricLevel', RESEED, {maxIdAfterDelete})", con);
            resetIdentityCmd.ExecuteNonQuery();
            MessageBox.Show("Successfully deleted and reset identity");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("Invalid Id format. Please enter a valid integer for the Id.");
                return; // Exit the method if validation fails
            }
            SqlCommand cmd = new SqlCommand("Select RubricId, Details, MeasurementLevel FROM RubricLevel WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", int.Parse(textBox1.Text));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
