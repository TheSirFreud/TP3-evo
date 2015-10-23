using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeuxDuPendu
{
    public class Mots
    {
        //TODO: Intégrer les methodes du prof et des varialbes dans a classe mot       
        public string[] motsDejaEssayes;
        public string motATrouver;
        public String motCourant;
        public string[] dico;


        public String Mot
        {
            get { return motATrouver; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Le mot ne doit pas être null");
                }
                motATrouver = value;
            }
        }
        public String[] MotsDejaEssayes
        {
            get { return motsDejaEssayes; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("La liste de mots ne doit pas être null");
                }
                motsDejaEssayes = value;
            }
        }

        public Mots(Langues langue)
        {
            InitialiserMotsEssayes();
            InitialiserDico(langue);
        }

        /// <summary>
        /// Initialisation du dictionnaire
        /// </summary>
        /// <remarks>La taille déclarée du tableau Dico doit correspondre au nombre de mots du fichier dictionnaire.text</remarks>
        public void InitialiserDico(Langues langue)
        {
            String chemin = "";

            switch (langue)
            {
                case Langues.Fraçais:
                    chemin = @"..\..\..\dictionnaireFR.txt";
                    break;
                case Langues.Anglais:
                    chemin = @"..\..\..\dictionnaireEN.txt";
                    break;
            }
            string[] lines = System.IO.File.ReadAllLines(chemin);
            dico = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                dico[i] = lines[i].ToUpper();
            }
        }
        /// <summary>
        /// Permet de charger les mots déjà essayés
        /// </summary>
        public void InitialiserMotsEssayes()
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\motsEssayes.txt");
            motsDejaEssayes = new string[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                motsDejaEssayes[i] = lines[i].ToUpper();
            }
        }

        public void InitialiserMotsATrouver()
        {
            if (dico.All(p => motsDejaEssayes.Contains(p)))
            {
                throw new ArgumentException("Vous avez essayé(e) tous les mots!");
            }
            else
            {
                motCourant = "";
                do
                {
                    motATrouver = dico[new Random().Next(0, dico.Length)];
                }
                while (motsDejaEssayes.Contains(motATrouver.ToUpper()));
                motCourant = motCourant.PadRight(motATrouver.Length, '-');
            }
        }
        /// <summary>
        /// Ajout du mot à la liste des mots essayés
        /// </summary>
        public void AjouterMot()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"..\..\..\motsEssayes.txt", true))
            {
                file.WriteLine(motATrouver);
            }
        }

        public bool VerifierLettre(char lettre)
        {
            StringBuilder strBuild;
            bool lettreTrouvee = false;
            for (int i = 0; i < motATrouver.Length; i++)
            {
                if (motATrouver[i] == lettre)
                {
                    lettreTrouvee = true;
                    strBuild = new StringBuilder(motCourant);
                    strBuild[i] = lettre;
                    motCourant = strBuild.ToString();
                }
            }
            return lettreTrouvee;
        }

    }
}
