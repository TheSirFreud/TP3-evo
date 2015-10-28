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
    public partial class ConsultStat : Form
    {
        private Statistique statistique;
        public ConsultStat(int noJoueur)
        {
            statistique = Utilitaire.getSats(noJoueur);
            InitializeComponent();
            Translation();
        }

        private void ConsultStat_Load(object sender, EventArgs e)
        {
            lblGagne.Text = statistique.NbPartieGagne.ToString();
            lblPerdu.Text = statistique.NbPartiePerdu.ToString();
            lblPourcentage.Text = statistique.NbPartiePerdu == 0 ? "100,00%" : ((double)statistique.NbPartieGagne / ((double)statistique.NbPartiePerdu + (double)statistique.NbPartieGagne)).ToString("P2");
            lblScore.Text = statistique.Score.ToString();
            Dictionary<String, Statistique> dicoTop = Utilitaire.getTop3();
            for (int i = 0; i < dicoTop.Count; i++)
            {
                Label lab = (Label)Controls["lbl" + (i + 1).ToString()];
                Label labScore = (Label)Controls["lblScore" + (i + 1).ToString()];
                lab.Text = dicoTop.Keys.ElementAt(i);
                labScore.Text = ((Statistique)dicoTop.Values.ElementAt(i)).Score.ToString();
            }

        }

        private void Translation()
        {
            Assembly assembly = Assembly.Load("JeuxDuPendu");
            ResourceManager rm = new ResourceManager("JeuxDuPendu.Langues.langres", assembly);

            this.Text = rm.GetString("statistiqueToolStripMenuItem", JeuxPendu.ci);
            label1.Text = rm.GetString("statsPartiesG", JeuxPendu.ci);
            label2.Text = rm.GetString("statsPartiesP", JeuxPendu.ci);
            label3.Text = rm.GetString("statsPourcent", JeuxPendu.ci);
        }
    }
}
