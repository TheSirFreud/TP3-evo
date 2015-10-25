using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace JeuxDuPendu
{
    class GestionnaireClientTCP
    {
        //TODO : La gestion de connexion devrait être intrisèque,
        //même chose pour le serveur
        //Attribut
        string adresseIP;
        int noPort;
        TcpClient leClient;

        //Constructeur
        public GestionnaireClientTCP(string adresseIP, int noPort)
        {
            this.adresseIP = adresseIP;
            this.noPort = noPort;
        }

        //Méthodes

        /// <summary>
        /// Tentative de connexion au serveur
        /// </summary>
        /// <returns>Retourne true si la connexion est réussie.</returns>
        public bool Connexion()
        {
            try
            {
                leClient = new TcpClient(adresseIP, noPort);
            }
            catch (SocketException err)
            {
                return false;
            }

            return true;
        }

        public void execBouclePrincipale()
        {
            bool finExec = false;
            while (!finExec)
            {
                //Si l'on ne reçoit pas de mot, cela veut dire
                //qu'on attend le deuxième client, donc attendre
                //aussi
                string leMot = "";
                if (leClient.GetStream().DataAvailable)
                    leMot = LireReponse();

                if (leMot == null || leMot == "")
                {
                    //Note : le serveur répond à toutes les
                    //500 milisecondes
                    Thread.Sleep(200);
                }
            }
        }

        private string LireReponse()
        {
            byte[] buffer = new byte[256];
            int nbrBytesLusTotal = 0;

            //Lecture jusqu'à la fin
            do
            {
                int nbrBytesLus = leClient.GetStream().Read(buffer, nbrBytesLusTotal, buffer.Length - nbrBytesLusTotal);
                nbrBytesLusTotal += nbrBytesLus;
            }
            while (leClient.GetStream().DataAvailable);

            return Encoding.Unicode.GetString(buffer, 0, nbrBytesLusTotal);
        }

        //private Task<string> LireReponseAsync()
        //{
        //    byte[] buffer = new byte[256];
        //    int nbrBytesLusTotal = 0;

        //    //Lecture jusqu'à la fin
        //    do
        //    {
        //        int nbrBytesLus = leClient.GetStream().Read(buffer, nbrBytesLusTotal, buffer.Length - nbrBytesLusTotal);
        //        nbrBytesLusTotal += nbrBytesLus;
        //    }
        //    while (leClient.GetStream().DataAvailable);
        //}
    }
}
