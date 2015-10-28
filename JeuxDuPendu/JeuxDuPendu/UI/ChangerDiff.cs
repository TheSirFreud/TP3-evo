using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Globalization;
using System.Resources;

namespace JeuxDuPendu
{
    public partial class ChangerDiff : Form
    {
        public NiveauDiff NiveauDiff { get; set; }
        public ChangerDiff(NiveauDiff niveauActuelle)
        {
            InitializeComponent();
            NiveauDiff = niveauActuelle;
            Translation();
        }

        private void btnChanger_Click(object sender, EventArgs e)
        {
            if (radFacile.Checked)
            {
                NiveauDiff = NiveauDiff.Facile;
            }
            else if (radMoyen.Checked)
            {
                NiveauDiff = NiveauDiff.Moyen;
            }
            else
            {
                NiveauDiff = NiveauDiff.Difficile;
            }
            DialogResult = DialogResult.OK;
        }

        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ChangerDiff_Load(object sender, EventArgs e)
        {
            switch (NiveauDiff)
            {
                case JeuxDuPendu.NiveauDiff.Facile:
                    radFacile.Checked = true;
                    break;
                case JeuxDuPendu.NiveauDiff.Moyen:
                    radMoyen.Checked = true;
                    break;
                case JeuxDuPendu.NiveauDiff.Difficile:
                    radDifficile.Checked = true;
                    break;
            }
        }
        private void Translation()
        {
            Assembly assembly = Assembly.Load("JeuxDuPendu");
            ResourceManager rm = new ResourceManager("JeuxDuPendu.Langues.langres", assembly);

            this.Text = rm.GetString("ChangerDiff", JeuxPendu.ci);
            groupBox1.Text = rm.GetString("gbxDifficulté", JeuxPendu.ci);
            radFacile.Text = rm.GetString("radFacile", JeuxPendu.ci);
            radMoyen.Text = rm.GetString("radMoyen", JeuxPendu.ci);
            radDifficile.Text = rm.GetString("radDifficile", JeuxPendu.ci);
            btnChanger.Text = rm.GetString("btnChanger", JeuxPendu.ci);
            btnAnnuler.Text = rm.GetString("btnAnnuler", JeuxPendu.ci);
        }
    }
}
