using Ev1_RegistroAlumnos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ev1_RegistroAlumnos.Fomularios
{
    public partial class FormProfJefe : Form
    {
        private registroAlumnosEntities db = new registroAlumnosEntities();

        int idProf = 0;

        private Helpers h = new Helpers();

        public FormProfJefe()
        {
            InitializeComponent();
            cargarProf();
            txtVerificador.MaxLength = 1;
            txtRut.MaxLength = 8;
            txtTelefono.MaxLength = 8;
        }

        private void cargarProf()
        {
            var listaProf = (from p in db.Prof_Jefe
                                  select new
                                  {
                                      p.id_prof,
                                      Rut = p.rut,
                                      Nombres = p.nombres,
                                      Apellidos = p.apellidos,
                                      Telefono = p.telefono,
                                      Direccion = p.direccion
                                  }).ToList();
            dgvProfesores.DataSource = listaProf;
            dgvProfesores.Columns[0].Visible = false;
        }

        private void Guardar()
        {
            Prof_Jefe p = new Prof_Jefe();
            p.rut = txtRut.Text.Trim() + "-" + txtVerificador.Text;
            p.nombres = txtNombres.Text.Trim();
            p.apellidos = txtApellidos.Text.Trim();  
            p.telefono = int.Parse("9" + txtTelefono.Text);
            p.direccion = txtDireccion.Text.Trim();

            db.Prof_Jefe.Add(p);
            db.SaveChanges();
        }

        private void Modificar()
        {
            Prof_Jefe p = db.Prof_Jefe.Find(idProf);
            p.rut = txtRut.Text.Trim() + "-" + txtVerificador.Text;
            p.nombres = txtNombres.Text.Trim();
            p.apellidos = txtApellidos.Text.Trim();
            p.telefono = int.Parse(txtTelefono.Text);
            p.direccion = txtDireccion.Text.Trim();

            db.SaveChanges();

        }

        private void Limpiar()
        {
            idProf = 0;
            txtRut.Text = "";
            txtVerificador.Text = "";
            txtNombres.Text = "";
            txtApellidos.Text = "";
            txtTelefono.Text = "";
            txtDireccion.Text = "";
            dgvProfesores.ClearSelection();
            btnEliminar.Enabled = false;
        }

        private string Validar()
        {
            string mensaje = "";
            if (string.IsNullOrEmpty(txtRut.Text.Trim()))
                mensaje = "Ingrese un Rut \n";
            if (string.IsNullOrEmpty(txtVerificador.Text.Trim()))
                mensaje += "Ingrese un digito verificador \n";
            if (string.IsNullOrEmpty(txtNombres.Text.Trim()))
                mensaje += "Ingrese al menos un Nombre \n";
            if (string.IsNullOrEmpty(txtApellidos.Text.Trim()))
                mensaje += "Ingrese al menos un Apellido \n";
            if (string.IsNullOrEmpty(txtTelefono.Text.Trim()))
                mensaje += "Debe ingresar un Telefono \n";
            if (string.IsNullOrEmpty(txtDireccion.Text.Trim()))
                mensaje += "Debe ingresar una Direccion \n";
            return mensaje;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idProf > 0)
            {
                var resultado = MessageBox.Show("¿Desea eliminar al profesor " + txtApellidos.Text + 
                    " de rut " + txtRut.Text + "-" + txtVerificador.Text + " ?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (resultado == DialogResult.Yes)
                {
                    Prof_Jefe p = db.Prof_Jefe.Find(idProf);
                    db.Prof_Jefe.Remove(p);
                    db.SaveChanges();
                    cargarProf();
                    Limpiar();
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string error = Validar();
            if (error != "")
            {
                MessageBox.Show(error, "Faltan datos");
            }
            else
            {
                if (idProf == 0)
                {
                    String dig = comprobarRut(int.Parse(txtRut.Text));
                    if (txtVerificador.Text != dig)
                    {
                        MessageBox.Show("El rut ingresado es invalido");
                        txtRut.Text = "";
                        txtVerificador.Text = "";
                    }
                    else
                    {
                        Guardar();
                        MessageBox.Show("El profesor se ha guardado con éxito");
                    }
                }
                else
                {
                    var resultado = MessageBox.Show("¿Desea modificar al profesor " + txtApellidos.Text +
                        " de rut " + txtRut.Text + " ?", "Modificar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {
                        Modificar();
                    }
                  
                }
                cargarProf();
                Limpiar();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void dgvProfesores_MouseClick(object sender, MouseEventArgs e)
        {
            idProf = int.Parse(dgvProfesores.CurrentRow.Cells[0].Value.ToString());
            txtRut.Text = dgvProfesores.CurrentRow.Cells[1].Value.ToString();
            txtNombres.Text = dgvProfesores.CurrentRow.Cells[2].Value.ToString();
            txtApellidos.Text = dgvProfesores.CurrentRow.Cells[3].Value.ToString();            
            txtTelefono.Text = dgvProfesores.CurrentRow.Cells[4].Value.ToString();
            txtDireccion.Text = dgvProfesores.CurrentRow.Cells[5].Value.ToString();

            btnEliminar.Enabled = true;
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            h.soloNumeros(e);
        }

        private void txtRut_KeyPress(object sender, KeyPressEventArgs e)
        {
            h.soloNumeros(e);
        }

        private void lblAtras_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form = new Form1();
            form.Show();
        }

        private bool verficarRut(string rut)
        {
            bool result = false;
            Prof_Jefe profe = db.Prof_Jefe.FirstOrDefault(p => p.rut.Equals(rut) && p.id_prof != idProf);
            if (profe != null)
            {
                result = true;
            }
            return result;
        }

        private void txtRut_Leave(object sender, EventArgs e)
        {
            if (txtRut.Text.Trim() != "")
            {
                if (verficarRut(txtRut.Text.Trim()))
                {
                    MessageBox.Show("El rut ingresado ya se ha registrado");
                    txtRut.Text = "";
                }
            }
        }

        private void txtVerificador_KeyPress(object sender, KeyPressEventArgs e)
        {
            h.verificadorChar(e);
        }

        private String comprobarRut(int rut)
        {
            int digito;
            int multiplicador = 2;
            int producto;
            int suma = 0;
            String digitoFinal;

            while (rut != 0)
            {
                producto = (rut % 10) * multiplicador;
                suma = suma + producto;
                rut = rut / 10;
                multiplicador++;
                if (multiplicador == 8)
                {
                    multiplicador = 2;
                }
            }

            digito = 11 - (suma % 11);
            digitoFinal = digito.ToString().Trim();
            if (digito == 10)
            {
                digitoFinal = "K";
            }
            if (digito == 11)
            {
                digitoFinal = "0";
            }
            return digitoFinal;
        }
    }
}