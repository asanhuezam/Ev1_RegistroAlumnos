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
            this.Hide();
            FormProfJefe prof = new FormProfJefe();
            prof.Show();
        }
    }
}
