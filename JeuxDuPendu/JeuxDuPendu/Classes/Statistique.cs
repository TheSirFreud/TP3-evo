using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeuxDuPendu
{
    public class Statistique
    {
       
        public int NbPartieGagne { get; set; }
        public int NbPartiePerdu { get; set; }

        public int Score { get; set; }

        public Statistique(int gagne, int perdu, int score)
        {
            
            NbPartieGagne = gagne;
            NbPartiePerdu = perdu;
            Score = score;
        }
    }
}
