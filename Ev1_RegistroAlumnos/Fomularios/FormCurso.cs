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
    public partial class FormCurso : Form
    {
        private registroAlumnosEntities db = new registroAlumnosEntities();
        int idCurso = 0;
        private Helpers h = new Helpers();

        public FormCurso()
        {
            InitializeComponent();
            cargarProf();
            cargarCurso();
            txtLetra.MaxLength = 1;
            txtNivel.MaxLength = 1;
        }

        private void cargarProf()
        {
            var listaProf = (from p in db.Prof_Jefe
                             select new
                             {
                                 id = p.id_prof,
                                 Datos = p.rut + " - " + p.nombres + " " + p.apellidos
                             }).ToList();
            cbProfe.DataSource = listaProf;
            cbProfe.ValueMember = "id";
            cbProfe.DisplayMember = "Datos";
            cbProfe.SelectedIndex = -1;
        }

        private void cargarCurso()
        {
            var listaCurso = (from c in db.Curso
                             select new
                             {
                                 c.id_curso,
                                 c.id_prof,
                                 Nivel = c.nivel,
                                 Letra = c.letra,
                                 Profesor = c.Prof_Jefe.nombres + " " + c.Prof_Jefe.apellidos,
                                 Rut_profesor = c.Prof_Jefe.rut
                             }).ToList();
            dgvCursos.DataSource = listaCurso;
            dgvCursos.Columns[0].Visible = false;
            dgvCursos.Columns[1].Visible = false;
        }

        private void Guardar()
        {
            Curso c = new Curso();
            c.nivel = int.Parse(txtNivel.Text);
            c.letra = txtLetra.Text.Trim();
            c.id_prof = int.Parse(cbProfe.SelectedValue.ToString());

            db.Curso.Add(c);
            db.SaveChanges();
        }

        private void Modificar()
        {
            Curso c = db.Curso.Find(idCurso);
            c.nivel = int.Parse(txtNivel.Text);
            c.letra = txtLetra.Text.Trim();

            db.SaveChanges();
        }

        private void Limpiar()
        {
            idCurso = 0;
            txtNivel.Text = "";
            txtLetra.Text = "";
            cbProfe.SelectedIndex = -1;
            dgvCursos.ClearSelection();
            btnEliminar.Enabled = false;
        }

        private string Validar()
        {
            string mensaje = "";
            if (string.IsNullOrEmpty(txtNivel.Text.Trim()))
                mensaje = "Ingrese un Nivel \n";
            if (string.IsNullOrEmpty(txtLetra.Text.Trim()))
                mensaje += "Ingrese una Letra \n";
            if (string.IsNullOrEmpty(cbProfe.Text.Trim()))
                mensaje += "Debe seleccionar un Profesor \n";
            return mensaje;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idCurso > 0)
            {
                var resultado = MessageBox.Show("¿Desea eliminar el curso: " + txtNivel.Text + "°" 
                    + txtLetra.Text + "?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (resultado == DialogResult.Yes)
                {
                    Curso c = db.Curso.Find(idCurso);
                    db.Curso.Remove(c);
                    db.SaveChanges();
                    cargarCurso();
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
                if (idCurso == 0)
                {
                    if (txtLetra.Text.Trim() != "" && txtNivel.Text.Trim() != "")
                    {
                        if (verficarNivel(int.Parse(txtNivel.Text.Trim())))
                        {
                            if (verficarLetra(txtLetra.Text.Trim()))
                            {
                                MessageBox.Show("Ya se ha registrado este curso. Intente otro nivel o letra");
                                txtNivel.Text = "";
                                txtLetra.Text = "";
                                cbProfe.SelectedIndex = -1;
                            }
                        }
                    }
                    if(cbProfe.Text.Trim() != "")
                    {
                        if (verficarProfesor(cbProfe.Text))
                        {
                            MessageBox.Show("Este profesor ya tiene asignado un curso");
                            cbProfe.SelectedIndex = -1;
                        }
                    }
                    Guardar();
                    MessageBox.Show("El curso se ha guardado con éxito!");
                    Limpiar();
                    
                }
                else
                {
                    var resultado = MessageBox.Show("¿Desea modificar el curso: " + txtNivel.Text + " " + txtLetra.Text + "?", "Modificar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {
                        Modificar();
                    }
                }
                cargarCurso();
                
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void dgvCursos_MouseClick(object sender, MouseEventArgs e)
        {
            idCurso = int.Parse(dgvCursos.CurrentRow.Cells[0].Value.ToString());
            cbProfe.SelectedValue = int.Parse(dgvCursos.CurrentRow.Cells[1].Value.ToString());
            txtNivel.Text = dgvCursos.CurrentRow.Cells[2].Value.ToString();
            txtLetra.Text = dgvCursos.CurrentRow.Cells[3].Value.ToString();

            btnEliminar.Enabled = true;
        }

        private void txtNivel_KeyPress(object sender, KeyPressEventArgs e)
        {
            h.soloNumeros(e);
        }

        private void lblAtras_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form = new Form1();
            form.Show();
        }

        private bool verficarLetra(string letra)
        {
            bool result = false;
            Curso curso = db.Curso.FirstOrDefault(c => c.letra.Equals(letra) && c.id_curso != idCurso);
            if (curso != null)
            {
                result = true;
            }
            return result;
        }

        private bool verficarNivel(int nivel)
        {
            bool result = false;
            Curso curso = db.Curso.FirstOrDefault(c => c.nivel.Equals(nivel) && c.id_curso != idCurso);
            if (curso != null)
            {
                result = true;
            }
            return result;
        }

        private bool verficarProfesor(string id)
        {
            bool result = false;
            Curso curso = db.Curso.FirstOrDefault(c => (c.Prof_Jefe.rut + " - " +
            c.Prof_Jefe.nombres + " " + c.Prof_Jefe.apellidos).Equals(id) && c.id_curso != idCurso);
            if (curso != null)
            {
                result = true;
            }
            return result;
        }

        private void txtLetra_KeyPress(object sender, KeyPressEventArgs e)
        {
            h.soloMayus(e);
        }
    }
}
