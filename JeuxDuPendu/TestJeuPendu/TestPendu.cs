using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JeuxDuPendu;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TestJeuPendu
{
    [TestClass]
    public class TestPendu
    {
        Mots unMot;
        JeuxDuPendu.JeuxPendu frmPrinciplale;
        Joueur unJoueur;

        [TestInitialize]
        public void init()
        {
            unMot = new Mots(Langues.Fraçais);
            frmPrinciplale = new JeuxPendu(NiveauDiff.Moyen, new Joueur(1, "Batman"));
            unJoueur = Utilitaire.getUtils(1);
        }

        [TestMethod]
        public void ChangerDiff()
        {
            Assert.AreEqual(frmPrinciplale.difficulte, NiveauDiff.Moyen);
            frmPrinciplale.ChangerDifficulte(NiveauDiff.Difficile);
            Assert.AreEqual(frmPrinciplale.difficulte, NiveauDiff.Difficile);
        }

        [TestMethod]
        public void TempsReflexion()
        {
            Assert.AreEqual(frmPrinciplale.tempsReflexion, 30);
            frmPrinciplale.ChangerDifficulte(NiveauDiff.Difficile);
            Assert.AreEqual(frmPrinciplale.tempsReflexion, 20);
        }
        [TestMethod]
        public void ChangerDico()
        {
            string[] dicoFR = leDico(Langues.Fraçais);
            string[] dicoEn = leDico(Langues.Anglais);
            CollectionAssert.AreEqual(unMot.dico, dicoFR);
            unMot.InitialiserDico(Langues.Anglais);
            CollectionAssert.AreEqual(unMot.dico, dicoEn);
        }
        [TestMethod]
        public void ResetStats()
        {
            Utilitaire.ResetStats(unJoueur.NoJoueur);
            Utilitaire.updateSats(unJoueur.NoJoueur, true, NiveauDiff.Difficile);
            Statistique stats = Utilitaire.getSats(unJoueur.NoJoueur);
            Assert.AreNotEqual(stats.Score, 0);
            Utilitaire.ResetStats(unJoueur.NoJoueur);
            stats = Utilitaire.getSats(unJoueur.NoJoueur);
            Assert.AreEqual(stats.Score, 0);
        }
        [TestMethod]
        public void GagnerPartiStats()
        {
            Utilitaire.ResetStats(unJoueur.NoJoueur);
            Statistique stats = Utilitaire.getSats(unJoueur.NoJoueur);
            Assert.AreEqual(stats.Score, 0);
            Utilitaire.updateSats(unJoueur.NoJoueur, true, NiveauDiff.Difficile);
            stats = Utilitaire.getSats(unJoueur.NoJoueur);
            Assert.AreEqual(stats.Score, 3);
            Assert.AreEqual(stats.NbPartieGagne, 1);
        }
        [TestMethod]
        public void PerdrePartiStats()
        {
            Utilitaire.ResetStats(unJoueur.NoJoueur);
            Statistique stats = Utilitaire.getSats(unJoueur.NoJoueur);
            Assert.AreEqual(stats.Score, 0);
            Utilitaire.updateSats(unJoueur.NoJoueur, false, NiveauDiff.Difficile);
            stats = Utilitaire.getSats(unJoueur.NoJoueur);
            Assert.AreEqual(stats.Score, -3);
            Assert.AreEqual(stats.NbPartiePerdu, 1);
        }
        [TestMethod]
        public void NbPartieJouer()
        {
            Utilitaire.ResetStats(unJoueur.NoJoueur);
            Utilitaire.updateSats(unJoueur.NoJoueur, false, NiveauDiff.Facile);
            Utilitaire.updateSats(unJoueur.NoJoueur, false, NiveauDiff.Facile);
            Utilitaire.updateSats(unJoueur.NoJoueur, true, NiveauDiff.Facile);
            Utilitaire.updateSats(unJoueur.NoJoueur, true, NiveauDiff.Facile);
            Utilitaire.updateSats(unJoueur.NoJoueur, true, NiveauDiff.Facile);
            Utilitaire.updateSats(unJoueur.NoJoueur, false, NiveauDiff.Facile);
            Utilitaire.updateSats(unJoueur.NoJoueur, false, NiveauDiff.Facile);
            Statistique stats = Utilitaire.getSats(unJoueur.NoJoueur);
            Assert.AreEqual(stats.NbPartieGagne, 3);
            Assert.AreEqual(stats.NbPartiePerdu, 4);
            Assert.AreEqual(stats.NbPartiePerdu + stats.NbPartieGagne, 7);
            Assert.AreEqual(stats.Score, -1);
        }
        [TestMethod]
        public void AjoutPersonne()
        {
            Joueur unJoueur = null;
            Utilitaire.putJoueur("Catwoman");
            List<Joueur> listeJoueur = Utilitaire.getUtils();
            unJoueur = listeJoueur.Find(p => p.Nom.Equals("Catwoman"));
            Assert.AreEqual(unJoueur.Nom, "Catwoman");
            Statistique stats = Utilitaire.getSats(unJoueur.NoJoueur);
            Assert.IsNotNull(stats);
            Utilitaire.deleteUtils(unJoueur.NoJoueur);
            unJoueur = null;
            listeJoueur = Utilitaire.getUtils();
            unJoueur = listeJoueur.Find(p => p.Nom.Equals("Catwoman"));
            Assert.IsNull(unJoueur);
        }
        public String[] leDico(Langues lang)
        {
            String chemin = "";
            string[] dico;
            switch (lang)
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
            return dico;
        }

    }
}
