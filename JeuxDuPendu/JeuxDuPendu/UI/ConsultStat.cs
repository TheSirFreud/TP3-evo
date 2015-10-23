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
    public partial class ConsultStat : Form
    {
        private Statistique statistique;
        public ConsultStat(int noJoueur)
        {
            statistique = Utilitaire.getSats(noJoueur);
            InitializeComponent();
        }

        private void ConsultStat_Load(object sender, EventArgs e)
        {
            lblGagne.Text = statistique.NbPartieGagne.ToString();
            lblPerdu.Text = statistique.NbPartiePerdu.ToString();
            lblPourcentage.Text = statistique.NbPartiePerdu == 0 ? "100,00%" : (statistique.NbPartieGagne / statistique.NbPartiePerdu).ToString("P2");
            lblScore.Text = statistique.Score.ToString();
        }
    }
}
