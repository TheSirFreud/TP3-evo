using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Resources;
using System.Reflection;

namespace JeuxDuPendu
{
    public partial class APropos : Form
    {

        public APropos()
        {
            InitializeComponent();
            Translation();
            
        }

        private void APropos_Load(object sender, EventArgs e)
        {

        }

        private void Translation()
        {
            Assembly assembly = Assembly.Load("JeuxDuPendu");
            ResourceManager rm = new ResourceManager("JeuxDuPendu.Langues.langres", assembly);

            this.Text = rm.GetString("fenetrePropos", JeuxPendu.ci);
            label2.Text = rm.GetString("titrePropos", JeuxPendu.ci);
            label1.Text = rm.GetString("leAPropos", JeuxPendu.ci);
        }
    }
}
