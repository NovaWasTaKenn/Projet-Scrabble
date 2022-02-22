using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Scrabble
{
    public class Jeton
    {
        // déclaration des champs :
        private char lettre;
        private int score;
        private int nb_jetons;

        //Constructeurs :

        /// <summary>
        /// Constructeur qui prend comme paramètre : 
        /// <br />
        /// lettre, le score et le nombre de jetons
        /// </summary>
        /// <returns></returns>
        public Jeton(char lettre, int score, int nb_jetons)
        {
            this.lettre = char.ToUpper(lettre);
            this.nb_jetons = nb_jetons;
            this.score = score;
        }

        /// <summary>
        /// Constructeur qui prend comme paramètre : 
        /// <br />
        /// lettre et le chemin qui mène au fichier de jeton
        /// </summary>
        /// <returns></returns>
        public Jeton(char lettre, string pth_input)
        {
            this.lettre =char.ToUpper( lettre);
            this.score = Score_lié(lettre, pth_input);
        }

        /// <summary>
        /// Constructeur qui prend en paramètre :
        /// <br />
        /// la copie du jeton, pour ne pas modifier directement dans le sac de jetons,
        /// <br />
        /// et pour ne pas modifier le nombre de jetons on lui attribut nombre
        /// </summary>
        /// <returns></returns>
        public Jeton(Jeton jeton, int nombre)
        {
            this.lettre = jeton.lettre;
            this.score = jeton.score;
            this.nb_jetons = nombre;
        }

        /// <summary>
        /// Constructeur qui prend uniquement en paramètre lettre
        /// </summary>
        /// <returns></returns>
        public Jeton(char lettre)
        {
            this.lettre = lettre;
        }

        /// <summary>
        /// Constructeur qui prend en paramètre :
        /// <br />
        /// la copie du jeton, pour ne pas modifier directement dans le sac de jetons
        /// </summary>
        /// <returns></returns>
        public Jeton(Jeton jeton)
        {
            this.lettre = jeton.lettre;
            this.score = jeton.score;
            this.nb_jetons = jeton.nb_jetons;
        }
        #region Propriétées
        public char Lettre 
        {
            get { return lettre; } // accès en consultation
        }
        public int Nb_jetons
        {
            get { return nb_jetons; } // accès en consultation
            set { nb_jetons = value; } // accès en modification
        }
        public int Score
        {
            get { return score; } // accès en consultation
        }
        #endregion

        /// <summary>
        /// Retire un nombre au jeton 
        /// </summary>
        /// <returns></returns>
        public void Retire_Un_Nombre()
        {
            if(nb_jetons > 0)
            {
                nb_jetons--;
            }
        }

        /// <summary>
        /// On retourne un string qui comporte la lettre, son score, et le nombre de jeton de la lettre
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return lettre + ";" + score + ";" + nb_jetons;
        }

        /// <summary>
        /// Assigne un score à une lettre
        /// <br />
        /// Retourne : 
        /// <br />
        /// Retourne le score
        /// </summary>
        /// <returns></returns>
        public static int Score_lié(char lettre,string pth_input)
        {
            //méthode utilisée lors de la création des jetons mis dans le plateau (et chaque lettre est donc assignée à un score)
            string s;
            string[] s_tab;
            int score = 0;
            bool stop= false;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(pth_input); // parcourt le fichier de jetons
                while ((s = sr.ReadLine()) != null && !stop)
                {
                    s_tab = s.Split(';'); // on découpe au niveau des points virgules
                    if (Convert.ToChar(s_tab[0].ToUpper()).Equals(char.ToUpper(lettre))) // si le premier index correspond à la lettre recherché
                    {
                        score = Convert.ToInt32(s_tab[1]); // le score à retourner est ce qui se trouve en index 1
                        stop = true;
                    }
                }
                sr.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return score;
        }
    }
}
