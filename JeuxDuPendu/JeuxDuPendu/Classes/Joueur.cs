using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeuxDuPendu
{
    public class Joueur : IComparable<Joueur>, IEquatable<Joueur>
    {
        public String Nom { get; set; }
        public int NoJoueur { get; set; }
       

        public Joueur(int noJoueur, String nom)
        {
            NoJoueur = noJoueur;
            Nom = nom;
        }

        public override string ToString()
        {
            return Nom;
        }

        public int CompareTo(Joueur other)
        {
            return Nom.CompareTo(other.Nom);
        }

        public bool Equals(Joueur other)
        {
            return NoJoueur == other.NoJoueur ? true : false;
        }
    }
}
