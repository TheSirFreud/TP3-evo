using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JeuxDuPendu;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Net.NetworkInformation;

namespace ServeurJeuDuPendu
{
    class ServeurTCP
    {
        //Attributs
        TcpListener lEcouteur;
        string adresseIP;
        int noPort;
        Mots lesMots;
        Langues laLangue;

        //Constructeur
        public ServeurTCP(string adresseIP, int noPort)
        {
            this.adresseIP = adresseIP;
            this.noPort = noPort;
            this.lEcouteur = new System.Net.Sockets.TcpListener
                    (IPAddress.Parse(adresseIP), noPort);
            this.laLangue = Langues.Aucune;
        }

        //Méthodes

        /// <summary>
        /// Démarrage des services utilisés
        /// </summary>
        public void Demarrer()
        {
            lEcouteur.Start();
            ExecBouclePrincipale();
        }

        /// <summary>
        /// Boucle principale du serveur, à apeller dans Demarrer
        /// </summary>
        private void ExecBouclePrincipale()
        {
            string strLangue;
            if (laLangue == Langues.Aucune)
            {
                do
                {
                    //Choix de la langue des clients
                    Console.WriteLine("Quelle es la langue du serveur? (FR/EN)");
                    strLangue = Console.ReadLine();

                    if (strLangue.ToUpper() == "FR")
                        laLangue = Langues.Fraçais;
                    else if (strLangue.ToUpper() == "EN")
                        laLangue = Langues.Anglais;
                    else
                        Console.WriteLine("Mauvais choix de langue");
                }
                while (strLangue.ToUpper() != "FR" && strLangue.ToUpper() != "EN");
            }

            //Comme il ne peut qu'y avoir 2 joueurs pour un serveur,
            //coder le tout de manière linéaire

            Console.WriteLine("Attente de connexion...");
            TcpClient clientNo1 = lEcouteur.AcceptTcpClient();

            Console.WriteLine("Attente d'un deuxième joueur...");
            TcpClient clientNo2 = lEcouteur.AcceptTcpClient();

            //Deux joueurs maintenant connectés, démarrer la partie
            Console.WriteLine("Deux joueurs, démarrage de la partie");
            Console.WriteLine("Génération et envoi du mot...");

            //Création du mot
            lesMots = new Mots(laLangue);
            lesMots.InitialiserMotsATrouver();

            EnvoyerReponse(clientNo1, lesMots.Mot);
            EnvoyerReponse(clientNo2, lesMots.Mot);

            Console.WriteLine("Le mot à trouver : " + lesMots.Mot);
            Console.WriteLine("Attente d'un gagnant ou de deux perdants...");

            //Attente de la réponse d'un des deux clients
            while (!clientNo1.GetStream().DataAvailable && !clientNo2.GetStream().DataAvailable)
                Thread.Sleep(50);

            //Le client envoi soit "GAGNÉ" ou "PERDU" lorsqu'il fini la partie
            bool aGagneC1 = false;
            bool aGagneC2 = false;
            bool aPerduC1 = false;
            bool aPerduC2 = false;
            bool lesDeuxClientsOntPerdus = false;

            //Réception d'une réponse, envoi d'un message de fin lorsqu'un des deux clients a gagné, ou que les deux ont perdus,
            //sinon attendre la réponse d'un client
            while (!aGagneC1 && !aGagneC2 && !lesDeuxClientsOntPerdus)
            {
                if (clientNo1.GetStream().DataAvailable)
                {
                    string reponse = LireReponse(clientNo1);
                    if (reponse == "GAGNÉ")
                    {
                        aGagneC1 = true;
                        EnvoyerReponse(clientNo1, "GAGNÉ");
                        EnvoyerReponse(clientNo2, "PERDU");
                        Console.WriteLine("Le client no. 2 a perdu");
                    }
                    else if (reponse == "PERDU")
                    {
                        aPerduC1 = true;
                    }
                }
                else if (clientNo2.GetStream().DataAvailable)
                {
                    string reponse = LireReponse(clientNo2);
                    if (reponse == "GAGNÉ")
                    {
                        aGagneC2 = true;
                        EnvoyerReponse(clientNo1, "PERDU");
                        EnvoyerReponse(clientNo2, "GAGNÉ");
                        Console.WriteLine("Le client no. 1 a perdu");
                    }
                    else if (reponse == "PERDU")
                    {
                        aPerduC2 = true;
                    }
                }

                if (aPerduC1 && aPerduC2)
                {
                    lesDeuxClientsOntPerdus = true;
                    EnvoyerReponse(clientNo1, "PERDU");
                    EnvoyerReponse(clientNo2, "PERDU");
                }

                Thread.Sleep(50);
            }

            //Ajout du mot essayé
            lesMots.AjouterMot();
            //Redémarrer le jeu
            RedemarrerPartie();
        }

        /// <summary>
        /// Méthode permettant de recommencer une partie
        /// </summary>
        public void RedemarrerPartie()
        {
            Console.WriteLine("La partie redémarre...");
            lEcouteur.Stop();
            //Réinitialisation du serveur
            lEcouteur = new System.Net.Sockets.TcpListener
                    (IPAddress.Parse(adresseIP), noPort);
            Demarrer();
        }

        /// <summary>
        /// Méthode permettant de lire une réponse d'un expéditeur TCP
        /// </summary>
        /// <param name="expediteur">L'expéditeur du message</param>
        /// <returns>La réponse de l'expéditeur</returns>
        private string LireReponse(TcpClient expediteur)
        {
            byte[] buffer = new byte[256];
            int nbrBytesLusTotal = 0;

            //Lecture jusqu'à la fin
            while (expediteur.GetStream().DataAvailable)
            {
                int nbrBytesLus = expediteur.GetStream().Read(buffer, nbrBytesLusTotal, buffer.Length - nbrBytesLusTotal);
                nbrBytesLusTotal += nbrBytesLus;
            }

            return Encoding.Unicode.GetString(buffer, 0, nbrBytesLusTotal);
        }

        /// <summary>
        /// Méthode permettant d'envoyer un message à un destinataire TCP
        /// </summary>
        /// <param name="destinataire">Le destinataire TCP</param>
        /// <param name="message">Le message à envoyer</param>
        private void EnvoyerReponse(TcpClient destinataire, string message)
        {
            byte[] byteReponse = Encoding.Unicode.GetBytes(message);
            destinataire.GetStream().Write(byteReponse, 0, byteReponse.Length);
        }
    }
}
