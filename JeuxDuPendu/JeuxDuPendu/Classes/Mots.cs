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
        private string mot;
        public string[] motsDejaEssayes;
        public string motATrouver;
        public char[] motCourant;
        public string[] dico;
        public string[] motsEssayes;

        public String Mot
        {
            get { return mot; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Le mot ne doit pas être null");
                }
                mot = value;
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
            InitialiserMotsATrouver();
            InitialiserMotCourant();
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
            do
            {
                motATrouver = dico[new Random().Next(0, dico.Length)];
            }
            while (MotDejaApparu());
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
        /// <summary>
        /// Initialisation du mot courant avec des caractères '_'
        /// </summary>
        /// <remarks>La taille du mot courant doit être égale à la taille du mot à trouver</remarks>
        public void InitialiserMotCourant()
        {
            motCourant = new char[motATrouver.Length];
            for (int i = 0; i < motCourant.Length; i++) { motCourant[i] = '-'; }
        }
        /// <summary>
        /// Conversion d'un tableau de caractères en une chaine de caractères
        /// </summary>
        public string CharsToString()
        {
            string chaine = "";
            for (int i = 0; i < motCourant.Length; i++) { chaine = chaine + motCourant[i]; }
            return chaine;
        }

        public bool MotDejaApparu()
        {
            Array.Sort(motsDejaEssayes);
            // Recherche dichotomique du mot dans le tableau motsDejaEssayes
            bool motTrouve = false;
            int debut = 0;
            int fin = motsDejaEssayes.Length - 1;
            int milieu;
            if (motsDejaEssayes.Length != 0)
            {
                do
                {
                    milieu = (debut + fin) / 2;
                    if (motsDejaEssayes[milieu] == mot)
                    {
                        motTrouve = true;
                    }
                    else
                    {
                        if (String.Compare(mot, motsDejaEssayes[milieu]) > 0)
                            debut = milieu + 1;
                        else fin = milieu - 1;
                    }
                }
                while (!motTrouve && debut <= fin);
            }
            if (motTrouve) return true;
            else return false;
        }
    }
}
