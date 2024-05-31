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
    public partial class Form1 : Form

    {
        // Declarar los controles a nivel de clase
        private Label labelNombre;
        private TextBox textBoxNombre;
        private ComboBox comboBoxGenero;
        private DateTimePicker dateTimePickerNacimiento;
        private Button btnGuardar;
        private TextBox textBoxCedula;
        private TextBox textBoxEdad;
        private Button btnAbrirObras;
        private Button btnGestion;

        public Form1()
        {
            InitializeComponent();
            // Inicializar el Label para el nombre
            labelNombre = new Label();
            labelNombre.Text = "Nombre:";
            labelNombre.Location = new Point(10, 10);
            labelNombre.Size = new Size(80, 20);
            this.Controls.Add(labelNombre);

            // Inicializar el TextBox para el nombre
            textBoxNombre = new TextBox();
            textBoxNombre.Location = new Point(100, 10);
            textBoxNombre.Size = new Size(200, 20);
            this.Controls.Add(textBoxNombre);

            // Label y ComboBox para Género
            Label labelGenero = new Label();
            labelGenero.Text = "Género:";
            labelGenero.Location = new Point(10, 40);
            labelGenero.Size = new Size(80, 20);
            this.Controls.Add(labelGenero);

            comboBoxGenero = new ComboBox();
            comboBoxGenero.Location = new Point(100, 40);
            comboBoxGenero.Size = new Size(200, 20);
            comboBoxGenero.Items.AddRange(new object[] { "Femenino", "Masculino" });
            this.Controls.Add(comboBoxGenero);

            // Label y DateTimePicker para Fecha de Nacimiento
            Label labelFechaNacimiento = new Label();
            labelFechaNacimiento.Text = "Fecha de Nacimiento:";
            labelFechaNacimiento.Location = new Point(10, 70);
            labelFechaNacimiento.Size = new Size(140, 20);
            this.Controls.Add(labelFechaNacimiento);

            dateTimePickerNacimiento = new DateTimePicker();
            dateTimePickerNacimiento.Location = new Point(160, 70);
            dateTimePickerNacimiento.Size = new Size(140, 20);
            dateTimePickerNacimiento.Format = DateTimePickerFormat.Short;
            this.Controls.Add(dateTimePickerNacimiento);

            // Label y TextBox para Cédula
            Label labelCedula = new Label();
            labelCedula.Text = "Cédula:";
            labelCedula.Location = new Point(10, 100);
            labelCedula.Size = new Size(80, 20);
            this.Controls.Add(labelCedula);

            textBoxCedula = new TextBox();
            textBoxCedula.Location = new Point(100, 100);
            textBoxCedula.Size = new Size(200, 20);
            this.Controls.Add(textBoxCedula);

            // Label y TextBox para Edad
            Label labelEdad = new Label();
            labelEdad.Text = "Edad:";
            labelEdad.Location = new Point(10, 130);
            labelEdad.Size = new Size(80, 20);
            this.Controls.Add(labelEdad);

            textBoxEdad = new TextBox();
            textBoxEdad.Location = new Point(100, 130);  // Asegúrate de que las coordenadas sean correctas
            textBoxEdad.Size = new Size(200, 20);
            this.Controls.Add(textBoxEdad);
            ;

            // Botón para Guardar los datos
            btnGuardar = new Button();
            btnGuardar.Text = "Guardar";
            btnGuardar.Location = new Point(100, 160);
            btnGuardar.Size = new Size(100, 30);
            btnGuardar.Click += new EventHandler(this.btnGuardar_Click);
            this.Controls.Add(btnGuardar);

            btnAbrirObras = new Button();
            btnAbrirObras.Text = "Gestionar Obras";
            btnAbrirObras.Location = new Point(210, 160);
            btnAbrirObras.Size = new Size(100, 30);
            btnAbrirObras.Click += new EventHandler(this.btnAbrirObras_Click);
            this.Controls.Add(btnAbrirObras);

            btnGestion = new Button();
            btnGestion.Text = "Gestionar Datos";
            btnGestion.Location = new Point(320, 160);
            btnGestion.Size = new Size(100, 30);
            btnGestion.Click += new EventHandler(this.btnGestion_Click);
            this.Controls.Add(btnGestion);





        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {


            if (string.IsNullOrWhiteSpace(textBoxNombre.Text))
            {
                MessageBox.Show("Por favor, ingrese el nombre del autor.");
                return;
            }

            if (comboBoxGenero.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione un género.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxCedula.Text))
            {
                MessageBox.Show("Por favor, ingrese la cédula del autor.");
                return;
            }

            if (textBoxEdad == null || string.IsNullOrWhiteSpace(textBoxEdad.Text))
            {
                MessageBox.Show("Por favor, ingrese la edad del autor.");
                return;
            }

            if (!int.TryParse(textBoxEdad.Text, out int edad) || edad <= 0 || edad > 120)
            {
                MessageBox.Show("Por favor, ingrese una edad válida.");
                return;
            }

            GuardarDatos();
        }
        private void GuardarDatos()
        {
            // Implementación de la lógica de guardado
            // Supongamos que tienes una conexión de base de datos llamada 'connection'
            string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Autores (Nombre_autor, Genero_autor, Fecha_nacimiento, Cedula_autor, Edad) VALUES (@Nombre, @Genero, @FechaNacimiento, @Cedula, @Edad)", conn);

                    cmd.Parameters.AddWithValue("@Nombre", textBoxNombre.Text);
                    cmd.Parameters.AddWithValue("@Genero", comboBoxGenero.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Cedula", textBoxCedula.Text); // Asegúrate de que textBoxCedula no esté vacío
                    cmd.Parameters.AddWithValue("@FechaNacimiento", dateTimePickerNacimiento.Value);
                    cmd.Parameters.AddWithValue("@Edad", textBoxEdad.Text);



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
        private void btnAbrirObras_Click(object sender, EventArgs e)
        {
            FormObras formObras = new FormObras();
            formObras.Show();
        }

        private void btnGestion_Click(object sender, EventArgs e)
        {
            FormGestion formGestion = new FormGestion();
            formGestion.Show();
        }

    }
}

