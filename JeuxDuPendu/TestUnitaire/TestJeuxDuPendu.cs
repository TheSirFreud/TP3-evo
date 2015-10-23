using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JeuxDuPendu;
using System.Windows.Forms;


namespace TestUnitaire
{
    [TestClass]
    public class TestJeuxDuPendu
    {
        // Initialisation du test unitaire
        Mots unMot = null;
        Mots deuxiemeMot = null;
        JeuxPendu frmMain = null;
        String[] listeMots = null;
        [TestInitialize]
        public void initialize()
        {
            listeMots = new String[5] { "test", "batman", "robin", "joker", "catWoman" };
            unMot = new Mots("test", listeMots);
            deuxiemeMot = new Mots("nightWing", listeMots);
           
        }

        /// <summary>
        /// Permet de tester le constructeur de la classe Mots
        /// </summary>
        [TestMethod]
        public void TestConstructeur()
        {
            Assert.IsNotNull(unMot);
            Assert.AreEqual("test", unMot.Mot);
            Assert.AreEqual(5, unMot.MotsDejaEssayes.Length);
            for (int i = 0; i < listeMots.Length; i++)
            {
                Assert.AreEqual(listeMots[i], unMot.MotsDejaEssayes[i]);
            }
        }

        /// <summary>
        /// Permet de teste si le Mot est construit avec un null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructeurExceptionMot()
        {
            Mots testMot = new Mots(null, listeMots);
        }
        /// <summary>
        /// Permet de teste si la liste de mots est null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructeurExceptionListeMots()
        {
            Mots testMot = new Mots("test", null);
        }

        /// <summary>
        /// Permet de tester la méthode MotDejaApparu
        /// </summary>
        [TestMethod]
        public void TestMotDejaApparu()
        {
            Assert.IsTrue(unMot.MotDejaApparu());
            Assert.IsFalse(deuxiemeMot.MotDejaApparu());
        }

        /// <summary>
        /// Permet de tester l'initialisation du formulaire
        /// </summary>
        [TestMethod]
        public void TestIntialisation()
        {
            Assert.AreEqual(0, frmMain.getScore);
            Assert.AreEqual(frmMain.getMotATrouver.Length + 5, frmMain.getMaxTours);
            Assert.AreEqual(frmMain.getMotCourant.Length, frmMain.getMotATrouver.Length);
            Assert.AreEqual(frmMain.getLabel1.Text, frmMain.CharsToString(frmMain.getMotCourant));
          
            Assert.AreEqual(frmMain.getScoreLabel.Text, frmMain.getScore.ToString());
        }

        /// <summary>
        /// Permet de tester la méthode qui initialise le dictionnaire
        /// </summary>
        [TestMethod]
        public void TestInitialiserDico()
        {
            Assert.AreNotEqual(frmMain.getDico, null);
            Assert.IsTrue(frmMain.getDico.Length > 0);
        }

        /// <summary>
        /// Permet de tester la méthode qui initialise le tableau des mots déjà essayés
        /// </summary>
        [TestMethod]
        public void TestInitialiserMotsEssayes()
        {
            frmMain.InitialiserMotsEssayes();
            Assert.AreNotEqual(frmMain.getmotsEssayes, null);
        }

        /// <summary>
        /// Permet de tester la méthode qui initialise le mot courant avec des '_' à la place des lettres
        /// </summary>
        [TestMethod]
        public void TestInitialiserMotCourant()
        {
            for (int i = 0; i < frmMain.getMotCourant.Length; i++)
            {
                Assert.AreEqual(frmMain.getMotCourant[i], '-');
            }            
        }

        /// <summary>
        /// Permet de tester la méthode qui convertit un tableau de caractères en une chaine de caractères
        /// </summary>
        [TestMethod]
        public void TestCharsToString()
        {
            char[] tabChar = { 't', 'e', 's', 't' };
            string test = "test";
            string motATester = "";

            motATester = frmMain.CharsToString(tabChar);
            Assert.AreEqual(test, motATester);

            tabChar = new char[]  { 'V', 'o', 'i', 't', 'u', 'r', 'e' };
            test = "Voiture";
            motATester = "";

            motATester = frmMain.CharsToString(tabChar);
            Assert.AreEqual(test, motATester);

            tabChar = new char[] {};
            test = "";
            motATester = "";

            motATester = frmMain.CharsToString(tabChar);
            Assert.AreEqual(test, motATester);
        }

        /// <summary>
        /// Permet de tester la méthode qui ajoute un mot dans le tableau des mots déjà essayés
        /// </summary>
        [TestMethod]
        public void TestAjouterMot()
        {
            string mot = "BATMAN";
            frmMain.AjouterMot(mot.ToUpper());
            frmMain.reinitialise();
            bool exist = false;
            for (int i = 0; i < frmMain.getmotsEssayes.Length; i++)
            {
                if (frmMain.getmotsEssayes[i].ToLower() == mot.ToLower())
                {
                    exist = true;
                }
            }
            Assert.IsTrue(exist);
        }

        /// <summary>
        /// Permet de tester la méthode qui active les boutons
        /// </summary>
        [TestMethod]
        public void TestActivationBoutton()
        {
            // Test de la méthode pour l'activation des boutons
            frmMain.activationBoutton(true);

            foreach (Control controle in frmMain.getListBoutons)
            {
                if (controle.GetType() == typeof(Button))
                {
                    Assert.AreEqual(true, controle.Enabled);
                }
            }

            // Test de la méthode pour la désactivation des boutons
            // Les boutons quitter et nouvelle parties sont toujours activés
            frmMain.activationBoutton(false);

            foreach (Control controle in frmMain.getListBoutons)
            {
                if (controle.GetType() == typeof(Button))
                {
                    if (controle.Name == "btnNouvellePartie" || controle.Name == "btnQuitter")
                    {
                        Assert.AreEqual(true, controle.Enabled);
                    }
                    else
                    {
                        Assert.AreEqual(false, controle.Enabled);
                    }
                }
            }
        }

        /// <summary>
        /// Permet de tester la méthode qui réinitialise la partie
        /// </summary>
        [TestMethod]
        public void TestReinitialise()
        {
            frmMain.reinitialise();
            Assert.AreEqual(frmMain.getMotATrouver.Length + 5, frmMain.getMaxTours);
            Assert.AreEqual(frmMain.getMotCourant.Length, frmMain.getMotATrouver.Length);
            Assert.AreEqual(frmMain.getLabel1.Text, frmMain.CharsToString(frmMain.getMotCourant));
           
        }

        /// <summary>
        /// Permet de tester la méthode qui met à jour les boutons
        /// </summary>
        [TestMethod]
        public void TestMise_a_jour()
        {
            frmMain.getMotATrouver = "B";
            frmMain.getMotCourant = new char[] { '-' };
            frmMain.MAJJeu(frmMain.getBoutonA.Text[0]);
            Assert.AreEqual(0, frmMain.getScore);

            frmMain.getMotATrouver = "A";
            frmMain.getMotCourant = new char[] {'-'};
            frmMain.MAJJeu(frmMain.getBoutonA.Text[0]);
            Assert.AreEqual(1, frmMain.getScore);            
        }
    }
}
