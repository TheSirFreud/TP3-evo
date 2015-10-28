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
    public partial class dialogJoueur : Form
    {
        List<Joueur> list;
        BindingSource bindUtil;
        public static CultureInfo ci;

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
                int noJoueur = Utilitaire.putJoueur(cboUtil.Text);
                Joueur joueur = new Joueur(noJoueur, cboUtil.Text);
                frmPrincipal = new JeuxPendu(niveauDiff, joueur);
            }
            frmPrincipal.Owner = this;
            frmPrincipal.Show();
            //Hide();
        }

        private void dialogJoueur_Load(object sender, EventArgs e)
        {
            list = Utilitaire.getUtils();
            bindUtil = new BindingSource(list, null);
            cboUtil.DataSource = bindUtil;
        }

        
        public void Translation()
        {
            Assembly assembly = Assembly.Load("JeuxDuPendu");
            ResourceManager rm = new ResourceManager("JeuxDuPendu.Langues.langres", assembly);


            radFacile.Text = rm.GetString("radFacile", ci);
            radMoyen.Text = rm.GetString("radMoyen", ci);
            radDifficile.Text = rm.GetString("radDifficile", ci);
            groupBox1.Text = rm.GetString("gbxDifficulté", ci);
            btnJouer.Text = rm.GetString("jouer", ci);
            btnQuitter.Text = rm.GetString("btnQuitter2", ci);


        }
    }
}
