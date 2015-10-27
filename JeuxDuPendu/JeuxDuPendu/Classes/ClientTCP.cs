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
        string leMotADevinerRecu;
        BackgroundWorker backWorkerAttendreReponseDefaite;

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
        /// Méthode qui envoie au serveur que ce client a gagné
        /// </summary>
        public void EnvoyerGagne()
        {
            //Message pas important, c'est le geste qui compte
            EnvoyerReponse("J'ai gagné!");
            //Arrêter le background worker qui attend une réponse "J'ai gagné" de l'autre client
            backWorkerAttendreReponseDefaite.CancelAsync();

        }

        /// <summary>
        /// Méthode qui s'exécute lorsque le serveur indique avoir trouvé un deuxième joueur (Continuation de l'exécution normale)
        /// </summary>
        private void bwAttendreDeuxiemeJoueurCompletee(object sender, RunWorkerCompletedEventArgs e)
        {
            //Deuxième joueur trouvé, début de la partie
            //Note : La valeur du string est déjà initialisée
            parent.NouvellePartieEnLigne(leMotADevinerRecu);

            //Attente et vérification de la réception de "J'ai gagné" de l'autre client
            backWorkerAttendreReponseDefaite = new BackgroundWorker();
            backWorkerAttendreReponseDefaite.WorkerSupportsCancellation = true;
            backWorkerAttendreReponseDefaite.DoWork +=
                new DoWorkEventHandler(bwAttendreReponseDefaite);
            backWorkerAttendreReponseDefaite.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(bwAttendreReponseDefaiteTerminee);

            if (!backWorkerAttendreReponseDefaite.IsBusy)
                backWorkerAttendreReponseDefaite.RunWorkerAsync();
        }

        /// <summary>
        /// Méthode qui s'exécute lorsque le serveur indique que l'autre client a gagné
        /// </summary>
        private void bwAttendreReponseDefaiteTerminee(object sender, RunWorkerCompletedEventArgs e)
        {
            //Indiquer une défaite
            if(!e.Cancelled)
                parent.Perdu();
        }

        /// <summary>
        /// Méthode exécutée par un background worker qui attend que le serveur indique que l'autre client a gagné
        /// </summary>
        private void bwAttendreReponseDefaite(object sender, DoWorkEventArgs e)
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
                    string defaiteRecue = "";
                    defaiteRecue = LireReponse();

                    if (defaiteRecue == null || defaiteRecue == "")
                        Thread.Sleep(50);
                    else
                    {
                        //Il y a défaite, continuer
                        e.Result = DialogResult.OK;
                        break;
                    }
                }

            }
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
                        leMotADevinerRecu = leMotRecu;
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

        private void EnvoyerReponse(string message)
        {
            byte[] byteReponse = Encoding.Unicode.GetBytes(message);
            leClient.GetStream().Write(byteReponse, 0, byteReponse.Length);
        }
    }
}
