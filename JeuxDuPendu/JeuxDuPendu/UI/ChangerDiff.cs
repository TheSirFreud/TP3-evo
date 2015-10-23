using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeuxDuPendu
{
    public partial class ChangerDiff : Form
    {
        public NiveauDiff NiveauDiff { get; set; }
        public ChangerDiff(NiveauDiff niveauActuelle)
        {
            InitializeComponent();
            NiveauDiff = niveauActuelle;
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
    }
}
