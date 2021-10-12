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
    public partial class FormAlumno : Form
    {
        private registroColegioEntities db = new registroColegioEntities();
        int idAlumno = 0;

        public FormAlumno()
        {
            InitializeComponent();
            cargarAlumno();
            cargarCurso();
        }
        private void cargarAlumno()
        {
            var listaAlumno = (from a in db.Alumno
                              select new
                              {
                                  a.id_alumno,
                                  a.id_curso,
                                  Nombre = a.nombre,
                                  Apellido = a.apellido,
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
            a.nombre = txtNom_A.Text.Trim();
            a.apellido = txtApe_A.Text.Trim();
            a.id_curso = int.Parse(cbCurso.SelectedValue.ToString());

            db.Alumno.Add(a);
            db.SaveChanges();
        }

        private void Modificar()
        {
            Alumno a = db.Alumno.Find(idAlumno);
            a.nombre = txtNom_A.Text.Trim();
            a.apellido = txtApe_A.Text.Trim();

            db.SaveChanges();
        }

        private void Limpiar()
        {
            idAlumno = 0;
            txtNom_A.Text = "";
            txtApe_A.Text = "";
            cbCurso.SelectedIndex = -1;
            dgvAlumnos.ClearSelection();
            btnEliminar.Enabled = false;
        }

        private string Validar()
        {
            string mensaje = "";
            if (string.IsNullOrEmpty(txtNom_A.Text.Trim()))
                mensaje = "Ingrese un Nombre \n";
            if (string.IsNullOrEmpty(txtApe_A.Text.Trim()))
                mensaje += "Ingrese al menos un Apellido \n";
            if (string.IsNullOrEmpty(cbCurso.Text.Trim()))
                mensaje += "Debe seleccionar un Curso \n";
            return mensaje;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idAlumno > 0)
            {
                var resultado = MessageBox.Show("¿Desea eliminar al alumno: " + txtNom_A.Text + " " + txtApe_A.Text + "?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
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
                    Guardar();
                }
                else
                {
                    var resultado = MessageBox.Show("¿Desea modificar al alumno: " + txtNom_A.Text + " " + txtApe_A.Text + "?", "Modificar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {
                        Modificar();
                    }
                }
                MessageBox.Show("El registro se ha guardado con éxito!");
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
            txtNom_A.Text = dgvAlumnos.CurrentRow.Cells[2].Value.ToString();
            txtApe_A.Text = dgvAlumnos.CurrentRow.Cells[3].Value.ToString();

            btnEliminar.Enabled = true;
        }
    }
}
