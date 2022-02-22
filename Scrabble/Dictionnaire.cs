using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Scrabble
{
    public class Dictionnaire
    {
        private int length = 0;
        private List<List<string>> dico; 
        private string langue;
        private string pth_dico;
        private int smallest_length;
        public Dictionnaire(string pth_input)
        {
            this.pth_dico = pth_input;
            bool stop = false;
            int debut = -1;
            for (int i = pth_input.Length - 1; i >= 0 && !stop; i--)
            {
                if (pth_input[i] == '\u005C') { debut = i; stop = true; }
            }
            if (debut == -1)
            {
                this.langue = pth_input.Substring(0 , pth_input.Length - 4);
            }
            else { this.langue = pth_input.Substring(debut + 1, pth_input.Length - 5 - debut); }
            #region Remplissge liste dico
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(pth_input);
                dico = new List<List<string>>();
                List<string> groupe_mot = new List<string>();
                string s;
                string[] s_tab;
                int i = -1;
                do
                { 
                    
                    s = sr.ReadLine();
                    if(s == null)
                    {
                        length += groupe_mot.Count; dico.Add(new List<string>(groupe_mot)); groupe_mot.Clear();
                    }
                    else
                    {
                        if (Int32.TryParse(s, out int n))
                        {
                            if (i != -1) { length += groupe_mot.Count; dico.Add(new List<string>(groupe_mot)); groupe_mot.Clear(); }
                            else { smallest_length = n; }
                            i++;

                        }
                        else
                        {
                            s_tab = s.Split(' ');
                            for (int j = 0; j < s_tab.Length; j++)
                            {
                                groupe_mot.Add(s_tab[j]);
                            }
                            
                        }
                    }
                    
                    
                        

                    
                } while (s != null);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (sr != null) { sr.Close(); }
            }
            #endregion
        }

        /// <summary>
        /// retourne un string qui affiche les informations importantes à connaitre sur le dictionnaire
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Dictionnaire " + langue + " contennant : " + length + " mots";
        }

        public List<List<string>> Dico
        {
            get { return dico; }
        }

        /// <summary>
        /// Affichage de l'ensemble du dictionnaire 
        /// </summary>
        /// <returns></returns>
        public void Affichage_Dico()
        {
            for (int j = 0; j < dico.Count; j++)
            {
                Console.WriteLine(j);
                Afficher_liste(dico[j]);
            }
        }

        /// <summary>
        /// Recherche dichiotomique recursive qui permet de comparer notre mot en fonction du mot qui est au milieu par ordre alphabétique
        /// </summary>
        /// <returns></returns>
        public static bool RechDichoRecursif(List<string> liste,string mot)
        {
            int milieu = (liste.Count) / 2;
            if (0 > liste.Count-1) return false; // détecte la fin
            if (liste[milieu].ToLower() == mot.ToLower()) return true; 
            if (String.Compare(mot, liste[milieu], true) < 0) return RechDichoRecursif(liste.GetRange(0, milieu), mot) ;
            return RechDichoRecursif(liste.GetRange(milieu+1, liste.Count-(milieu+1)), mot);
        }

        /// <summary>
        /// Retourne la liste à traiter dans la recherche récursive 
        /// </summary>
        /// <returns></returns>
        public List<string> Liste_RechRecursive(string mot)
        {
            return dico[mot.Length - smallest_length];
        }

        /// <summary>
        /// Affichage des listes
        /// </summary>
        /// <returns></returns>
        private static void Afficher_liste(List<string> liste)
        {
            for (int i = 0; i < liste.Count; i++)
            {
                Console.WriteLine(liste[i]);
            }
        }
    }
}
