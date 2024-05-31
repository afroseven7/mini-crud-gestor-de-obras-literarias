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
    public partial class FormObras : Form
    {
        private Label labelTitulo;
        private TextBox textBoxTitulo;
        private DateTimePicker dateTimePickerCreacion;
        private DateTimePicker dateTimePickerRegistro;
        private ComboBox comboBoxAutor;
        private CheckBox checkBoxPublica;
        private TextBox textBoxTexto;
        private Button btnGuardar;

        public FormObras()
        {
            InitializeComponent();

            // Inicializar el Label para el título
            labelTitulo = new Label();
            labelTitulo.Text = "Título:";
            labelTitulo.Location = new Point(10, 10);
            labelTitulo.Size = new Size(80, 20);
            this.Controls.Add(labelTitulo);

            // Inicializar el TextBox para el título
            textBoxTitulo = new TextBox();
            textBoxTitulo.Location = new Point(100, 10);
            textBoxTitulo.Size = new Size(200, 20);
            this.Controls.Add(textBoxTitulo);

            // Inicializar y configurar el DateTimePicker para Fecha de Creación
            Label labelFechaCreacion = new Label();
            labelFechaCreacion.Text = "Fecha de Creación:";
            labelFechaCreacion.Location = new Point(10, 40);
            labelFechaCreacion.Size = new Size(120, 20);
            this.Controls.Add(labelFechaCreacion);

            dateTimePickerCreacion = new DateTimePicker();
            dateTimePickerCreacion.Location = new Point(140, 40);
            dateTimePickerCreacion.Size = new Size(160, 20);
            dateTimePickerCreacion.Format = DateTimePickerFormat.Short;
            this.Controls.Add(dateTimePickerCreacion);

            // Inicializar y configurar el DateTimePicker para Fecha de Registro
            Label labelFechaRegistro = new Label();
            labelFechaRegistro.Text = "Fecha de Registro:";
            labelFechaRegistro.Location = new Point(10, 70);
            labelFechaRegistro.Size = new Size(120, 20);
            this.Controls.Add(labelFechaRegistro);

            dateTimePickerRegistro = new DateTimePicker();
            dateTimePickerRegistro.Location = new Point(140, 70);
            dateTimePickerRegistro.Size = new Size(160, 20);
            dateTimePickerRegistro.Format = DateTimePickerFormat.Short;
            this.Controls.Add(dateTimePickerRegistro);

            // Inicializar y configurar el ComboBox para Autor (Cédula)
            Label labelAutor = new Label();
            labelAutor.Text = "Cédula Autor:";
            labelAutor.Location = new Point(10, 100);
            labelAutor.Size = new Size(120, 20);
            this.Controls.Add(labelAutor);

            comboBoxAutor = new ComboBox();
            comboBoxAutor.Location = new Point(140, 100);
            comboBoxAutor.Size = new Size(160, 20);
            // Aquí debes llenar el ComboBox con los valores de cédula desde la base de datos
            LlenarComboBoxAutor();
            this.Controls.Add(comboBoxAutor);

            // Inicializar y configurar el CheckBox para Publica
            checkBoxPublica = new CheckBox();
            checkBoxPublica.Text = "¿Publica?";
            checkBoxPublica.Location = new Point(10, 130);
            this.Controls.Add(checkBoxPublica);

            // Inicializar el TextBox para el texto de la obra
            Label labelTexto = new Label();
            labelTexto.Text = "Texto:";
            labelTexto.Location = new Point(10, 160);
            labelTexto.Size = new Size(80, 20);
            this.Controls.Add(labelTexto);

            textBoxTexto = new TextBox();
            textBoxTexto.Location = new Point(100, 160);
            textBoxTexto.Size = new Size(200, 100);
            textBoxTexto.Multiline = true;
            this.Controls.Add(textBoxTexto);

            // Inicializar y configurar el botón para guardar los datos
            btnGuardar = new Button();
            btnGuardar.Text = "Guardar";
            btnGuardar.Location = new Point(100, 270);
            btnGuardar.Size = new Size(100, 30);
            btnGuardar.Click += new EventHandler(this.btnGuardar_Click);
            this.Controls.Add(btnGuardar);
        }

        private void LlenarComboBoxAutor()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Cedula_autor FROM Autores", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBoxAutor.Items.Add(reader["Cedula_autor"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al llenar el ComboBox de autores: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validaciones de los campos de entrada
            if (string.IsNullOrWhiteSpace(textBoxTitulo.Text))
            {
                MessageBox.Show("Por favor, ingrese el título de la obra.");
                return;
            }

            if (comboBoxAutor.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione un autor.");
                return;
            }

            GuardarDatos();
        }

        private void GuardarDatos()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Obras (Titulo_obra, Fecha_creacion, Fecha_registro, Cedula_autor, Publica, Texto_obra) VALUES (@Titulo, @FechaCreacion, @FechaRegistro, @CedulaAutor, @Publica, @Texto)", conn);
                    cmd.Parameters.AddWithValue("@Titulo", textBoxTitulo.Text);
                    cmd.Parameters.AddWithValue("@FechaCreacion", dateTimePickerCreacion.Value);
                    cmd.Parameters.AddWithValue("@FechaRegistro", dateTimePickerRegistro.Value);
                    cmd.Parameters.AddWithValue("@CedulaAutor", comboBoxAutor.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Publica", checkBoxPublica.Checked);
                    cmd.Parameters.AddWithValue("@Texto", textBoxTexto.Text);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Datos guardados correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("No se pudieron guardar los datos.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar los datos: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
