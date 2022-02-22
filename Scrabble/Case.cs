using System;
using System.Collections.Generic;
using System.Text;

namespace Scrabble
{
    public class Case
    {
        private string bonus;
        private Jeton jeton_case;
        private List<string[]> mots_direction;
        /*liste de tableau de string (mot et direction) permet d'avoir accès 
         * depuis le plateau dont font partie les différentes lettres et donc aux mots présents sur le plateau
           Ainsi qu' à leur direction*/

        /// <summary>
        /// Initialise une instance de la classe Case avec la valeur bonus spécifiée
        /// </summary>
        /// <returns></returns>
        public Case(string bonus)
        {
            this.bonus = bonus;
            this.jeton_case = new Jeton(' ');
            mots_direction = new List<string[]>();
        }
        public List<string[]> Mots
        {
            get { return mots_direction; } // accès en consultation
            set { mots_direction = value; } // accès en modification
        }
        public string Bonus
        {
            get { return bonus; } // accès en consultation
        }
        public Jeton Jeton_case
        {
            get { return jeton_case; } // accès en consultation
            set { jeton_case = value; } // accès en modification
        }
        /// <summary>
        /// Recherche : "mot_recherché" avec la direction : "direction_recherché" dans la liste de couple mot-direction
        /// <br />
        /// <br />
        /// Retourne :  
        /// <br />
        /// un int qui correspond à l'index du couple mot-direction recherché dans la liste de couple mot-direction
        /// </summary>
        /// <returns></returns>
        public int Recherche_Mot(string  mot_recherché, string direction_recherché)
        {
            for(int i = 0; i< mots_direction.Count; i++) // on parcourt la liste de tableau
            {
                if(mots_direction[i][0] == mot_recherché && mots_direction[i][1] == direction_recherché) // on regarde si le mot (l'index 0 du tableau correspond au mot recherché, pareil pour la direction)
                {
                    return i; // on retourne l'index
                }
            }
            return 0;
        }

        /// <summary>
        /// Crée un string qui représente la case spécifiée
        /// <br />
        /// <br />
        /// Retourne :  
        /// <br />
        /// Retourne un string qui représente la case spécifiée
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string liste = "";
            string retour = "";
            for(int i = 0; i< mots_direction.Count; i++) // on parcourt la liste de tableau
            {
                liste = liste + mots_direction[i][0] + "-" + mots_direction[i][1]; // on sépare le mot et la direction d'un tiret
                if (i != mots_direction.Count - 1)
                {
                    liste = liste + "|"; 
                }
            }
            retour = Convert.ToString(jeton_case.Lettre) + ";" + liste;
            return retour; // ; entre lettre et le reste  //  - entre mot et direction //  | entre chaque couple mot - direction
        }

    }
}
