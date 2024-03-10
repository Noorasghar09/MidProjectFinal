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
    public partial class Rubric : UserControl
    {
        public Rubric()
        {
            InitializeComponent();
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

            // Validate CloId as Integer
            if (!int.TryParse(textBox3.Text, out int cloId))
            {
                MessageBox.Show("Invalid CloId format. Please enter a valid integer for the CloId.");
                return; // Exit the method if validation fails
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[Rubric] VALUES (@Details, @CloId)", con);
            cmd.Parameters.AddWithValue("@Details", textBox2.Text);
            cmd.Parameters.AddWithValue("@CloId", cloId);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully saved");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from dbo.Rubric", con);
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
            // Validate CloId as Integer
            if (!int.TryParse(textBox3.Text, out int cloId))
            {
                MessageBox.Show("Invalid CloId format. Please enter a valid integer for the CloId.");
                return; // Exit the method if validation fails
            }
            SqlCommand cmd = new SqlCommand("UPDATE Rubric SET Details=@Details, CloId = @CloId WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Details", textBox2.Text);
            cmd.Parameters.AddWithValue("@CloId", cloId);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully updated");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            // Validate Id as Integer
            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("Invalid Id format. Please enter a valid integer for the Id.");
                return; // Exit the method if validation fails
            }
            // Get the current maximum identity value before deleting
            SqlCommand getMaxIdCmd = new SqlCommand("SELECT MAX(Id) FROM Rubric", con);
            int maxIdBeforeDelete = Convert.ToInt32(getMaxIdCmd.ExecuteScalar());
            SqlCommand deleteCmd = new SqlCommand("DELETE FROM Rubric WHERE Id = @Id", con);
            deleteCmd.Parameters.AddWithValue("@Id", int.Parse(textBox1.Text));
            deleteCmd.ExecuteNonQuery();
            // Get the new maximum identity value after deleting
            int maxIdAfterDelete = maxIdBeforeDelete - 1;
            // Reset the identity column to the new maximum value
            SqlCommand resetIdentityCmd = new SqlCommand($"DBCC CHECKIDENT ('Rubric', RESEED, {maxIdAfterDelete})", con);
            resetIdentityCmd.ExecuteNonQuery();
            MessageBox.Show("Successfully deleted and reset identity");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            // Validate Id as Integer
            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("Invalid Id format. Please enter a valid integer for the Id.");
                return; // Exit the method if validation fails
            }
            SqlCommand cmd = new SqlCommand("Select Details, CloId FROM Rubric WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", int.Parse(textBox1.Text));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
