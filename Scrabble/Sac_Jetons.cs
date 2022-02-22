using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scrabble
{
    public class Sac_Jetons
    {
        private List<Jeton> sac_lst = new List<Jeton>();
        private int taille_sac;

        /// <summary>
        /// Initialise une instance de la classe Sac_Jetons avec les jetons du fichier passé en paramètre et une taille de 102 jetons.
        /// <br />
        /// Utilisé lors de la création d'une instance de la classe Jeu sans sauvegarde
        /// <br />
        /// </summary>
        /// <returns></returns>
        public Sac_Jetons(string pth_input)
        {
            this.taille_sac = 102; // taille du sac : 102 jetons
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(pth_input); // on parcourt le fichier de jetons
                string s;
                string[] s_tab;
                
                do
                {
                    s = sr.ReadLine();
                    if (s != null)
                    {
                        s_tab = s.Split(';'); // on découpe au niveau des points virgules
                        sac_lst.Add(new Jeton(Convert.ToChar(s_tab[0].ToUpper()), Convert.ToInt32(s_tab[1]), Convert.ToInt32(s_tab[2]))); // on ajoute à la liste jeton aves ses variables
                        
                    }
                } while (s != null);
                sr.Close(); //On ferme le streamreader
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + e.Source);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.Source);
            }
        }

        /// <summary>
        /// Initialise une instance de la classe Sac_Jetons avec les jetons du fichier de sauvegarde (Sauvegarde.txt) et une taille qui correspond au nombre de jetons dans le sac sauvegardé.
        /// <br />
        /// Utilisé lors de la création d'une instance de la classe Jeu avec sauvegarde
        /// <br />
        /// </summary>
        /// <returns></returns>
        public Sac_Jetons()
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader("Sauvegarde.txt"); // on parcourt le fichier sauvegardé
                string s;
                string[] s_tab;
                for (int i = 0; i < 24; i++) // on saute les lignes pour arriver directement au sac de jetons
                {
                    sr.ReadLine();
                }
                do
                {
                    s = sr.ReadLine();
                    if (s != null)
                    {
                        s_tab = s.Split(';'); // on découpe au niveau des points virgules
                        sac_lst.Add(new Jeton(Convert.ToChar(s_tab[0].ToUpper()), Convert.ToInt32(s_tab[1]), Convert.ToInt32(s_tab[2]))); // on ajoute à la liste jeton aves ses variables

                    }
                } while (s != null);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (sr != null) { sr.Close(); }
                taille_sac = CalculTaille_Sac();
            }
        }

        /// <summary>
        /// Pioche un jeton dans le sac de jeton et retire ce jeton du sac au hasard
        /// <br />
        /// 
        /// <br />
        /// Retourne :  
        /// <br />
        /// Un jeton qui correspond au jeton pioché avec l'attribut nb_jetons égal à 1
        /// </summary>
        /// <returns></returns>
        public Jeton Pioche_Jeton(Random r)
        {
            int nb_picked = r.Next(0, taille_sac);
            int index_picked = ToSac_lst_index(nb_picked);
            Jeton retour = null;
            if(index_picked == -1)
            {
                Console.WriteLine("Erreur ToSac_lst_index (retour -1 )");
            }
            else
            {
                retour = new Jeton(sac_lst[index_picked], 1);
                if (sac_lst[index_picked].Nb_jetons > 1)
                {
                    sac_lst[index_picked].Retire_Un_Nombre();
                    taille_sac--;
                }
                else
                {
                    sac_lst.RemoveAt(index_picked);
                    taille_sac--;
                }

                
            }
            return retour;
        }
        public List<Jeton> Sac_lst
        {
            get { return sac_lst;}
        }
        /// <summary>
        /// Transforme un nombre passé en paramètre en un idex de la liste sac_lst
        /// <br />
        /// 
        /// <br />
        /// Retourne :  
        /// <br />
        /// Un int égal au nombre convertit en index ou -1 en cas d'erreur
        /// </summary>
        /// <returns></returns>
        private int ToSac_lst_index(int nombre)
        {
            for(int i =0;i < sac_lst.Count; i++)
            {
                nombre -= sac_lst[i].Nb_jetons;
                if(nombre < 0) { return i; }
            }
            return -1;
        }
        /// <summary>
        /// Calcule le nombre de jetons dans la liste sac_lst
        /// <br />
        /// 
        /// <br />
        /// Retourne :  
        /// <br />
        /// Un int égal au nombre de jetons dans la liste
        /// </summary>
        /// <returns></returns>
        private int CalculTaille_Sac()
        {
            int retour = 0;
            for(int  i = 0;i < sac_lst.Count; i++)
            {
                retour += sac_lst[i].Nb_jetons;
            }
            return retour;
        }  

        /// <summary>
        /// Crée un string représentant l'instance de Sac_Jetons spécifiée
        /// <br />
        /// 
        /// <br />
        /// Retourne :  
        /// <br />
        /// Retourne un string représentant l'instance de Sac_Jetons spécifiée
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string sac_str = "";
            for(int i = 0; i<sac_lst.Count; i++)
            {
                sac_str = sac_str + sac_lst[i].ToString() + "\n ";
            }
            return sac_str;
        }

    }
}
