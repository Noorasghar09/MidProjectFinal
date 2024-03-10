using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf;
using iTextSharp.text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProjectB
{
    public partial class ClassAttendance : UserControl
    {
        public ClassAttendance()
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
            SqlCommand cmd = new SqlCommand("Insert into [dbo].[ClassAttendance] values (@AttendanceDate)", con);
            //cmd.Parameters.AddWithValue("@Id", textBox1.Text);
            cmd.Parameters.AddWithValue("@AttendanceDate", dateTimePicker1.Value);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully saved");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from dbo.ClassAttendance", con);
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
            SqlCommand cmd = new SqlCommand("UPDATE ClassAttendance SET AttendanceDate = @AttendanceDate WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@AttendanceDate", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@Id", int.Parse(textBox1.Text));
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully updated");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //var con = Configuration.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("DELETE FROM ClassAttendance WHERE Id = @Id", con);
            //cmd.Parameters.AddWithValue("@Id", int.Parse(textBox1.Text));
            //cmd.ExecuteNonQuery();
            //MessageBox.Show("Successfully deleted");

            var con = Configuration.getInstance().getConnection();
            // Validate Id as Integer
            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("Invalid Id format. Please enter a valid integer for the Id.");
                return; // Exit the method if validation fails
            }
            // Get the current maximum identity value before deleting
            SqlCommand getMaxIdCmd = new SqlCommand("SELECT MAX(Id) FROM ClassAttendance", con);
            int maxIdBeforeDelete = Convert.ToInt32(getMaxIdCmd.ExecuteScalar());
            SqlCommand cmd = new SqlCommand("DELETE FROM ClassAttendance WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", int.Parse(textBox1.Text));
            cmd.ExecuteNonQuery();
            // Get the new maximum identity value after deleting
            int maxIdAfterDelete = maxIdBeforeDelete - 1;
            // Reset the identity column to the new maximum value
            SqlCommand resetIdentityCmd = new SqlCommand($"DBCC CHECKIDENT ('ClassAttendance', RESEED, {maxIdAfterDelete})", con);
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
            SqlCommand cmd = new SqlCommand("Select ID, AttendanceDate FROM ClassAttendance WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", int.Parse(textBox1.Text));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Check if DataGridView is empty
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No data available in the DataGridView.", "Empty DataGridView", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Exit the method if DataGridView is empty
            }
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            PdfWriter.GetInstance(pdfDoc, new FileStream("Attendance.pdf", FileMode.Create));
            pdfDoc.Open();
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance("boy-student.png");
            image.ScaleAbsolute(50f, 50f);
            pdfDoc.Add(image);
            iTextSharp.text.Font headingFont = FontFactory.GetFont("Times New Roman", 18, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font headingFont1 = FontFactory.GetFont("Times New Roman", 14, iTextSharp.text.Font.BOLD);
            // Create a new paragraph for the heading
            Paragraph heading1 = new Paragraph("Class Management System", headingFont);
            heading1.Alignment = Element.ALIGN_LEFT;
            heading1.SpacingBefore = 10f;
            heading1.SpacingAfter = 10f;
            pdfDoc.Add(heading1);
            LineSeparator line = new LineSeparator();
            pdfDoc.Add(line);
            Paragraph heading = new Paragraph("Attendance Report for classes", headingFont1);
            heading.Alignment = Element.ALIGN_CENTER;
            heading.SpacingBefore = 10f;
            heading.SpacingAfter = 10f;
            pdfDoc.Add(heading);
            LineSeparator line1 = new LineSeparator();
            pdfDoc.Add(line1);
            PdfPTable pdfTable = new PdfPTable(dataGridView1.Columns.Count);

            // Add the heading to the document


            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                pdfTable.AddCell(cell);
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null)
                    {
                        pdfTable.AddCell(cell.Value.ToString());
                    }
                    else
                    {
                        pdfTable.AddCell("");
                    }
                }
            }

            pdfDoc.Add(pdfTable);
            pdfDoc.Close();
            if (File.Exists("Attendance.pdf"))
            {
                System.Diagnostics.Process.Start("Attendance.pdf");
            }
        }
    }
}
