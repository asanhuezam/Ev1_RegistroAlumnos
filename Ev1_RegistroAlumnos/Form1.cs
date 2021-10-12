using Ev1_RegistroAlumnos.Fomularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ev1_RegistroAlumnos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void profJefeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            FormProfJefe prof = new FormProfJefe();
            prof.Show();
        }

        private void cursoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            FormCurso curso = new FormCurso();
            curso.Show();
        }

        private void alumnoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAlumno alumno = new FormAlumno();
            alumno.Show();
        }
    }
}
