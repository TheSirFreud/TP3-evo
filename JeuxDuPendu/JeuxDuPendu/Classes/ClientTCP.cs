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
using System.Diagnostics;


namespace JeuxDuPendu
{
    class GestionnaireClientTCP
    {
        //Attributs
        JeuxPendu parent;
        string adresseIP;
        int noPort;
        TcpClient leClient;
        string leMotADevinerRecu;
        BackgroundWorker bwAttReponseStatusPartie;

        //Constructeur
        public GestionnaireClientTCP(string adresseIP, int noPort, JeuxPendu parent)
        {
            this.adresseIP = adresseIP;
            this.noPort = noPort;

            //Obtenir le parent (JeuxPendu)
            this.parent = parent;
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
            catch (SocketException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Boucle de début du client TCP
        /// </summary>
        public void execBouclePrincipale()
        {
            //Créer un background worker qui attend la réponse du serveur qu'il y a un deuxième joueur
            BackgroundWorker bWorker = new BackgroundWorker();
            bWorker.WorkerSupportsCancellation = true;
            bWorker.DoWork +=
                new DoWorkEventHandler(BwAttendreDeuxiemeJoueur);
            bWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(BwAttendreDeuxiemeJoueurCompletee);

            //Le démarrer
            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Méthode exécutée par un background worker qui attend que le serveur indique qu'il y a un deuxième joueur pour commencer la partie.
        /// </summary>
        private void BwAttendreDeuxiemeJoueur(object sender, DoWorkEventArgs e)
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

        /// <summary>
        /// Méthode qui s'exécute lorsque le serveur indique avoir trouvé un deuxième joueur (Continuation de l'exécution normale)
        /// </summary>
        private void BwAttendreDeuxiemeJoueurCompletee(object sender, RunWorkerCompletedEventArgs e)
        {
            //Deuxième joueur trouvé, début de la partie
            //Note : La valeur du string est déjà initialisée
            parent.NouvellePartieEnLigne(leMotADevinerRecu);

            //Attente du message de la fin de la partie par le serveur
            bwAttReponseStatusPartie = new BackgroundWorker();
            bwAttReponseStatusPartie.WorkerSupportsCancellation = true;
            bwAttReponseStatusPartie.DoWork +=
                new DoWorkEventHandler(BwAttReponseStatusPartie);
            bwAttReponseStatusPartie.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(BwAttReponseStatusPartieTeminee);

            if (!bwAttReponseStatusPartie.IsBusy)
                bwAttReponseStatusPartie.RunWorkerAsync();
        }

        /// <summary>
        /// Méthode exécutée par un background worker qui attend le message du serveur de fin de partie
        /// </summary>
        private void BwAttReponseStatusPartie(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (true)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {

                    string messageRecu = "";
                    messageRecu = LireReponse();

                    if (messageRecu == null || messageRecu == "")
                        Thread.Sleep(50);
                    else
                    {
                        //Si c'est une défaite, enregistrer comme cancel
                        if (messageRecu == "PERDU")
                        {
                            e.Cancel = true;
                            break;
                        }
                        else if(messageRecu == "GAGNÉ")
                        {
                            e.Cancel = false;
                            break;
                        }
                            
                    }
                }

            }
        }

        /// <summary>
        /// Méthode qui s'exécute lorsque le serveur indique que l'autre client a gagné
        /// </summary>
        private void BwAttReponseStatusPartieTeminee(object sender, RunWorkerCompletedEventArgs e)
        {
            //Indiquer une défaite (Cancel == Défaite)
            if (e.Cancelled)
                parent.Perdu();
            else
                parent.Gagne();

            //Redémarrer la partie à l'aide d'un background worker
            BackgroundWorker bwRedemarrerPartie = new BackgroundWorker();
            bwRedemarrerPartie.WorkerSupportsCancellation = true;
            bwRedemarrerPartie.DoWork +=
                new DoWorkEventHandler(BwRedemarrerPartie);
            bwRedemarrerPartie.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(BwRedemarrerPartieTeminee);

            if (!bwRedemarrerPartie.IsBusy)
                bwRedemarrerPartie.RunWorkerAsync();
        }

        /// <summary>
        /// Méthode permettant d'attendre 5 secondes avant le redémarrage de la partie
        /// </summary>
        private void BwRedemarrerPartie(object sender, DoWorkEventArgs e)
        {
            leClient.Close();
            //Le temps que le serveur effectue le "redémarrage"
            Thread.Sleep(5000);
        }

        /// <summary>
        /// Méthode permettant de redémarrer une partie côté client
        /// </summary>
        private void BwRedemarrerPartieTeminee(object sender, RunWorkerCompletedEventArgs e)
        {
            parent.demarrerEnLigne();
        }

        /// <summary>
        /// Méthode permettant de lire une réponse du serveur
        /// </summary>
        /// <returns>La réponse du serveur</returns>
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

        /// <summary>
        /// Méthode permettant d'envoyer un message au serveur
        /// </summary>
        /// <param name="message">Le message à envoyer</param>
        private void EnvoyerReponse(string message)
        {
            byte[] byteReponse = Encoding.Unicode.GetBytes(message);
            leClient.GetStream().Write(byteReponse, 0, byteReponse.Length);
        }

        /// <summary>
        /// Méthode qui envoie au serveur que ce client a gagné
        /// </summary>
        public void EnvoyerGagne()
        {
            EnvoyerReponse("GAGNÉ");
        }

        /// <summary>
        /// Méthode qui envoie au serveur que ce client a perdu
        /// </summary>
        public void EnvoyerPerdu()
        {
            EnvoyerReponse("PERDU");
        }
    }
}
