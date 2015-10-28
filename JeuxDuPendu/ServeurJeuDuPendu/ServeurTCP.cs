using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JeuxDuPendu;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServeurJeuDuPendu
{
    class ServeurTCP
    {
        //Attributs
        TcpListener lEcouteur;
        Mots lesMots;
        //TODO : A changer
        const Langues laLangue = Langues.Fraçais;
        //int nbrJoueurs;

        public ServeurTCP(string adresseIP, int noPort)
        {
            this.lEcouteur = new System.Net.Sockets.TcpListener
                    (IPAddress.Parse(adresseIP), noPort);
            //this.nbrJoueurs = 0;
        }

        public void Demarrer()
        {
            lEcouteur.Start();
        }

        public void ExecBouclePrincipale()
        {
            //Comme il ne peut qu'y avoir 2 joueurs pour un serveur,
            //coder le tout de manière linéaire

            Console.WriteLine("Attente de connexion...");
            TcpClient clientNo1 = lEcouteur.AcceptTcpClient();

            //Un thread par client
            //Thread threadPrClient = new Thread
            //    (new ParameterizedThreadStart(GestionnaireClient));
            //threadPrClient.Start();

            Console.WriteLine("Attente d'un deuxième joueur...");
            TcpClient clientNo2 = lEcouteur.AcceptTcpClient();

            //threadPrClient = new Thread
            //    (new ParameterizedThreadStart(GestionnaireClient));
            //threadPrClient.Start();

            //Deux joueurs maintenant connectés, démarrer la partie
            Console.WriteLine("Deux joueurs, démarrage de la partie");
            Console.WriteLine("Génération et envoi du mot...");

            lesMots = new Mots(laLangue);
            lesMots.InitialiserMotsATrouver();

            EnvoyerReponse(clientNo1, lesMots.Mot);
            EnvoyerReponse(clientNo2, lesMots.Mot);

            Console.WriteLine("Le mot à trouver : " + lesMots.Mot);
            Console.WriteLine("Attente d'un gagnant...");

            //Attente de la réponse d'un des deux clients
            while(!clientNo1.GetStream().DataAvailable && !clientNo2.GetStream().DataAvailable)
                Thread.Sleep(50);

            //Réception d'une réponse, envoi d'un message de fin au joueur perdant
            if (clientNo1.GetStream().DataAvailable)
            {
                EnvoyerReponse(clientNo2, "Perdu");
                Console.WriteLine("Le client no. 2 a perdu");
            } 
            else
            {
                EnvoyerReponse(clientNo1, "Perdu");
                Console.WriteLine("Le client no. 1 a perdu");
            }

            Console.ReadKey();
        }

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

        private void EnvoyerReponse(TcpClient destinataire, string message)
        {
            byte[] byteReponse = Encoding.Unicode.GetBytes(message);
            destinataire.GetStream().Write(byteReponse, 0, byteReponse.Length);
        }

        //public void GestionnaireClient(object clientTCP)
        //{
        //    //Connexion de joueur, donc un joueur de plus
        //    nbrJoueurs++;

        //    TcpClient unClient = clientTCP as TcpClient;

        //    //Envoi du mot à trouver au client,
        //    //ou du message d'attente d'un autre client

        //    while (nbrJoueurs < 2)
        //    {
        //        Thread.Sleep(500);
        //    }

        //    bool finExec = false;
        //    while (!finExec)
        //    {
        //        //?
        //        Thread.Sleep(500);
        //    }
        //}
    }
}
