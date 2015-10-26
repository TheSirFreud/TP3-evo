using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServeurJeuDuPendu
{
    class Program
    {
        const string localhost = "127.0.0.1";
        const int portParDefaut = 1330;
        static void Main(string[] args)
        {
            ServeurTCP leServeur = new ServeurTCP(localhost, portParDefaut);
            leServeur.Demarrer();
            leServeur.ExecBouclePrincipale();
        }
    }
}
