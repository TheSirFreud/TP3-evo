using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows;

namespace JeuxDuPendu
{
    class GestionnaireClientTCP
    {
        //TODO : La gestion de connexion devrait être intrisèque,
        //même chose pour le serveur
        //Attribut
        JeuxPendu parent;
        string adresseIP;
        int noPort;
        Langues laLangue;
        TcpClient leClient;
        string leMotADeviner;

        //Constructeur
        public GestionnaireClientTCP(string adresseIP, int noPort, JeuxPendu parent, Langues laLangue)
        {
            this.adresseIP = adresseIP;
            this.noPort = noPort;

            //Obtenir le parent (JeuxPendu)
            this.parent = parent;

            this.laLangue = laLangue;
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
            BackgroundWorker bWorker = new BackgroundWorker();
            bWorker.WorkerSupportsCancellation = true;
            bWorker.DoWork +=
                new DoWorkEventHandler(bwAttendreDeuxiemeJoueur);
            bWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(bwAttendreDeuxiemeJoueurCompletee);

            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Méthode qui s'exécute lorsque le serveur indique avoir trouvé un deuxième joueur (Continuation de l'exécution normale)
        /// </summary>
        private void bwAttendreDeuxiemeJoueurCompletee(object sender, RunWorkerCompletedEventArgs e)
        {
            //Deuxième joueur trouvé, début de la partie
            //Note : La valeur du string est déjà initialisée
            parent.NouvellePartieEnLigne(leMotADeviner);
        }

        /// <summary>
        /// Méthode exécutée par un background worker qui attend que le serveur indique qu'il y a un deuxième joueur pour commencer la partie.
        /// </summary>
        private void bwAttendreDeuxiemeJoueur(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            bool finExec = false;
            while (!finExec)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    //Si l'on ne reçoit pas de mot, cela veut dire
                    //qu'on attend le deuxième client, donc attendre
                    //aussi
                    string leMotRecu = "";
                    leMotRecu = LireReponse();

                    if (leMotRecu == null || leMotRecu == "")
                        //Note : le serveur répond à toutes les
                        //500 milisecondes
                        Thread.Sleep(200);
                    else
                    {
                        //Un mot a été reçu, initialiser la classe
                        leMotADeviner = leMotRecu;
                        e.Cancel = true;
                        break;
                    }
                }

            }
        }

        private string LireReponse()
        {
            byte[] buffer = new byte[256];
            int nbrBytesLusTotal = 0;

            //Lecture jusqu'à la fin
            while (leClient.GetStream().DataAvailable)
            {
                int nbrBytesLus = leClient.GetStream().Read(buffer, nbrBytesLusTotal, buffer.Length - nbrBytesLusTotal);
                nbrBytesLusTotal += nbrBytesLus;
            }

            return Encoding.Unicode.GetString(buffer, 0, nbrBytesLusTotal);
        }
    }
}
