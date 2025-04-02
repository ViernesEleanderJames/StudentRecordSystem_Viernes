using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using StudentRecordSystem_Viernes;

namespace StudentRecordSystem_Viernes
{
    public partial class Student_Page : Form
    {
        private MySqlConnection connection;

        public Student_Page()
        {
            InitializeComponent();
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            string connectionString = "server=localhost;database=StudentInfoDB;user=root;password=12345;";
            connection = new MySqlConnection(connectionString);
        }

        private void Student_Page_Load(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                connection.Open();
                string query = "SELECT s.studentId, s.firstName, s.lastName, c.courseName FROM StudentRecordTB s " +
                               "JOIN CourseTB c ON s.courseId = c.courseId";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView.DataSource = dataTable;

                // Set auto-sizing
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                // Remove extra spacing
                dataGridView.RowHeadersVisible = false;
                dataGridView.AllowUserToResizeRows = false;

                // Adjust column width to fit content
                dataGridView.Columns[0].Width = 50;  // For studentId column
                dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;  // Adjusts First Name column
                dataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;  // Adjusts Last Name column
                dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;  // Adjusts Course column

                // Add the View Button Column
                DataGridViewButtonColumn viewButton = new DataGridViewButtonColumn();
                viewButton.Name = "ViewBtn";
                viewButton.HeaderText = "Action";
                viewButton.Text = "VIEW";
                viewButton.UseColumnTextForButtonValue = true;
                dataGridView.Columns.Add(viewButton);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns["ViewBtn"].Index && e.RowIndex >= 0)
            {
                int studentId = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["studentId"].Value);

                // Open the StudentPage_Individual form with the selected studentId
                StudentPage_Individual individualForm = new StudentPage_Individual(studentId);
                individualForm.Show();
            }
        }
    }
}
