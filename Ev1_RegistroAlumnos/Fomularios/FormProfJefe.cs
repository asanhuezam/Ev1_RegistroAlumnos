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
        private registroColegioEntities db = new registroColegioEntities();

        int idProf = 0;

        private Helpers h = new Helpers();

        public FormProfJefe()
        {
            InitializeComponent();
            cargarProf();
        }

        private void cargarProf()
        {
            var listaProf = (from p in db.Prof_Jefe
                                  select new
                                  {
                                      p.id_prof,
                                      Nombres = p.nombres,
                                      Apellidos = p.apellidos,
                                      Email = p.email,
                                      Telefono = p.telefono
                                  }).ToList();
            dgvProfesores.DataSource = listaProf;
            dgvProfesores.Columns[0].Visible = false;
        }

        private void Guardar()
        {
            Prof_Jefe p = new Prof_Jefe();
            p.nombres = txtNombres.Text.Trim();
            p.apellidos = txtApellidos.Text.Trim();
            p.email = txtEmail.Text.Trim();
            
            p.telefono = int.Parse(txtTelefono.Text);

            db.Prof_Jefe.Add(p);
            db.SaveChanges();
        }

        private void Modificar()
        {
            Prof_Jefe p = db.Prof_Jefe.Find(idProf);
            p.nombres = txtNombres.Text.Trim();
            p.apellidos = txtApellidos.Text.Trim();
            p.email = txtEmail.Text.Trim();
            p.telefono = int.Parse(txtTelefono.Text);

            db.SaveChanges();

        }

        private void Limpiar()
        {
            idProf = 0;
            txtNombres.Text = "";
            txtApellidos.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            dgvProfesores.ClearSelection();
            btnEliminar.Enabled = false;
        }

        private string Validar()
        {
            string mensaje = "";
            if (string.IsNullOrEmpty(txtNombres.Text.Trim()))
                mensaje = "Ingrese al menos un Nombre \n";
            if (string.IsNullOrEmpty(txtApellidos.Text.Trim()))
                mensaje += "Ingrese al menos un Apellido \n";
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
                mensaje += "Debe ingresar un Email \n";
            if (string.IsNullOrEmpty(txtTelefono.Text.Trim()))
                mensaje += "Debe ingresar un Telefono \n";
            return mensaje;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idProf > 0)
            {
                var resultado = MessageBox.Show("¿Desea eliminar al profesor " + txtNombres.Text + " " + txtApellidos.Text + " ?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
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
                    Guardar();
                }
                else
                {
                    Modificar();
                }
                MessageBox.Show("EL registro se ha guardado con éxito");
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
            txtNombres.Text = dgvProfesores.CurrentRow.Cells[1].Value.ToString();
            txtApellidos.Text = dgvProfesores.CurrentRow.Cells[2].Value.ToString();
            txtEmail.Text = dgvProfesores.CurrentRow.Cells[3].Value.ToString();
            txtTelefono.Text = dgvProfesores.CurrentRow.Cells[4].Value.ToString();

            btnEliminar.Enabled = true;
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            h.soloNumeros(e);
        }
    }
}
