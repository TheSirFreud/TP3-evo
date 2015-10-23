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
    public partial class dialogJoueur : Form
    {
        List<Joueur> list;
        BindingSource bindUtil;

        public dialogJoueur()
        {
            InitializeComponent();
        }

        private void btnQuitter_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnJouer_Click(object sender, EventArgs e)
        {
            JeuxPendu frmPrincipal;
            NiveauDiff niveauDiff;
            if (radFacile.Checked)
            {
                niveauDiff = NiveauDiff.Facile;
            }
            else if (radMoyen.Checked)
            {
                niveauDiff = NiveauDiff.Moyen;
            }
            else
            {
                niveauDiff = NiveauDiff.Difficile;
            }
            if (cboUtil.SelectedItem != null)
            {
                frmPrincipal = new JeuxPendu(niveauDiff, (Joueur)cboUtil.SelectedItem);
            }
            else
            {
                int noJoueur = Utilitaire.putUtil(cboUtil.Text);
                Joueur joueur = new Joueur(noJoueur, cboUtil.Text);
                frmPrincipal = new JeuxPendu(niveauDiff, joueur);
            }          
            frmPrincipal.Show();
            Hide();
        }

        private void dialogJoueur_Load(object sender, EventArgs e)
        {
            list = Utilitaire.getUtils();
            bindUtil = new BindingSource(list, null);
            cboUtil.DataSource = bindUtil;
        }
    }
}
