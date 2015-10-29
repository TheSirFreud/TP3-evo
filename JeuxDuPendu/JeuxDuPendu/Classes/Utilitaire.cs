using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.OleDb;
using System.Reflection;
using System.ComponentModel;

namespace JeuxDuPendu
{
    public static class Utilitaire
    {
        private static String connBD = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\..\bdJeuPendu.accdb;";
        private static OleDbCommand commande;

        //Methode diverses
        public static string GetDescription(object enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                return ((DescriptionAttribute)attrs[0]).Description;
            }
            return enumValue.ToString();
        }
        public static Image Redimention(Image image, int largeur, int hauteur)
        {
            var ratioX = (double)largeur / image.Width;
            var ratioY = (double)hauteur / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }


        //Methodes base de données
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //Retourne les utilisatuers
        public static List<Joueur> getUtils()
        {
            List<Joueur> nomsUtil = new List<Joueur>();
            OleDbConnection connexion = new OleDbConnection(connBD);
            try
            {
                connexion.Open();
                commande = new OleDbCommand("SELECT * FROM tblUtilisateurs", connexion);

                OleDbDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    String nom = reader["nom"].ToString();
                    int no = Convert.ToInt32(reader["noUtil"]);
                    nomsUtil.Add(new Joueur(no, nom));
                }

                return nomsUtil;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                commande.Dispose();
                connexion.Close();
            }
        }
        public static Joueur getUtils(int noJoueur)
        {
            List<Joueur> nomsUtil = new List<Joueur>();
            OleDbConnection connexion = new OleDbConnection(connBD);
            Joueur unJoueur = null;
            try
            {

                connexion.Open();
                commande = new OleDbCommand("SELECT * FROM tblUtilisateurs WHERE noUtil=@noUtil", connexion);
                commande.Parameters.Add("@noUtil", OleDbType.Integer).Value = noJoueur;

                OleDbDataReader reader = commande.ExecuteReader();
                if (reader.Read())
                {
                    String nom = reader["nom"].ToString();
                    int no = Convert.ToInt32(reader["noUtil"]);
                    unJoueur = new Joueur(no, nom);
                }

                return unJoueur;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                commande.Dispose();
                connexion.Close();
            }
        }
        //Ajoute un utilisateur
        public static int putJoueur(String nomJoueur)
        {
            OleDbCommand commandeValidation;
            OleDbCommand commandeStat;
            OleDbConnection connexion = new OleDbConnection(connBD);
            try
            {
                connexion.Open();
                commandeValidation = new OleDbCommand("SELECT nom FROM tblUtilisateurs WHERE nom=@nomJoueur", connexion);
                commandeValidation.Parameters.Add("@nom", OleDbType.VarChar).Value = nomJoueur;
                OleDbDataReader reader = commandeValidation.ExecuteReader();

                if (!reader.Read())
                {
                    commande = new OleDbCommand("INSERT INTO tblUtilisateurs (nom) VALUES (@nom)", connexion);
                    commande.Parameters.Add("@nom", OleDbType.VarChar).Value = nomJoueur;

                    commande.ExecuteNonQuery();

                    OleDbCommand getId = new OleDbCommand("SELECT @@Identity", connexion);
                    int id = (int)getId.ExecuteScalar();

                    commandeStat = new OleDbCommand("INSERT INTO tblStatistique (nbPartieGagne,nbPartiePerdu,score,noJoueur) VALUES (0,0,0,@noJoueur)", connexion);
                    commandeStat.Parameters.Add("@nom", OleDbType.Integer).Value = id;
                    commandeStat.ExecuteNonQuery();
                    return id;
                }
                else
                {
                    throw new Exception("Nom déjà existant");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                commande.Dispose();
                connexion.Close();
            }
        }
        public static void deleteUtils(int noJoueur)
        {
            List<Joueur> nomsUtil = new List<Joueur>();
            OleDbConnection connexion = new OleDbConnection(connBD);
            try
            {
                connexion.Open();
                commande = new OleDbCommand("DELETE FROM tblUtilisateurs WHERE noUtil=@noUtil", connexion);
                commande.Parameters.Add("@noUtil", OleDbType.Integer).Value = noJoueur;

                commande.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                commande.Dispose();
                connexion.Close();
            }
        }
        //Retourne les stats
        public static Statistique getSats(int noJoueur)
        {
            Statistique stat = null;
            OleDbConnection connexion = new OleDbConnection(connBD);
            try
            {
                connexion.Open();
                commande = new OleDbCommand("SELECT * FROM tblStatistique WHERE noJoueur=@noJoueur", connexion);
                commande.Parameters.Add("@noJoueur", OleDbType.Integer).Value = noJoueur;

                OleDbDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    int nbGagne = reader["nbPartieGagne"] == DBNull.Value ? 0 : Convert.ToInt32(reader["nbPartieGagne"]);
                    int nbPerdu = reader["nbPartiePerdu"] == DBNull.Value ? 0 : Convert.ToInt32(reader["nbPartiePerdu"]);
                    int score = reader["score"] == DBNull.Value ? 0 : Convert.ToInt32(reader["score"]);
                    stat = new Statistique(nbGagne, nbPerdu, score);
                }

                return stat;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                commande.Dispose();
                connexion.Close();
            }
        }
        public static Dictionary<String, Statistique> getTop3()
        {

            Dictionary<String, Statistique> dico = new Dictionary<String, Statistique>();
            OleDbConnection connexion = new OleDbConnection(connBD);
            Joueur unJoueur = null;
            try
            {
                connexion.Open();
                commande = new OleDbCommand("SELECT * FROM tblStatistique", connexion);

                OleDbDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    unJoueur = null;
                    unJoueur = getUtils(Convert.ToInt32(reader["noJoueur"]));
                    int nbGagne = reader["nbPartieGagne"] == DBNull.Value ? 0 : Convert.ToInt32(reader["nbPartieGagne"]);
                    int nbPerdu = reader["nbPartiePerdu"] == DBNull.Value ? 0 : Convert.ToInt32(reader["nbPartiePerdu"]);
                    int score = reader["score"] == DBNull.Value ? 0 : Convert.ToInt32(reader["score"]);

                    dico.Add(unJoueur.Nom, new Statistique(nbGagne, nbPerdu, score));
                }
                dico = dico.OrderByDescending(p => p.Value.Score).ToDictionary(pair => pair.Key, pair => pair.Value);
                return dico;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                commande.Dispose();
                connexion.Close();
            }
        }
        //Update les stats 
        public static void ResetStats(int noJoueur)
        {

            OleDbConnection connexion = new OleDbConnection(connBD);

            try
            {
                connexion.Open();

                commande = new OleDbCommand("UPDATE tblStatistique SET nbPartieGagne=0,nbPartiePerdu=0, score=0 WHERE noJoueur=@noJoueur", connexion);
                commande.Parameters.Add("@noJoueur", OleDbType.Integer).Value = noJoueur;
                commande.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                commande.Dispose();
                connexion.Close();
            }
        }
        public static void updateSats(int noJoueur, bool estGagne, NiveauDiff nivDiff)
        {
            int nbPoint = 1;
            OleDbConnection connexion = new OleDbConnection(connBD);
            switch (nivDiff)
            {
                case NiveauDiff.Facile:
                    nbPoint = 1;
                    break;
                case NiveauDiff.Moyen:
                    nbPoint = 2;
                    break;
                case NiveauDiff.Difficile:
                    nbPoint = 3;
                    break;
            }
            try
            {
                connexion.Open();
                if (estGagne)
                {
                    commande = new OleDbCommand("UPDATE tblStatistique SET tblStatistique.nbPartieGagne = nbPartieGagne+1, tblStatistique.score=score+@score WHERE noJoueur=@noJoueur", connexion);
                }
                else
                {
                    nbPoint *= -1;
                    commande = new OleDbCommand("UPDATE tblStatistique SET tblStatistique.nbPartiePerdu = nbPartiePerdu+1, tblStatistique.score=score+@score WHERE noJoueur=@noJoueur", connexion);
                }
                commande.Parameters.Add("@score", OleDbType.Integer).Value = nbPoint;
                commande.Parameters.Add("@noJoueur", OleDbType.Integer).Value = noJoueur;
                commande.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                commande.Dispose();
                connexion.Close();
            }
        }
    }
}