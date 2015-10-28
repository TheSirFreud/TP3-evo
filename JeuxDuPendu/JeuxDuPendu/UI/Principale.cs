using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using System.Resources;
using System.Reflection;

namespace JeuxDuPendu
{
    public partial class JeuxPendu : Form
    {
        private int score;
        private int maxTours;
        private bool nouvellePartie;
        private bool enJeu;
        private Mots mot;
        private Dictionary<String, SoundPlayer> soundSample;
        private Joueur joueur;
        private NiveauDiff difficulte;
        private int tempsReflexion;
        private int pointDepart;
        private Langues langue;
        private const int fin = 9;
        private int nbPartieJoue;
        private bool partieEnLigne;
        public static CultureInfo ci;
        GestionnaireClientTCP leClient;

        // Initialisations
        public JeuxPendu(NiveauDiff niveauDiff, Joueur joueurCourrant)
        {
            InitializeComponent();
            joueur = joueurCourrant;
            difficulte = niveauDiff;
            langue = Langues.Fraçais;
            nbPartieJoue = 0;
            enJeu = false;
            partieEnLigne = false;
        }
        private void JeuxPendu_Load(object sender, EventArgs e)
        {
            nouvellePartie = true;
            mot = new Mots(langue);
            activationBoutton(false);
            chrono.Stop();
            soundSample = new Dictionary<String, SoundPlayer>();
            soundSample.Add("ok", new SoundPlayer(Properties.Resources.ff7move));
            soundSample.Add("no", new SoundPlayer(Properties.Resources.erreur));
            soundSample.Add("perdu", new SoundPlayer(Properties.Resources.perdu));
            soundSample.Add("gagne", new SoundPlayer(Properties.Resources.gagne));
            soundSample["ok"].Load();
            soundSample["no"].Load();
            soundSample["perdu"].Load();
            soundSample["gagne"].Load();
            lblNom.Text = joueur.Nom;
            lblNiveau.Text = Utilitaire.GetDescription(difficulte);
            pbTemps.Maximum = tempsReflexion;
            lblCountDown.Text = pbTemps.Maximum.ToString();
            ChangerDifficulte(difficulte);
        }

        //Getter et setter

        public Mots Mots { get; set; }

        //Permet de changer la dificulté
        public void ChangerDifficulte(NiveauDiff niveauDiff)
        {
            switch (niveauDiff)
            {
                case NiveauDiff.Facile:
                    tempsReflexion = 40;
                    pointDepart = 1;
                    break;
                case NiveauDiff.Moyen:
                    tempsReflexion = 30;
                    pointDepart = 2;
                    break;
                case NiveauDiff.Difficile:
                    tempsReflexion = 20;
                    pointDepart = 4;
                    break;
            }
            difficulte = niveauDiff;
            pbTemps.Maximum = tempsReflexion;
            lblNiveau.Text = Utilitaire.GetDescription(niveauDiff);
            maxTours += pointDepart;
            AlignerLabelNom();
        }
        //Aligne les labels dans le haut relative à leur longeur
        public void AlignerLabelNom()
        {
            lblNom.Location = new Point((lblMotCourrant.Location.X + lblMotCourrant.Size.Width) - lblNom.Size.Width, lblNom.Location.Y);
            lblTNom.Location = new Point(lblNom.Location.X - lblTNom.Size.Width - 5, lblTNom.Location.Y);
            lblNiveau.Location = new Point(lblTNom.Location.X - 10 - lblNiveau.Size.Width, lblNiveau.Location.Y);
            lblTNiveau.Location = new Point(lblNiveau.Location.X - lblTNiveau.Size.Width - 5, lblTNiveau.Location.Y);
        }

        //Permet d'activer et de désactiver tous les boutons
        public void activationBoutton(bool activate)
        {
            foreach (Control controle in Controls)
            {
                if (controle.GetType() == typeof(Button))
                {
                    ((Button)controle).Enabled = activate;
                }
            }
            btnNouvellePartie.Enabled = true;
            btnQuitter.Enabled = true;
        }


        //Met à jour le jeu sans lettre (temps de reflexion dépassé) et regarde si le jeu est terminer
        public void MAJJeu()
        {
            if (nbPartieJoue != 10)
            {
                if (maxTours == fin) { Perdu(); }
                else
                { if (mot.motATrouver == mot.motCourant) { Gagne(); } }
            }
            else
            {
                activationBoutton(false);
                EtatNeutre();
                nouvellePartie = true;
                nbPartieJoue = 0;
                ConsultStat frmStat = new ConsultStat(joueur.NoJoueur);
                frmStat.ShowDialog();
            }
        }
        //Met à jour avec une lettre. Met a jour le mot courrant et appelle MAJJeu
        public void MAJJeu(char lettre)
        {
            if (maxTours < fin && mot.motATrouver != mot.motCourant)
            {
                desactivBouton(lettre);
                if (!mot.VerifierLettre(lettre)) { Erreur(); }
                else
                {
                    soundSample["ok"].Play();
                    this.lblMotCourrant.Text = mot.motCourant;
                }
            }
            MAJJeu();
            pbTemps.Value = 0;
            lblCountDown.Text = (pbTemps.Maximum - pbTemps.Value).ToString();
        }

        //Survient lors d'un erreur
        public void Erreur()
        {
            maxTours += 1;
            MAJImagePendu(maxTours, pboPendu);
            soundSample["no"].Play();
        }
        //Lorsque perdu
        public void Perdu()
        {
            if (partieEnLigne) { this.lblMotCourrant.Text = "PAS ASSEZ RAPIDE! PERDU"; }
            this.lblTSolution.Visible = true;
            this.lblSolution.Text = mot.motATrouver;
            this.lblSolution.Show();
            chrono.Stop();
            soundSample["perdu"].Play();
            nbPartieJoue++;
            enJeu = false;
            activationBoutton(false);
            mot.AjouterMot();
            Utilitaire.updateSats(joueur.NoJoueur, false, difficulte);
        }
        //Lorsque gagné
        public void Gagne()
        {
            if (partieEnLigne) { leClient.EnvoyerGagne(); }
            enJeu = false;
            this.lblSolution.Text = "BRAVO! VOUS AVEZ TROUVÉ LE MOT.";
            chrono.Stop();
            lblTSolution.Visible = true;
            this.lblSolution.Show();
            this.lblScore.Text = score.ToString();
            activationBoutton(false);
            soundSample["gagne"].Play();
            nbPartieJoue++;
            mot.AjouterMot();
            Utilitaire.updateSats(joueur.NoJoueur, true, difficulte);
        }
        //Met a jour l'image selon le nombre max de tour
        public void MAJImagePendu(int maxTour, PictureBox pbo)
        {
            pbo.Image = Utilitaire.Redimention((Image)Properties.Resources.ResourceManager.GetObject("_" + maxTour.ToString()), pbo.Width, pbo.Height);
        }
        //Permet de désactiver le bouton de la lettre entré
        public void desactivBouton(char lettre)
        {
            foreach (Control item in Controls)
            {
                if (item.GetType() == typeof(Button) && ((Button)item).Text.Length == 1 && ((Button)item).Text[0] == lettre) { item.Enabled = false; }
            }
        }

        //Méthode gérant une nouvelle partie en ligne(Permet l'accesibilité à partir de l'extérieur)
        public void NouvellePartieEnLigne(string motATrouver)
        {
            mot.InitialiserMotsATrouver(motATrouver);
            partieEnLigne = true;
            button29_Click(this, null);
        }

        //État d'une nouvelle partie (tout activé, seul le score reste inchangé)
        public void EtatInit()
        {
            enJeu = true;
            pbTemps.Value = 0;
            lblCountDown.Text = pbTemps.Maximum.ToString();
            maxTours = pointDepart;
            lblSolution.Hide();
            chrono.Start();

            //Comme le mot est déjà initialisé s'il y a une partie en ligne
            if (!partieEnLigne)
                mot.InitialiserMotsATrouver();

            lblMotCourrant.Text = mot.motCourant;
            MAJImagePendu(maxTours, pboPendu);
            activationBoutton(true);
        }
        //Etat semblable au load (tout désactivé)
        public void EtatNeutre()
        {
            nouvellePartie = true;
            enJeu = false;
            chrono.Stop();
            pboPendu.Image = null;
            activationBoutton(false);
            lblCountDown.Text = pbTemps.Maximum.ToString();
            pbTemps.Value = 0;
            lblMotCourrant.Text = "";
            lblScore.Text = "";
        }


        // Gestion des clics sur les boutons
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //Permet de quitter
        private void button10_Click(object sender, EventArgs e)
        { Application.Exit(); }
        //Permet de faire une nouvelle partie
        private void button29_Click(object sender, EventArgs e)
        {
            if (nouvellePartie)
            {
                try
                {
                    enJeu = true;
                    score = 0;
                    maxTours = pointDepart;
                    chrono.Start();
                    EtatInit();
                    this.lblScore.Text = score.ToString();
                    nouvellePartie = false;
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Terminer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else { EtatInit(); }
        }

        //Evenement qui se déclanche lorsqu'une lettre est enfoncer au clavier ou sur l'interface
        private void button1_Click(object sender, EventArgs e)
        { MAJJeu(((Button)sender).Text[0]); }
        //Permet de géré les touche du clavier
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (enJeu)
            {
                if (e.KeyValue >= 65 && e.KeyValue <= 90)
                {
                    char lettre = Convert.ToChar(e.KeyValue);
                    MAJJeu(lettre);
                }
            }
        }

        //permet de changer le niveau de difficulté
        private void facileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangerDiff frmChangerDiff = new ChangerDiff(difficulte);
            if (DialogResult.OK == frmChangerDiff.ShowDialog())
            {
                if (frmChangerDiff.NiveauDiff != difficulte)
                {
                    ChangerDifficulte(frmChangerDiff.NiveauDiff);
                    EtatNeutre();
                }
            }
            frmChangerDiff.Dispose();
        }
        //Ferme l'application à la fermeture du formulaire
        private void JeuxPendu_FormClosing(object sender, FormClosingEventArgs e)
        { Application.Exit(); }
        //Se produit à chaque seconde et gere le temps de réflexion
        private void chrono_Tick(object sender, EventArgs e)
        {
            pbTemps.PerformStep();
            lblCountDown.Text = (pbTemps.Maximum - pbTemps.Value).ToString();
            if (pbTemps.Value == pbTemps.Maximum)
            {
                Erreur();
                MAJJeu();
                pbTemps.Value = 0;
            }
        }


        //Methodes du menuStrip
        private void voirInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            APropos frmAPropos = new APropos();
            frmAPropos.ShowDialog();
        }

        private void consulterStatistiqueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConsultStat frmStat = new ConsultStat(joueur.NoJoueur);
            frmStat.ShowDialog();
        }

        private void bgChangDico_DoWork(object sender, DoWorkEventArgs e)
        {
            mot.InitialiserDico(langue);
        }

        private void bgChangDico_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (langue)
            {
                case Langues.Fraçais:
                    MessageBox.Show("Changement de langue fait avec succès");
                    break;
                case Langues.Anglais:
                    MessageBox.Show("Language changed with succes");
                    break;
            }

        }

        private void button10_Click_1(object sender, EventArgs e)
        {

        }

        private void démarrerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Tentative de connexion au serveur
            leClient =
                new GestionnaireClientTCP("127.0.0.1", 1330, this, langue);
            if (!leClient.Connexion())
                MessageBox.Show("Erreur : le serveur n'a pu être trouvé");
            else
                leClient.execBouclePrincipale();
        }


        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            switch (((ToolStripMenuItem)sender).Text)
            {
                case "Francais":
                    ci = new CultureInfo("fr-FR");
                langue = Langues.Fraçais;
                    break;
                case "English":
                     ci = new CultureInfo("en-CA");
                langue = Langues.Anglais;
                    break;
            }           
            Translation();
            bgChangDico.RunWorkerAsync();
            EtatNeutre();
        }
        private void Translation()
        {
            Assembly assembly = Assembly.Load("JeuxDuPendu");
            ResourceManager rm = new ResourceManager("JeuxDuPendu.Langues.langres", assembly);


            jeuEnRéseauToolStripMenuItem.Text = rm.GetString("jeuEnRéseauToolStripMenuItem", ci);
            démarrerToolStripMenuItem.Text = rm.GetString("démarrerToolStripMenuItem", ci);
            changerDifficultéToolStripMenuItem.Text = rm.GetString("facileToolStripMenuItem", ci);
            optionsToolStripMenuItem.Text = rm.GetString("optionsToolStripMenuItem", ci);
            changerDutilisateurToolStripMenuItem.Text = rm.GetString("changerDutilisateurToolStripMenuItem", ci);
            multijoueurToolStripMenuItem.Text = rm.GetString("multijoueurToolStripMenuItem", ci);
            àProposToolStripMenuItem.Text = rm.GetString("àProposToolStripMenuItem", ci);
            voirInformationToolStripMenuItem.Text = rm.GetString("voirInformationToolStripMenuItem", ci);
            règlesToolStripMenuItem.Text = rm.GetString("règlesToolStripMenuItem", ci);
            voirRèglesToolStripMenuItem.Text = rm.GetString("voirRèglesToolStripMenuItem", ci);
            statistiqueToolStripMenuItem.Text = rm.GetString("statistiqueToolStripMenuItem", ci);
            consulterStatistiqueToolStripMenuItem.Text = rm.GetString("consulterStatistiqueToolStripMenuItem", ci);
            langueToolStripMenuItem1.Text = rm.GetString("langueToolStripMenuItem", ci);
            lblTNiveau.Text = rm.GetString("lblTNiveau", ci);
            lblTNom.Text = rm.GetString("lblTNom", ci);
            lblTempsReflexion.Text = rm.GetString("lblTempsReflexion", ci);
            btnNouvellePartie.Text = rm.GetString("btnNouvellePartie", ci);
            btnQuitter.Text = rm.GetString("btnQuitter", ci);


        }



    }
}
