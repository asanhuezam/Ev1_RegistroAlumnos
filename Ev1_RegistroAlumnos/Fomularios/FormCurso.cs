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
        private registroColegioEntities db = new registroColegioEntities();
        int idCurso = 0;
        private Helpers h = new Helpers();

        public FormCurso()
        {
            InitializeComponent();
            cargarProf();
            cargarCurso();
        }

        private void cargarProf()
        {
            var listaProf = (from p in db.Prof_Jefe
                             select new
                             {
                                 id = p.id_prof,
                                 Nombres = p.nombres,
                                 Apellidos = p.apellidos,
                                 Email = p.email,
                                 Telefono = p.telefono
                             }).ToList();
            cbProfe.DataSource = listaProf;
            cbProfe.ValueMember = "id";           
            cbProfe.DisplayMember = "Nombres";
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
                                 Profesor = c.Prof_Jefe.nombres,
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
                mensaje = "Ingrese al menos un Nivel \n";
            if (string.IsNullOrEmpty(txtLetra.Text.Trim()))
                mensaje += "Ingrese al menos una Letra \n";
            if (string.IsNullOrEmpty(cbProfe.Text.Trim()))
                mensaje += "Debe seleccionar un Profesor \n";
            return mensaje;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idCurso > 0)
            {
                var resultado = MessageBox.Show("¿Desea eliminar el curso: " + txtNivel.Text + " " + txtLetra.Text + "?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
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
                    Guardar();
                }
                else
                {
                    var resultado = MessageBox.Show("¿Desea modificar el curso: " + txtNivel.Text + " " + txtLetra.Text + "?", "Modificar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {
                        Modificar();
                    }
                }
                MessageBox.Show("El registro se ha guardado con éxito!");
                cargarCurso();
                Limpiar();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void dgvCursos_MouseClick(object sender, MouseEventArgs e)
        {
            idCurso = int.Parse(dgvCursos.CurrentRow.Cells[0].Value.ToString());
            txtNivel.Text = dgvCursos.CurrentRow.Cells[1].Value.ToString();
            txtLetra.Text = dgvCursos.CurrentRow.Cells[2].Value.ToString();
            cbProfe.SelectedValue = dgvCursos.CurrentRow.Cells[3].Value.ToString();

            btnEliminar.Enabled = true;
        }

        private void txtNivel_KeyPress(object sender, KeyPressEventArgs e)
        {
            h.soloNumeros(e);
        }
    }
}
