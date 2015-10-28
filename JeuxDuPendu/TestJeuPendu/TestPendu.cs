using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JeuxDuPendu;

namespace TestJeuPendu
{
    [TestClass]
    public class TestPendu
    {
        Mots unMot;

        [TestInitialize]
        public void init()
        {
            unMot = new Mots(Langues.Fraçais);
        }

        [TestMethod]
        public void ChangerLangueDico()
        {
           
        }
    }
}
