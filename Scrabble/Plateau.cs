using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scrabble
{
    public class Plateau
    {
        private Case[,] plateau_jeu= new Case[15,15];
        /// <summary>
        /// Initialise une instance de la classe Plateau avec les bonus du fichier passé en paramètre
        /// <br />
        /// Utilisé lors de la création d'une instance de la classe Plateau sans sauvegarde
        /// <br />
        /// <br />
        /// </summary>
        /// <returns></returns>
        public Plateau(string pth_input_bonus)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(pth_input_bonus);
                string s;
                string[] s_tab;
                int index_i = 0;
                do
                {
                    s = sr.ReadLine();
                    if (s != null)
                    {
                        s_tab = s.Split(';'); // on découpe au niveau des points virgules
                        for (int j = 0; j < s_tab.Length; j++)
                        {
                            plateau_jeu[index_i,j] = new Case(s_tab[j]); // on ajoute les bonus aux bonnes cases
                        }
                    }
                    index_i++;
                } while (s != null);
                sr.Close(); //On ferme le streamreader
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
        /// <summary>
        /// Initialise une instance de la classe Plateau avec les bonus du fichier input_bonus, les lettres et les listes de couples du fichier Sauvegarde.txt
        /// <br />
        /// Utilisé lors de la création d'une instance de la classe Plateau à partie d'une sauvegarde
        /// <br />
        /// <br />
        /// </summary>
        /// <returns></returns>
        public Plateau(string pth_input_bonus, string pth_input_jetons)
        {
            StreamReader input =null ;
            StreamReader save = null;
            
            string s_input;
            string[] s_input_tab;
            string s_save;
            string[] s_save_tab;
            int index_i = 0;
            try
            {
                input = new StreamReader(pth_input_bonus);
                save = new StreamReader("Sauvegarde.txt");

                for (int i = 0; i < 4; i++)
                {
                    save.ReadLine();
                }
                do
                {
                    s_input = input.ReadLine();
                    s_save = save.ReadLine();
                    #region Lecture Bonus
                    if (s_input != null)
                    {
                        s_input_tab = s_input.Split(';'); // on découpe au niveau des points virgules
                        for (int j = 0; j < s_input_tab.Length; j++)
                        {
                            plateau_jeu[index_i, j] = new Case(s_input_tab[j]); // on ajoute les bonus aux bonnes cases
                        }
                    }
                    #endregion
                    #region Lecture Sauvegarde
                    if (s_save != null && s_save != "joueurs")
                    {
                        s_save_tab = s_save.Split('_'); // on sépare au niveau des _
                        for (int k = 0; k < s_save_tab.Length; k++)
                        {
                            string[] case_tab = s_save_tab[k].Split(';'); // on sépare au niveau des points virgules
                            // on obtient la lettre et la liste des mots trouvés
                            plateau_jeu[index_i, k].Jeton_case = new Jeton(Convert.ToChar(case_tab[0]), pth_input_jetons); // on initialise le jeton
                            List<string[]> mots = new List<string[]>();
                            if (case_tab.Length > 1) // si la liste des mots trouvés existe
                            {
                                string[] mots_str = case_tab[1].Split('|'); // on sépare chaque couple mot direction par des |
                                for (int h = 0; h < mots_str.Length; h++)
                                {
                                    string[] couple_mot_direction = mots_str[h].Split('-'); // on sépare dans le couple par des -
                                    mots.Add(couple_mot_direction); // on ajoute le tableau dans la liste de mots
                                }
                            }
                        }
                    }
                    #endregion
                    index_i++;
                } while (s_save != "joueurs");

            }
            catch (IOException e)
            {
                Console.WriteLine("Une erreur est survenu");
                Console.WriteLine(e.Message + e.Source);
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur est survenu");
                Console.WriteLine(e.Message + e.Source);
            }
            finally
            {
               
                    if (input != null) { input.Close(); }
                   
                    if (save != null) { save.Close(); }
                    
                
            }
        }

        public Case[,] Plateau_jeu
        {
            get { return plateau_jeu; } // accès en consultation
        }


        /// <summary>
        /// Affiche le plateau sous forme de matrice de lettre en transformant les bonus en couleur de fond 
        /// <br />
        /// <br />
        /// </summary>
        /// <returns></returns>
        public void Affichage_plateau()
        {
            string bonus;
            Console.WriteLine("   0  1  2  3  4  5  6  7  8  9  10 11 12 13 14");
            for(int i = 0; i < plateau_jeu.GetLength(0); i++)
            {
                if (i < 10)
                {
                    Console.Write(i + " |");
                }
                else
                {
                    Console.Write(i + "|");
                }
                for(int j = 0; j < plateau_jeu.GetLength(1); j++)
                {
                    bonus = plateau_jeu[i, j].Bonus;
                    switch (bonus)  //Associe chaque string bonus à une couleur et affiche cette couleur
                    {
                        case "Ve":
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            break;
                        case "Bc":
                            Console.BackgroundColor = ConsoleColor.Blue;
                            break;
                        case "Bf":
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            break;
                        case "Ro":
                            Console.BackgroundColor = ConsoleColor.Magenta;
                            break;
                        case "Ru":
                            Console.BackgroundColor = ConsoleColor.Red;
                            break;
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" "+plateau_jeu[i,j].Jeton_case.Lettre);
                    Console.ResetColor();
                    Console.Write("|");
                   
                } 
                Console.WriteLine();
            }
            
        }
        /// <summary>
        /// Crée un string représentant l'instance de Plateau spécifiée
        /// <br />
        /// 
        /// <br />
        /// Retourne :  
        /// <br />
        /// Retourne un string représentant l'instance de Plateau spécifiée
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string retour = "    0     1     2     3     4     5     6     7     8     9     10    11    12    13    14\n";
            string str;
            for(int i = 0; i < plateau_jeu.GetLength(0); i++)
            {
                if (i < 10)
                {
                    str = i + "  ";
                }
                else
                {
                    str = i + " ";
                }
                for(int j = 0; j < plateau_jeu.GetLength(1); j++)
                {
                    str = str + plateau_jeu[i, j].Bonus + ";" + plateau_jeu[i, j].Jeton_case.Lettre+"  ";
                }
                str.Trim();
                retour = retour + str + "\n";    
            }
            return retour;
        }

        /// <summary>
        /// Ajoute un jeton spécifié à la case i , j de l'instance de Plateau spécifée
        /// <br />
        /// 
        /// <br />
        /// </summary>
        /// <returns></returns>
        public void Add_Jeton(Jeton jeton, int i, int j)
        {
            plateau_jeu[i, j].Jeton_case = new Jeton(jeton);
        }

        /// <summary>
        /// Accomplit différentes action autour du mot défini par adjacent_perpendiculaire_valide
        /// <br />
        /// Elements de adjacent_perpendiculaire_valide dans l'ordre: (mot, ligne, colonne, direction, position dans le mot de la lettre ajoutée)
        /// <br />
        /// <br />
        /// Actions accomplies : 
        /// <br />
        /// modifie la valeur mot de la liste de couples mot-direction pour chaque case où le mot est placé pour qu'elle corresponde au mot spécifié
        /// <br />
        /// Ajoute le score du mot au score du joueur spécifié
        /// <br />
        /// Ajoute le mot à la liste mot trouvés du joueur spécifié
        /// <br />
        /// <br />
        /// </summary>
        /// <returns></returns>
        public void Process_Mot_Adjacent( List<string[]> adjacent_perpendiculaire_valide, Joueur joueur, List<int> croisements)
        {
            for(int i = 0; i< adjacent_perpendiculaire_valide.Count; i++) //Parcourt la liste des différents mots adjacents trouvés
            {
                for (int j = 0; j < adjacent_perpendiculaire_valide[i][0].Length; j++)  //Parcourt le mot en position i de la liste
                {
                    #region direction b
                    if (adjacent_perpendiculaire_valide[i][3] == "b")  
                    {
                        if(j != Convert.ToInt32(adjacent_perpendiculaire_valide[i][4]))  
                        /*Teste si l'index actuel est égal à l'index de la lettre qui a été
                          rajoutée au mot existant pour former le nouveau mot présent dans 
                          la liste adjacent_mot_valide
                          Cette lettre n'est pas encore placée sur le plateau contrairement aux autres*/
                        {                                                                   
                            int ligne = Convert.ToInt32(adjacent_perpendiculaire_valide[i][1]) + j;
                            int colonne = Convert.ToInt32(adjacent_perpendiculaire_valide[i][2]);
                            int index_ancien_mot = plateau_jeu[ligne, colonne].Recherche_Mot(adjacent_perpendiculaire_valide[i][0], adjacent_perpendiculaire_valide[i][3]);
                            /* Trouve l'index de l'ancien mot dans la liste de couples mot-direction de la position ligne colonne du plateau*/
                            if(plateau_jeu[ligne, colonne].Mots != null && plateau_jeu[ligne, colonne].Mots.Count > index_ancien_mot)
                            {
                                plateau_jeu[ligne, colonne].Mots[index_ancien_mot][0] = adjacent_perpendiculaire_valide[i][0];
                            }
                            
                            /* Remplace l'ancien mot par le nouveau mot*/
                        }
                        else
                        {
                            string[] element = { adjacent_perpendiculaire_valide[i][0], adjacent_perpendiculaire_valide[i][3] };
                            plateau_jeu[Convert.ToInt32(adjacent_perpendiculaire_valide[i][1]) + j, Convert.ToInt32(adjacent_perpendiculaire_valide[i][2])].Mots.Add(element);
                        }
                        
                        
                    }
                    #endregion
                    #region direction d
                    if (adjacent_perpendiculaire_valide[i][3] == "d")
                    {
                        if (j != Convert.ToInt32(adjacent_perpendiculaire_valide[i][4]))
                        /*Teste si l'index actuel est égal à l'index de la lettre qui a été
                              rajoutée au mot existant pour former le nouveau mot présent dans 
                              la liste adjacent_mot_valide
                            Cette lettre n'est pas encore placée sur le plateau contrairement aux autres*/
                        {
                            int ligne = Convert.ToInt32(adjacent_perpendiculaire_valide[i][1]);
                            int colonne = Convert.ToInt32(adjacent_perpendiculaire_valide[i][2]) + j;
                            int index_ancien_mot = plateau_jeu[ligne, colonne].Recherche_Mot(adjacent_perpendiculaire_valide[i][0], adjacent_perpendiculaire_valide[i][3]);
                            /* Trouve l'index de l'ancien mot dans la liste de couples mot-direction de la position ligne colonne du plateau*/
                            if (plateau_jeu[ligne, colonne].Mots != null && plateau_jeu[ligne, colonne].Mots.Count > index_ancien_mot)
                            {
                                plateau_jeu[ligne, colonne].Mots[Convert.ToInt32(index_ancien_mot)][0] = adjacent_perpendiculaire_valide[i][0];
                            }
                               
                            /* Remplace l'ancien mot par le nouveau mot*/
                        }
                        else
                        {
                            string[] element = { adjacent_perpendiculaire_valide[i][0], adjacent_perpendiculaire_valide[i][3] };
                            plateau_jeu[Convert.ToInt32(adjacent_perpendiculaire_valide[i][1]), Convert.ToInt32(adjacent_perpendiculaire_valide[i][2]) + j].Mots.Add(element);
                        }
                    }
                    #endregion

                }
                Score(adjacent_perpendiculaire_valide[i][0], croisements, Convert.ToInt32(adjacent_perpendiculaire_valide[i][1]), Convert.ToInt32(adjacent_perpendiculaire_valide[i][2]), Convert.ToChar(adjacent_perpendiculaire_valide[i][3]), joueur, Convert.ToInt32(adjacent_perpendiculaire_valide[i][4]));
                joueur.Add_Mot(adjacent_perpendiculaire_valide[i][0]);

            }
        }
        /// <summary>
        /// Accomplit différentes action autour du mot spécifié à la position spécifié et plcé dans la direction spécifiée
        /// <br />
        /// <br />
        /// Actions accomplies : 
        /// <br />
        /// modifie la valeur mot de la liste de couples mot-direction pour chaque case où le mot est placé pour qu'elle corresponde au mot spécifié
        /// <br />
        /// Ajoute le score du mot au score du joueur spécifié
        /// <br />
        /// Ajoute le mot à la liste mot trouvés du joueur spécifié
        /// <br />
        /// <br />
        /// </summary>
        /// <returns></returns>
        public void Process_Mot_valide(string mot, int ligne, int colonne, char direction, Joueur joueur, List<int> croisements, string pth_jeton)
        {
            for (int j = 0; j < mot.Length; j++)
            {
                #region la lettre n'est pas déjà sur le plateau
                if (!croisements.GetRange(1, croisements.Count - 1).Contains(j))
                /* teste si l'index j du mot n'est pas dans la liste croisement
                 * Et donc si la lettre n'est pas déjà placée
                 Le premier index de croisement est un code d'erreur il est donc exclu par GetRange*/
                {
                    Jeton jeton = new Jeton(mot[j], pth_jeton);
                    if (direction == 'b')
                    {
                        plateau_jeu[ligne + j, colonne].Jeton_case = jeton;
                        string[] element_mots = { mot, Convert.ToString(direction) };
                        plateau_jeu[ligne + j, colonne].Mots.Add(element_mots);
                    }
                    if (direction == 'd')
                    {
                        plateau_jeu[ligne, colonne + j].Jeton_case = jeton;
                        string[] element_mots = { mot, Convert.ToString(direction) };
                        plateau_jeu[ligne , colonne+j].Mots.Add(element_mots);
                    }
                   
                    joueur.Remove_Main_Courante(jeton);
                }
                #endregion
                #region La lettre est déjà sur le plateau
                else
                {
                    if (direction == 'b')
                    {
                        string[] element_mots = { mot, Convert.ToString(direction) };
                        bool remplacer = false;
                        for(int i = 0; i< plateau_jeu[ligne + j, colonne].Mots.Count; i++) //Parcourt 
                        {
                            if (plateau_jeu[ligne + j, colonne].Mots[i][1].ToUpper() == Convert.ToString(direction).ToUpper())
                            {
                                plateau_jeu[ligne + j, colonne].Mots[i] = element_mots;
                                remplacer = true;
                            }
                        }
                        if (!remplacer)
                        {
                            plateau_jeu[ligne + j, colonne].Mots.Add(element_mots);
                        }
                    }
                    if (direction == 'd')
                    {
                        string[] element_mots = { mot, Convert.ToString(direction) };
                        bool remplacer = false;
                        for (int i = 0; i < plateau_jeu[ligne, colonne + j].Mots.Count; i++)
                        {
                            if (plateau_jeu[ligne , colonne + j].Mots[i][1].ToUpper() == Convert.ToString(direction).ToUpper())
                            {
                                plateau_jeu[ligne , colonne + j].Mots[i] = element_mots;
                                remplacer = true;
                            }
                        }
                        if (!remplacer)
                        {
                            plateau_jeu[ligne, colonne + j].Mots.Add(element_mots);
                        }
                    }
                }
                #endregion
            }
            joueur.Add_Mot(mot);
            
        }
        /// <summary>
        /// Calcule le score du mot spécifié à la position spécifiée en prenant en compte les bonus et ajoute le score au score du joueur spécifié
        /// <br />
        /// <br />
        /// </summary>
        /// <returns></returns>
        public void Score(string mot, List<int> croisements,int ligne, int colonne, int direction, Joueur joueur, int index = -1)
        {
            int score_mot = 0;
            int multiplicateur_final = 1;
            for (int i = 0; i < mot.Length; i++)
            {
                #region La lettre vient d'être ajoutée sur le plateau
                if ((!croisements.GetRange(1,croisements.Count-1).Contains(i) && index == -1) || (index != -1 && index == i)) 
                    /* La premiere partie du if teste dans le cas ou le mot a été placé en utilisant un croisement, la deuxième partie teste le cas ou  le mot 
                     * a été ajouté en complétant un mot déjà présent sans croisements
                     */
                {
                    
                    if (direction == 'b')
                    {
                        switch (plateau_jeu[ligne + i, colonne].Bonus)
                        {
                            case "Ve":
                                score_mot += plateau_jeu[ligne + i, colonne].Jeton_case.Score;
                                break;
                            case "Bc":
                                score_mot += 2*plateau_jeu[ligne + i, colonne].Jeton_case.Score;
                                break;
                            case "Bf":
                                score_mot += 3*plateau_jeu[ligne + i, colonne].Jeton_case.Score;
                                break;
                            case "Ro":
                                score_mot += plateau_jeu[ligne + i, colonne].Jeton_case.Score;
                                multiplicateur_final *= 2;
                                break;
                            case "Ru":
                                score_mot += plateau_jeu[ligne + i, colonne].Jeton_case.Score;
                                multiplicateur_final *= 3;
                                break;
                        }
                    }
                    if(direction == 'd')
                    {
                        switch(plateau_jeu[ligne, colonne + i].Bonus)
                        {
                            case "Ve":
                                score_mot += plateau_jeu[ligne, colonne + i].Jeton_case.Score;
                                break;
                            case "Bc":
                                score_mot += 2 * plateau_jeu[ligne, colonne + i].Jeton_case.Score;
                                break;
                            case "Bf":
                                score_mot += 3 * plateau_jeu[ligne, colonne + i].Jeton_case.Score;
                                break;
                            case "Ro":
                                score_mot += plateau_jeu[ligne, colonne + i].Jeton_case.Score;
                                multiplicateur_final *= 2;
                                break;
                            case "Ru":
                                score_mot += plateau_jeu[ligne, colonne + i].Jeton_case.Score;
                                multiplicateur_final *= 3;
                                break;
                        }
                    }
                }
                #endregion
                #region La lettre était déjà présente
                else
                {
                    if(direction == 'b')
                    {
                        score_mot += plateau_jeu[ligne + i, colonne].Jeton_case.Score;
                    }
                    if(direction == 'd')
                    {
                        score_mot += plateau_jeu[ligne, colonne + i].Jeton_case.Score;
                    }
                }
                #endregion
            }
            joueur.Add_Score(score_mot * multiplicateur_final);
        }
        /// <summary>
        /// Accomplit des tests sur le mot passé en paramètre à la position spécifiée
        /// Teste si la position est valide du mot est valide, si le mot appartient au dictionnaire et si les lettres composants le mot appartiennent à la main du joueur.
        /// <br />
        /// <br />
        /// Retourne : 
        /// <br />
        /// un int qui dépendant de l'état du test
        /// <br />
        /// Si le mot n'est pas le premier mot placé : 0 si la position est fausse, 1 si le mot n'appartient pas au dictionnaire, 2 si le test est valide et 3 si une lettre n'appartient pas à la main du joueur
        /// <br />
        /// Si le mot est le premier mot placé : 4 si la place sur le plateau n'est pas suffisante, 5 si la positions n'est pas 7;7, 6 si le mot n'appartient pas au dictionnaire, 7 si le test est valide et 8 si une lettre n'appartient pas à la main du joueur
        /// </summary>
        /// <returns></returns>
        public int Test_Plateau(string mot, int ligne, int colonne, char direction, Dictionnaire dico, Joueur joueur, List<int> croisements, bool premier_mot, List<string[]> Adjacent_valide_perpendiculaire)
        {
            int retour = 0;
            #region Le mot n'est pas le premier mot placé 
            if (!premier_mot)
            {
                if ((croisements[0] == 1 && croisements.Count > 1) || (croisements[0] == 1 && Adjacent_valide_perpendiculaire.Count >0))
                    /* 1er test : mot positionné en utilisant un croisement valide
                       2eme test : mot positionné en utilisant une complétion valide sans croisements*/
                {
                    retour = 1;
                    if (Dictionnaire.RechDichoRecursif(dico.Liste_RechRecursive(mot), mot))
                    {
                        retour = 2;
                        for (int i = 0; i < mot.Length && retour == 2; i++)
                        /* Vérifie que chaque lettre qui n'as pas de croisement 
                         * valide donc doit être placée appartient bien à la main du joueur*/
                        {
                            if (!croisements.GetRange(1, croisements.Count - 1).Contains(i))
                            {
                                if (joueur.Appartient_Main_Test(mot[i]) == -1)
                                {
                                    retour = 3;
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region Le mot est le premier mot placé
            else
            {
                retour = 4;
                if(croisements[0] == 1) // Test si la place est suffisante (inutile le test de position = 7;7 suffirait)
                {
                    retour = 5;
                    if (ligne == 7 && colonne == 7)
                    {
                        retour = 6;
                        if(Dictionnaire.RechDichoRecursif(dico.Liste_RechRecursive(mot), mot)) 
                        {
                            retour = 7;
                            for (int i = 0; i < mot.Length && retour == 7 ; i++)
                            /* Vérifie que chaque lettre qui n'as pas de croisement 
                             * valide donc doit être placée appartient bien à la main du joueur*/
                            {
                                if (!croisements.GetRange(1, croisements.Count - 1).Contains(i))
                                {
                                    if (joueur.Appartient_Main_Test(mot[i]) == -1)
                                    {
                                        retour = 8;
                                    }
                                }
                            }
                        }
                    }
                    
                }
                
            }
        #endregion
        return retour;
        }
        /// <summary>
        /// Accomplit des tests sur la position du mot passé en paramètre à la position spécifiée dans la direction spécifiée
        /// <br />
        /// Teste si la place sur le plateau est suffisante, et s'il y a des croisements non valides (une lettre est déjà placée la où on veut placer une des lettres du mot et les lettres sont différentes)
        /// <br />
        /// <br />
        /// Retourne : 
        /// <br />
        /// Une liste de int contennant en position 0 un int indiquant l'état du test et dans les autres positions les positions, dans le mot spécifié, des croisements valides
        /// L'entier en position 0 indique : Si la place est insuffisante ( valeur = -1 ) et s'il y a des croisements non valides ( valeur = -2 )
        /// </summary>
        /// <returns></returns>
        public List<int> Croisement_Valide(string mot, int ligne, int colonne, char direction)
        {
            List<int> valide = new List<int>();
            valide.Add(1);  
           
            if (direction == 'b')
            {
                if (plateau_jeu.GetLength(0)-ligne< mot.Length) { valide[0] = -1; }  //test si la place sur le plateau est suffisante
                for (int i = 0; i < mot.Length && valide[0] == 1; i++)
                {
                    if (plateau_jeu[ligne + i, colonne].Jeton_case.Lettre != ' ') 
                        /* Si la case n'est pas vide teste si la lettre sur la case et la lettre du mot sont différentes.
                         Si c'est le cas, il y a un croisement non valide et le test croisement valide s'arrète car le mot ne pourra pas être placé ici
                        Sinon ajoute la position de la lettre dans le mot testé à la liste retournée par le programme*/
                    {       
                        if (char.ToUpper(plateau_jeu[ligne + i, colonne].Jeton_case.Lettre) != char.ToUpper(mot[i])) { valide[0] = -2; }
                        else { valide.Add(i);  }
                    }
                }
            }
            if (direction == 'd')
            {
                if (plateau_jeu.GetLength(1)-colonne < mot.Length) { valide[0] = -1; }
                for (int i = 0; i < mot.Length && valide[0] == 1; i++)
                {
                    if (plateau_jeu[ligne, colonne + i].Jeton_case.Lettre != ' ') 
                    {
                        if (char.ToUpper(plateau_jeu[ligne, colonne+i].Jeton_case.Lettre) != char.ToUpper(mot[i])){ valide[0] = -2; }
                        else { valide.Add(i); }
                    }
                }
            } 
            
            return valide;
        }
        /// <summary>
        /// Teste si il existe des mots adjacents ( pas de croisements ) et perpendiculaires au mot défini par les paramètres qui soient valides
        /// <br />
        /// Pour cela, teste si un mot adjacent perpendiculaire existe et si la combinaison de ce mot et du mot en paramètre forment un mot appartenant au dictionnaire
        /// <br />
        /// <br />
        /// Retourne : 
        /// <br />
        /// Une liste de tableau définissant les différents mots adjacents et perpendiculaires valide
        /// <br />
        /// Un tableau contient dans l'ordre: (mot, ligne, colonne, direction, position dans le mot de la lettre ajoutée)
        /// <br />
        /// la position dans le mot de la lettre ajoutée est la position dans le nouveau mot retourné de la lettre du mot passé en paramètre qui a permis de former le nouveau mot
        /// </summary>
        /// <returns></returns>
        public List<string[]> Adjacent_perpendiculaire_valide(string mot, int ligne, int colonne, char direction,Dictionnaire dico)
        
        {
            List<string[]> retour = new List<string[]>();
            for(int i = 0;i < mot.Length; i++)
            {
                #region direction du mot placé droite
                if (direction == 'd')
                {
                    #region parcours des case au dessus du mot

                    /*Pour trouver les mots adjacents et perpendiculaire et vérifier si le nouveau mot formé par la lettre du mot placé et l'ancien mot 
                     appartient au dictionnaire*/
                    for (int j = 0; j < plateau_jeu[ligne - 1, colonne + i].Mots.Count; j++)
                    {
                        int plateau_null = 0;
                        if (plateau_jeu[ligne, colonne + i].Mots != null && plateau_jeu[ligne, colonne + i].Mots.Count>j)
                        /*Test que la liste de mot de la case ligne + i, colonne est 
                         * non nulle et contient plus de j élément pour pouvoir faire le test suivant*/
                        {
                            plateau_null = 1;
                            if (plateau_jeu[ligne, colonne + i].Mots[j][0] != plateau_jeu[ligne - 1, colonne + i].Mots[j][0])
                            {
                                plateau_null = 2;
                            }
                            /*Teste si la case à droite de la lettre à l'index j est différente de cette dernière 
                             Sinon il y a un croisement et le test adjacent_perpendiculaire_valide est donc faux*/
                        }
                        if (plateau_jeu[ligne - 1, colonne + i].Mots[j][0] != "" && plateau_null != 1 && plateau_jeu[ligne - 1, colonne + i].Mots[j][1] == "b")
                        {
                            string new_mot = plateau_jeu[ligne - 1, colonne + i].Mots[j][0] + mot[i];
                            if (Dictionnaire.RechDichoRecursif(dico.Liste_RechRecursive(new_mot), new_mot))
                            {
                                int index_ligne = ligne - plateau_jeu[ligne - 1, colonne + i].Mots[j][0].Length;
                                int index_colonne = colonne + i;
                                string[] element = { new_mot, Convert.ToString(index_ligne), Convert.ToString(index_colonne), Convert.ToString(plateau_jeu[ligne - 1, colonne + i].Mots[j][1]), Convert.ToString(new_mot.Length - 1)};
                                retour.Add(element);
                            }
                        }
                    }
                    #endregion
                    #region parcours des case au dessous du mot
                    /*Pour trouver les mots adjacents et perpendiculaire et vérifier si le nouveau mot formé par la lettre du mot placé et l'ancien mot 
                    appartient au dictionnaire*/
                    for (int j = 0; j< plateau_jeu[ligne + 1, colonne + i].Mots.Count; j++)
                    {
                        int plateau_null = 0;
                        if (plateau_jeu[ligne, colonne + i].Mots != null && plateau_jeu[ligne, colonne + i].Mots.Count >j)
                        /*Test que la liste de mot de la case ligne + i, colonne est 
                        * non nulle et contient plus de j élément pour pouvoir faire le test suivant*/
                        {
                            plateau_null = 1;
                            if (plateau_jeu[ligne, colonne + i].Mots[j][0] != plateau_jeu[ligne + 1, colonne + i].Mots[j][0])
                            {
                                plateau_null = 2;
                            }
                            /*Teste si la case à droite de la lettre à l'index j est différente de cette dernière 
                             Sinon il y a un croisement et le test adjacent_perpendiculaire_valide est donc faux*/
                        }
                        if (plateau_jeu[ligne + 1, colonne + i].Mots[j][0] != null && plateau_null != 1 && plateau_jeu[ligne + 1, colonne + i].Mots[j][1] == "b")
                        {
                            string new_mot = mot[i] + plateau_jeu[ligne + 1, colonne + i].Mots[j][0];
                            if (Dictionnaire.RechDichoRecursif(dico.Liste_RechRecursive(new_mot), new_mot))
                            {
                                int index_ligne = ligne; 
                                int index_colonne = colonne + i;
                                string[] element = { new_mot, Convert.ToString(index_ligne), Convert.ToString(index_colonne), Convert.ToString(plateau_jeu[ligne + 1, colonne + i].Mots[j][1]), Convert.ToString(0)};
                                retour.Add(element);
                            }
                        } 
                    }
                    #endregion

                }
                #endregion
                #region direction du mot pacé bas
                if (direction == 'b')
                {
                    #region Parcours les cases à la droite du mot
                    /*Pour trouver les mots adjacents et perpendiculaire et vérifier si le nouveau mot formé par la lettre du mot placé et l'ancien mot 
                    appartient au dictionnaire*/
                    for (int j = 0; j < plateau_jeu[ligne + i, colonne + 1].Mots.Count; j++)
                    {
                        int plateau_null = 0;
                        if (plateau_jeu[ligne + i, colonne].Mots != null && plateau_jeu[ligne + i, colonne].Mots.Count >j)
                        /*Test que la liste de mot de la case ligne + i, colonne est 
                         * non nulle et contient plus de j élément pour pouvoir faire le test suivant*/
                        {
                            plateau_null = 1;
                            if (plateau_jeu[ligne + i, colonne].Mots[j][0] != plateau_jeu[ligne + i, colonne + 1].Mots[j][0])
                            {
                                plateau_null = 2;
                            }
                            /*Teste si la case à droite de la lettre à l'index j est différente de cette dernière 
                             Sinon il y a un croisement et le test adjacent_perpendiculaire_valide est donc faux*/
                        }
                        if (plateau_jeu[ligne + i, colonne + 1].Mots[j][0] != "" &&  plateau_null != 1 && plateau_jeu[ligne + i, colonne + 1].Mots[j][1] == "d")
                        {
                            string new_mot = mot[i] + plateau_jeu[ligne +i, colonne + 1].Mots[j][0];
                            if (Dictionnaire.RechDichoRecursif(dico.Liste_RechRecursive(new_mot), new_mot))
                            {
                                int index_ligne = ligne + i; 
                                int index_colonne = colonne;
                                string[] element = { new_mot, Convert.ToString(index_ligne), Convert.ToString(index_colonne), Convert.ToString(plateau_jeu[ligne + i, colonne + 1].Mots[j][1]), Convert.ToString(0)};
                                retour.Add(element);
                            }
                        }
                    }
                    #endregion
                    #region Parcours les cases à la gauche du mot
                    /*Pour trouver les mots adjacents et perpendiculaire et vérifier si le nouveau mot formé par la lettre du mot placé et l'ancien mot 
                    appartient au dictionnaire*/
                    for (int j = 0; j< plateau_jeu[ligne + i, colonne - 1].Mots.Count; j++)
                    {
                        int plateau_null = 0;
                        if (plateau_jeu[ligne + i, colonne].Mots != null && plateau_jeu[ligne + i, colonne].Mots.Count >j) 
                        {
                            /*Test que la liste de mot de la case ligne + i, colonne est 
                             * non nulle et contient plus de j élément pour pouvoir faire le test suivant*/
                            plateau_null = 1;
                            if(plateau_jeu[ligne + i, colonne].Mots[j][0] != plateau_jeu[ligne + i, colonne - 1].Mots[j][0])
                            {
                                plateau_null = 2;
                            }
                            /*Teste si la case à droite de la lettre à l'index j est différente de cette dernière 
                             Sinon il y a un croisement et le test adjacent_perpendiculaire_valide est donc faux*/
                        }
                        if (plateau_jeu[ligne + i, colonne - 1].Mots[j][0] != null && plateau_null != 1 && plateau_jeu[ligne + i, colonne - 1].Mots[j][1] == "d")
                        {
                            string new_mot = plateau_jeu[ligne + i, colonne - 1].Mots[j][0] + mot[i];
                            if (Dictionnaire.RechDichoRecursif(dico.Liste_RechRecursive(new_mot), new_mot))
                            {
                                int index_ligne = ligne + i;
                                int index_colonne = colonne - plateau_jeu[ligne + i, colonne - 1].Mots[j][0].Length;
                                string[] element = { new_mot, Convert.ToString(index_ligne), Convert.ToString(index_colonne), Convert.ToString(plateau_jeu[ligne + i, colonne - 1].Mots[j][1]), Convert.ToString(new_mot.Length - 1) };
                                retour.Add(element);
                            }
                        }
                    }
                    #endregion

                }
                #endregion

            }
            return retour;
        }
    }
    

}
