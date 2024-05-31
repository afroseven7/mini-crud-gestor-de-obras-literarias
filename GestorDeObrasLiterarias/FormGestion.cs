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
using System.Configuration;

namespace GestorDeObrasLiterarias
{
    public partial class FormGestion : Form
    {
        private ComboBox comboBoxTablas;
        private DataGridView dataGridView;
        private Button btnCargar;
        private Button btnGuardarCambios;
        private Button btnEliminar;

        public FormGestion()
        {
            InitializeComponent();

            // Inicializar el ComboBox para seleccionar la tabla
            comboBoxTablas = new ComboBox();
            comboBoxTablas.Location = new Point(10, 10);
            comboBoxTablas.Size = new Size(200, 20);
            comboBoxTablas.Items.AddRange(new object[] { "Autores", "Obras" });
            this.Controls.Add(comboBoxTablas);

            // Inicializar el DataGridView para mostrar los datos
            dataGridView = new DataGridView();
            dataGridView.Location = new Point(10, 40);
            dataGridView.Size = new Size(460, 200);
            this.Controls.Add(dataGridView);

            // Inicializar el botón para cargar los datos
            btnCargar = new Button();
            btnCargar.Text = "Cargar Datos";
            btnCargar.Location = new Point(220, 10);
            btnCargar.Size = new Size(100, 20);
            btnCargar.Click += new EventHandler(this.btnCargar_Click);
            this.Controls.Add(btnCargar);

            // Inicializar el botón para guardar los cambios
            btnGuardarCambios = new Button();
            btnGuardarCambios.Text = "Guardar Cambios";
            btnGuardarCambios.Location = new Point(10, 250);
            btnGuardarCambios.Size = new Size(100, 30);
            btnGuardarCambios.Click += new EventHandler(this.btnGuardarCambios_Click);
            this.Controls.Add(btnGuardarCambios);

            // Inicializar el botón para eliminar los registros seleccionados
            btnEliminar = new Button();
            btnEliminar.Text = "Eliminar";
            btnEliminar.Location = new Point(120, 250);
            btnEliminar.Size = new Size(100, 30);
            btnEliminar.Click += new EventHandler(this.btnEliminar_Click);
            this.Controls.Add(btnEliminar);
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            string tablaSeleccionada = comboBoxTablas.SelectedItem.ToString();
            CargarDatos(tablaSeleccionada);
        }

        private void CargarDatos(string tabla)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter($"SELECT * FROM {tabla}", conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los datos: " + ex.Message);
                }
            }
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            string tablaSeleccionada = comboBoxTablas.SelectedItem.ToString();
            GuardarCambios(tablaSeleccionada);
        }

        private void GuardarCambios(string tabla)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter($"SELECT * FROM {tabla}", conn);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                    DataTable dataTable = (DataTable)dataGridView.DataSource;
                    dataAdapter.Update(dataTable);
                    MessageBox.Show("Cambios guardados correctamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar los cambios: " + ex.Message);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView.SelectedRows)
            {
                dataGridView.Rows.RemoveAt(row.Index);
            }
            MessageBox.Show("Registro(s) eliminado(s). Asegúrate de guardar los cambios para aplicarlos a la base de datos.");
        }
    }
}
