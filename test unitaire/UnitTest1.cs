using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace Scrabble
{
    [TestClass]
    public class UnitTest1
    {
        #region RechRecursive
        [TestMethod]
        public void RechRecursive_True_Short()
        {
            Dictionnaire dico = new Dictionnaire("Francais.txt");
            Assert.IsTrue(Scrabble.Dictionnaire.RechDichoRecursif(dico.Liste_RechRecursive("quoi"), "quoi"));
        }
        [TestMethod]
        public void RechRecursive_False_Short()
        {
            Dictionnaire dico = new Dictionnaire("Francais.txt");
            Assert.IsFalse(Scrabble.Dictionnaire.RechDichoRecursif(dico.Liste_RechRecursive("srth"), "srth"));
        }
        [TestMethod]
        public void RechRecursive_True_DébutListe()
        {
            Dictionnaire dico = new Dictionnaire("Francais.txt");
            Assert.IsTrue(Scrabble.Dictionnaire.RechDichoRecursif(dico.Liste_RechRecursive("aas"), "aas"));
        }
        [TestMethod]
        public void RechRecursive_True_FinListe()
        {
            Dictionnaire dico = new Dictionnaire("Francais.txt");
            Assert.IsTrue(Scrabble.Dictionnaire.RechDichoRecursif(dico.Liste_RechRecursive("Zut"), "Zut"));
        }
        [TestMethod]
        public void RechRecursive_True_Long()
        {
            Dictionnaire dico = new Dictionnaire("Francais.txt");
            int k = dico.Dico.Count;
            Assert.IsTrue(Scrabble.Dictionnaire.RechDichoRecursif(dico.Liste_RechRecursive("RAGAILLARDIRONT"), "RAGAILLARDIRONT"));
        }
        [TestMethod]
        public void RechRecursive_False_Long()
        {
            Dictionnaire dico = new Dictionnaire("Francais.txt");
            Assert.IsFalse(Scrabble.Dictionnaire.RechDichoRecursif(dico.Liste_RechRecursive("zsekfnzeonfth"), "zsekfnzeonfth"));
        }
        #endregion
        [TestMethod]
        public void TestPLateau()
        {
            Plateau plateau = new Plateau("Bonus.txt");
            Dictionnaire Dico = new Dictionnaire("Francais.txt");
            Joueur joueur1 = new Joueur("Jean-Jacque");
            Jeton A = new Jeton('A');
            Jeton I = new Jeton('I');
            Jeton P = new Jeton('P');
            Jeton C = new Jeton('C');
            Jeton E = new Jeton('E');
            joueur1.Add_Main_Courante(C);
            joueur1.Add_Main_Courante(E);
            joueur1.Add_Main_Courante(P);
            joueur1.Add_Main_Courante(I);
            joueur1.Add_Main_Courante(E);
            plateau.Add_Jeton(A, 4, 2);
            plateau.Add_Jeton(P, 3, 9);
            plateau.Add_Jeton(I, 4, 9);
            plateau.Add_Jeton(E, 5, 9);
            List<int> croisements = plateau.Croisement_Valide("PIECE", 3, 9, 'b');
            List<string[]> adjacent_valide = plateau.Adjacent_perpendiculaire_valide("PIECE", 3, 9, 'b', Dico);
            Assert.AreEqual(2, plateau.Test_Plateau("PIECE", 3, 9, 'b', Dico, joueur1, croisements, false, adjacent_valide)) ;
        }
        [TestMethod]
        public void Appartient_Main_True()
        {
            Joueur joueur1 = new Joueur("Jean-Jacque");
            Jeton A = new Jeton('A', 1, 1);
            Jeton B = new Jeton('B', 3, 1);
            Jeton C = new Jeton('C', 3, 1);
            Jeton D = new Jeton('D', 2, 1);
            Jeton E = new Jeton('E', 1, 1);
            Jeton F = new Jeton('F', 4, 1);
            Jeton G = new Jeton('G', 2, 1);
            joueur1.Add_Main_Courante(A);
            joueur1.Add_Main_Courante(B);
            joueur1.Add_Main_Courante(C);
            joueur1.Add_Main_Courante(D);
            joueur1.Add_Main_Courante(E);
            joueur1.Add_Main_Courante(F);
            joueur1.Add_Main_Courante(G);
            Assert.AreEqual(0, joueur1.Appartient_Main('A'));
        }
        [TestMethod]
        public void Appartient_Main_False()
        {
            Joueur joueur1 = new Joueur("Jean-Jacque");
            Jeton A = new Jeton('A', 1, 1);
            Jeton B = new Jeton('B', 3, 1);
            Jeton C = new Jeton('C', 3, 1);
            Jeton D = new Jeton('D', 2, 1);
            Jeton E = new Jeton('E', 1, 1);
            Jeton F = new Jeton('F', 4, 1);
            Jeton G = new Jeton('G', 2, 1);
            joueur1.Add_Main_Courante(A);
            joueur1.Add_Main_Courante(B);
            joueur1.Add_Main_Courante(C);
            joueur1.Add_Main_Courante(D);
            joueur1.Add_Main_Courante(E);
            joueur1.Add_Main_Courante(F);
            joueur1.Add_Main_Courante(G);
            Assert.AreEqual(-1, joueur1.Appartient_Main('V'));
        }
        [TestMethod]
        public void Score_Lié_True()
        {
            Assert.AreEqual(2,Jeton.Score_lié('G', "Jetons.txt"));
        }
        [TestMethod]
        public void Recherche_Mot()
        {
            Case test = new Case("Ve");
            string[] mot1 = { "toit", "d" };
            string[] mot2 = {"poney", "b"};
            test.Mots.Add(mot1);
            test.Mots.Add(mot2);
            Assert.AreEqual(0, test.Recherche_Mot("toit", "d"));
        }

    }
}
