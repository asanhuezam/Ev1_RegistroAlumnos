using Ev1_RegistroAlumnos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ev1_RegistroAlumnos.Fomularios
{
    public partial class FormAlumno : Form
    {
        private registroAlumnosEntities db = new registroAlumnosEntities();
        int idAlumno = 0;
        Helpers h = new Helpers();

        public FormAlumno()
        {
            InitializeComponent();
            cargarAlumno();
            cargarCurso();
            txtVerificador.MaxLength = 1;
            txtRut.MaxLength = 8;
        }
        private void cargarAlumno()
        {
            var listaAlumno = (from a in db.Alumno
                              select new
                              {
                                  a.id_alumno,
                                  a.id_curso,
                                  Rut = a.rut,
                                  Nombre = a.nombres,
                                  Apellido = a.apellidos,
                                  Direccion = a.direccion,
                                  Curso = a.Curso.nivel + "°" + a.Curso.letra
                              }).ToList();
            dgvAlumnos.DataSource = listaAlumno;
            dgvAlumnos.Columns[0].Visible = false;
            dgvAlumnos.Columns[1].Visible = false;
        }

        private void cargarCurso()
        {
            var listaCurso = (from c in db.Curso
                             select new
                             {
                                 id = c.id_curso,
                                 Curso = c.nivel + "°" + c.letra
                             }).ToList();
            cbCurso.DataSource = listaCurso;
            cbCurso.ValueMember = "id";
            cbCurso.DisplayMember = "Curso";
            cbCurso.SelectedIndex = -1;
        }

        private void Guardar()
        {
            Alumno a = new Alumno();
            a.rut = txtRut.Text.Trim() + "-" + txtVerificador.Text;
            a.nombres = txtNom_A.Text.Trim();
            a.apellidos = txtApe_A.Text.Trim();
            a.direccion = txtDireccion.Text.Trim();
            a.id_curso = int.Parse(cbCurso.SelectedValue.ToString());

            db.Alumno.Add(a);
            db.SaveChanges();
        }

        private void Modificar()
        {
            Alumno a = db.Alumno.Find(idAlumno);
            a.rut = txtRut.Text.Trim() + "-" + txtVerificador.Text;
            a.nombres = txtNom_A.Text.Trim();
            a.apellidos = txtApe_A.Text.Trim();
            a.direccion = txtDireccion.Text.Trim();

            db.SaveChanges();
        }

        private void Limpiar()
        {
            idAlumno = 0;
            txtVerificador.Text = "";
            txtRut.Text = "";
            txtNom_A.Text = "";
            txtApe_A.Text = "";
            txtDireccion.Text = "";
            cbCurso.SelectedIndex = -1;
            dgvAlumnos.ClearSelection();
            btnEliminar.Enabled = false;
        }

        private string Validar()
        {
            string mensaje = "";
            if (string.IsNullOrEmpty(txtRut.Text.Trim()))
                mensaje = "Ingrese un Rut \n";
            if (string.IsNullOrEmpty(txtVerificador.Text.Trim()))
                mensaje += "Ingrese un digito verificador \n";
            if (string.IsNullOrEmpty(txtNom_A.Text.Trim()))
                mensaje += "Ingrese un Nombre \n";
            if (string.IsNullOrEmpty(txtApe_A.Text.Trim()))
                mensaje += "Ingrese al menos un Apellido \n";
            if (string.IsNullOrEmpty(txtDireccion.Text.Trim()))
                mensaje += "Ingrese una Direccion \n";
            if (string.IsNullOrEmpty(cbCurso.Text.Trim()))
                mensaje += "Debe seleccionar un Curso \n";
            return mensaje;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idAlumno > 0)
            {
                var resultado = MessageBox.Show("¿Desea eliminar al alumno: " + txtNom_A.Text +
                    " " + txtApe_A.Text + ", rut " + txtRut.Text
                    + "-" + txtVerificador.Text + "?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (resultado == DialogResult.Yes)
                {
                    Alumno a = db.Alumno.Find(idAlumno);
                    db.Alumno.Remove(a);
                    db.SaveChanges();
                    cargarAlumno();
                    Limpiar();
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string error = Validar();
            if (error != "")
            {
                MessageBox.Show(error, "Faltan datos!");
            }
            else
            {
                if (idAlumno == 0)
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
                        MessageBox.Show("El alumno se ha guardado con éxito!");
                    }
                }
                else
                {
                    var resultado = MessageBox.Show("¿Desea modificar al alumno: " + txtNom_A.Text + " "
                        + txtApe_A.Text + ", rut " + txtRut.Text + "?", "Modificar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {
                        Modificar();
                    }
                }
                cargarAlumno();
                Limpiar();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void dgvAlumnos_MouseClick(object sender, MouseEventArgs e)
        {
            idAlumno = int.Parse(dgvAlumnos.CurrentRow.Cells[0].Value.ToString());
            cbCurso.SelectedValue = int.Parse(dgvAlumnos.CurrentRow.Cells[1].Value.ToString());
            txtRut.Text = dgvAlumnos.CurrentRow.Cells[2].Value.ToString();
            txtNom_A.Text = dgvAlumnos.CurrentRow.Cells[3].Value.ToString();
            txtApe_A.Text = dgvAlumnos.CurrentRow.Cells[4].Value.ToString();
            txtDireccion.Text = dgvAlumnos.CurrentRow.Cells[5].Value.ToString();

            btnEliminar.Enabled = true;
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
            Alumno alumno = db.Alumno.FirstOrDefault(a => a.rut.Equals(rut) && a.id_alumno != idAlumno);
            if (alumno != null)
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
