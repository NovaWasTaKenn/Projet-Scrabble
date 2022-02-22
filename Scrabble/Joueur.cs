using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Scrabble
{
    public class Joueur
    {
        // déclaration des champs :
        private string nom; // nom du joueur
        private int score; // le score du joueur
        private List<string> mot_trouvés_lst; // liste des mots trouvés au cours de la partie
        private List<Jeton> main_courante_lst; // liste de tous les jetons d'un joueur

        // Déclaration du constructeur 

        /// <summary>
        /// Constructeur qui prend comme paramètre : 
        /// <br />
        /// le nom du joueur
        /// </summary>
        /// <returns></returns>
        public Joueur(string nom)
        {
            this.nom = nom;
            this.score = 0;
            this.mot_trouvés_lst = null;
            this.main_courante_lst = new List<Jeton>();
        }
        public List<Jeton> Main_courante
        {
            get { return main_courante_lst; } // accès en consultation
        } 
        public List<string> Mot_trouvés
        {
            get { return mot_trouvés_lst; } // accès en consultation
        }
        public string Nom
        { 
            get { return nom; } // accès en consultation
        }
        public int Score
        {
            get { return score; } // accès en consultation
        }

        /// <summary>
        /// Paramètre : mot 
        /// <br />
        /// ajoute le mot dans la liste des mots trouvés par le joueur
        /// </summary>
        /// <returns></returns>
        public void Add_Mot(string mot)
        {
            if (mot_trouvés_lst == null)
            {
                mot_trouvés_lst = new List<string>();
            }
            mot_trouvés_lst.Add(mot.ToUpper());
        }

        /// <summary>
        /// Retourne :
        /// <br />
        /// Retourne un string qui décrit les informations d'un joueur (points et mots trouvés)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string retour = "";
            if(mot_trouvés_lst == null)
            {
                retour = nom + " a " + score+ " points";
            }
            else
            {
                string mot_trouvés_str = "";
                for (int i = 0; i < mot_trouvés_lst.Count; i++) // boucle parcourant tous les mots trouvés
                {
                    mot_trouvés_str = mot_trouvés_str + mot_trouvés_lst[i] + " ; ";
                }
                retour = nom + " a " + score + " points, il/elle a trouvé les mots suivants :\n" + mot_trouvés_str.Substring(0,mot_trouvés_str.Length-3); // substring récupère une sous-chaine à partir d'une chaine de caractère
            }
            return retour;

        }

        /// <summary>
        /// Crée le string permettant l'affichage de la main
        /// <br />
        /// Retourne :  
        /// <br />
        /// un string représentant la main
        /// </summary>
        /// <returns></returns>
        public string Affichage_Main() // affichage de la main
        {
            string main_courante = "";
            for(int i =0; i < main_courante_lst.Count; i++)  // boucle parcourant tous les jetons
            {
                main_courante = main_courante + main_courante_lst[i].Lettre + " x "+main_courante_lst[i].Nb_jetons + "  |  ";
            }
            main_courante.Trim(' ', '|'); // on retire les caractères au debut et à la fin pour l'affichage
            return main_courante;
        }

        /// <summary>
        /// Ajoute une valeur au score
        /// </summary>
        /// <returns></returns>
        public void Add_Score(int val)
        {
            this.score += val;
        }

        /// <summary>
        /// Ajoute un jeton à la main courante
        /// </summary>
        /// <returns></returns>
        public void Add_Main_Courante(Jeton jeton)
        {
            int index = this.Appartient_Main(jeton.Lettre);
            if ( index != -1) // si la lettre existe dans la main courante (valeur retournée par la méthode Appartient_Main)
            {
                main_courante_lst[index].Nb_jetons += 1; // on augmente le nombre de jetons
            }
            else
            {
                main_courante_lst.Add(jeton); // on ajoute un jeton à la main courante
            }
        }

        /// <summary>
        /// Retire un jeton à la main courante
        /// </summary>
        /// <returns></returns>
        public void Remove_Main_Courante(Jeton jeton)
        {
            int index = this.Appartient_Main_Test(jeton.Lettre);
            if ( index != -1)
            {
                if(main_courante_lst[index].Nb_jetons > 1)
                {
                    main_courante_lst[index].Retire_Un_Nombre();
                }
                else
                {
                    main_courante_lst.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// Vérifie que la lettre existe dans la main courante
        /// <br />
        /// Retourne : 
        /// <br />
        /// Retourne l'indice 
        /// </summary>
        /// <returns></returns>
        public int Appartient_Main(char lettre)
        {
            int retour = -1;
            for (int i = 0; i < main_courante_lst.Count && retour == -1; i++)
            {
                if (char.ToLower(main_courante_lst[i].Lettre) == char.ToLower(lettre)) { retour = i; } // on retourne l'indice de la lettre si la lettre existe dans la main courante
            }
            return retour;
        }

        /// <summary>
        /// Vérifie si la lettre appartient à la main courante : 
        /// <br />
        /// - Si oui : on retourne l'indice de la lettre 
        /// <br />
        /// - Si non : on vérifie si l'index est un joker
        /// <br />
        /// on retourne alors l'index du joker
        /// <br />
        /// Retourne : 
        /// <br />
        /// Retourne l'indice 
        /// </summary>
        /// <returns></returns>
        public int Appartient_Main_Test(char lettre)
        {
            bool fin_for = false;
            int retour = -1;
            for (int i = 0; i < main_courante_lst.Count && !fin_for; i++)
            {
                if (char.ToLower(main_courante_lst[i].Lettre) == char.ToLower(lettre)) { retour = i; fin_for = true; } // on vérifie si la lettre appartient à la main 
                if (char.ToLower(main_courante_lst[i].Lettre) == '*') { retour = i; } // on vérifie si l'index est un joker
            }
            return retour; // on retourne l'index du joker que si la lettre n'appartient pas à la main courante
        }

        /// <summary>
        /// Retourne :
        /// <br />
        /// Retourne le nombre de jetons de la main courante
        /// </summary>
        /// <returns></returns>
        public int Main_Count()
        {
            int retour = 0;
            for(int i = 0; i<main_courante_lst.Count; i++) // on parcourt tous les jetons
            {
                retour += main_courante_lst[i].Nb_jetons; 
            }
            return retour; // retourne le nombre de jetons de la liste 
        } 
    }
}
