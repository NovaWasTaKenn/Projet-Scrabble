using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace Scrabble
{
    class Jeu
    {
        private Dictionnaire dico;
        private Sac_Jetons sac_jetons;
        private Plateau plateau;
        private Joueur[] joueurs;
        private bool premier_mot;
        private int index_joueurs;
        private System.Timers.Timer tour;
        /* Classe Timer
             * Ce timer continue de compter en parrallèle du reste du programme et déclenche l'évènement Elapsed quand il atteint l'intervalle spécifié
             * Cela permmet de continuer a compter même lorsque le joueur saisie son mot et que l'instruction Console.ReadLine est lancée */
        private bool temps_écoulé;
        public Jeu(string pth_input_dico, string pth_input_sac, string pth_input_bonus)
        {
            this.dico = new Dictionnaire(pth_input_dico);
            this.sac_jetons = new Sac_Jetons(pth_input_sac);
            this.plateau = new Plateau(pth_input_bonus);
            this.joueurs = Initialisation_joueurs();
            this.premier_mot = true;
            this.index_joueurs = 0;

            #region création du timer 
            Console.WriteLine("Saisissez la durée disponible pour entrer un mot ");
            bool boucle = false;
            double entrée = 300000;
            do
            {
                boucle = false;
                try
                {
                    entrée = Convert.ToDouble(Console.ReadLine());
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    boucle = true;
                }
            } while (boucle);
            int intervalle = (int) (entrée * 60000d);
            tour = new System.Timers.Timer(intervalle);
            /* Creation d'une instance de la classe timer avec pour paramètre intervalle
             * Ce timer continue de compter en parrallèle du reste du programme et déclenche l'évènement Elapsed quand il atteint l'intervalle spécifié
             * Cela permmet de continuer a compter même lorsque le joueur saisie son mot et que l'instruction Console.ReadLine est lancée 
                                                                        */
            tour.Elapsed += new System.Timers.ElapsedEventHandler(Tour_Elapsed);
            /* Abonne la méthode Tour_Elapsed à l'évènement Elapsed pour déclencher la méthode 
             * Tour_Elapsed lorsque le timer atteint le temps défini*/
            this.temps_écoulé = false;
            #endregion
        }
        public Jeu(string pth_input_dico, string pth_input_jetons, string pth_input_bonus, bool sauvegarde)
        {
            this.dico = new Dictionnaire(pth_input_dico);
            this.sac_jetons = new Sac_Jetons();
            #region création du timer
            Console.WriteLine("Saisissez la durée disponible pour entrer un mot ");
            bool boucle = false;
            double entrée = 300000;
            do
            {
                boucle = false;
                try
                {
                    entrée = Convert.ToDouble(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    boucle = true;
                }
            } while (boucle);
            int intervalle = (int) (entrée * 60000d);
            tour = new System.Timers.Timer(intervalle);
            /* Creation d'une instance de la classe timer avec pour paramètre intervalle
             * Ce timer continue de compter en parrallèle du reste du programme et déclenche l'évènement Elapsed quand il atteint l'intervalle spécifié
             * Cela permmet de continuer a compter même lorsque le joueur saisie son mot et que l'instruction Console.ReadLine est lancée 
                                                                        */
            tour.Elapsed += new System.Timers.ElapsedEventHandler(Tour_Elapsed);
            /* Abonne la méthode Tour_Elapsed à l'évènement Elapsed pour déclencher la méthode 
             * Tour_Elapsed lorsque le timer atteint le temps défini*/
            this.temps_écoulé = false;
            #endregion 
            this.plateau = new Plateau(pth_input_bonus, pth_input_jetons );
            List<Joueur> joueurs_lst = new List<Joueur>();
            #region Initialisation Joueurs et variables
            StreamReader sr = null;

            try
            {
                sr = new StreamReader("Sauvegarde.txt");
                string s;
                string[] s_tab;
                sr.ReadLine();
                premier_mot = Convert.ToBoolean(sr.ReadLine());
                index_joueurs = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < 17; i++)
                {
                    sr.ReadLine();
                }
                s = sr.ReadLine();
                if (s != null && s != "Mains_joueurs")
                {
                    s_tab = s.Split('_');
                    for(int k = 0; k<s_tab.Length; k++)
                    {
                        string[] joueur_tab = s_tab[k].Split(';');
                        
                        Joueur joueur_tmp = new Joueur(joueur_tab[0]);
                        joueur_tmp.Add_Score(Convert.ToInt32(joueur_tab[1]));
                        string[] mots_trouvés_tab = joueur_tab[2].Split('|');
                        for (int j = 0; j < mots_trouvés_tab.Length; j++)
                        {
                            joueur_tmp.Add_Mot(mots_trouvés_tab[j]);
                        }
                        joueurs_lst.Add(joueur_tmp);
                    }
                } 
                joueurs = new Joueur[joueurs_lst.Count];
                joueurs_lst.CopyTo(joueurs);
                s = sr.ReadLine();

                s = sr.ReadLine();
                if (s != null && s != "Sac_jetons")
                {
                    string[] mains = s.Split(';');
                    for(int i = 0; i < mains.Length; i++)
                    {
                        string[] jetons = mains[i].Split('|');
                        for(int j =0; j< jetons.Length; j++)
                        {
                            string[] lettre_nombre = jetons[j].Split('_');

                            Jeton jeton_tmp = new Jeton(Convert.ToChar(lettre_nombre[0]), Convert.ToInt32(lettre_nombre[1]), Convert.ToInt32(lettre_nombre[2]));
                            joueurs[i].Add_Main_Courante(jeton_tmp);
                        }
                    }
                }
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
            finally
            {
                if(sr != null) { sr.Close(); }
            }
            #endregion
           
        }
        #region Propriétées
        public System.Timers.Timer Tour
        {
            get { return tour; }  
        }
        public bool Temps_écoulé
        {
            get { return temps_écoulé; }
            set { temps_écoulé = value; }
        }
        public Dictionnaire Dico
        {
            get { return dico; }
        }
        public Sac_Jetons Sac_jetons
        {
            get { return sac_jetons; }
        }
        public Plateau Plateau
        {
            get { return plateau; }
        }
        public Joueur[] Joueurs
        {
            get { return joueurs; }
        }
        public bool Premier_mot
        {
            get { return premier_mot; }
            set { premier_mot = value; }
        }
        public int Count_joueurs
        {
            get { return index_joueurs; }
            set { index_joueurs = value; }
        }
        #endregion
        /// <summary>
        /// Sauvegarde :
        /// <br />
        /// - du plateau, 
        /// <br />
        /// - des joueurs,
        /// <br />
        /// - des mots trouvés, 
        /// <br />
        /// - des mains des joueurs,
        /// <br />
        /// - du sac de jeton.
        /// </summary>
        /// <returns></returns>
        public void Sauvegarde(bool mot_placé)
        {


            StreamWriter save = new StreamWriter("Sauvegarde.txt");

            save.WriteLine("Variables");
            save.WriteLine(premier_mot);
            if (mot_placé)
            {
                index_joueurs++;
                if (index_joueurs == joueurs.Length) { index_joueurs = 0; }
            }
            save.WriteLine(index_joueurs);
            save.WriteLine("plateau");
            string ligne;
            for(int i = 0; i < plateau.Plateau_jeu.GetLength(0); i++)
            {
                ligne = "";
                for(int j = 0; j < plateau.Plateau_jeu.GetLength(1); j++)
                {
                    ligne = ligne + plateau.Plateau_jeu[i, j].ToString(); // chaque case est mise dans une ligne, avec case en string
                    if(j != plateau.Plateau_jeu.GetLength(1) - 1)
                    {
                        ligne = ligne + "_"; //toutes les case avec un tiret de séparations
                    }
                }
                save.WriteLine(ligne); // on écrit la ligne sur la sauvegarde 
            }
            save.WriteLine("joueurs");
            string joueurs_str = "";
            for(int i = 0; i< joueurs.Length; i++)
            {
                string mots_trouvés = ""; // liste de mots que le joueur a trouvé
                if(joueurs[i].Mot_trouvés != null)
                {
                    for (int j = 0; j < joueurs[i].Mot_trouvés.Count; j++)
                    {
                        mots_trouvés = mots_trouvés + joueurs[i].Mot_trouvés[j];
                        if (j != joueurs[i].Mot_trouvés.Count - 1)
                        {
                            mots_trouvés = mots_trouvés + "|";
                        }
                    }
                }
                joueurs_str = joueurs_str + joueurs[i].Nom + ";" + joueurs[i].Score + ";" + mots_trouvés;
                if(i != joueurs.Length - 1)
                {
                    joueurs_str = joueurs_str + "_";
                }
            }
            save.WriteLine(joueurs_str);
            save.WriteLine("Mains_joueurs");

            string ligne_main = "";
            for(int j = 0; j< joueurs.Length; j++)
            {
               
                string main_courante = "";
                for (int i = 0; i < joueurs[j].Main_courante.Count; i++)
                {
                    main_courante = main_courante + joueurs[j].Main_courante[i].Lettre + "_" + joueurs[j].Main_courante[i].Score +"_"+ joueurs[j].Main_courante[i].Nb_jetons ;
                    if(i != joueurs[j].Main_courante.Count - 1) { main_courante = main_courante + "|"; }
                }
                ligne_main = ligne_main + main_courante;
                if(j != joueurs.Length - 1) { ligne_main = ligne_main + ";"; }
                
            }
            save.WriteLine(ligne_main);
            save.WriteLine("Sac_jetons");
            for(int i = 0; i< sac_jetons.Sac_lst.Count; i++)
            {
                save.WriteLine(sac_jetons.Sac_lst[i].ToString());
            }
            save.Close();
        }

        /// <summary>
        /// Permet l'initialisation de tous les joueurs (nombre de joueurs, pseudos, + toutes les vérifications à faire)
        /// <br />
        /// Retourne : 
        /// <br />
        /// Retourne les infos des joueurs dans le tableau
        /// </summary>
        /// <returns></returns>
        public Joueur[] Initialisation_joueurs()
        {
            bool nb_joueurs_valide = false;
            Joueur[] joueurs = null;
            int nb_joueurs = 2;
            do
            {
                bool sortie = true;
                do
                {
                    sortie = true;
                    try
                    {
                        Console.WriteLine("Saisissez le nombre de joueurs");
                        nb_joueurs = Convert.ToInt32(Console.ReadLine());

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine();
                        Console.WriteLine("         Appuyez sur entrée pour continuer");
                        Console.ReadKey();
                        Console.Clear();
                        sortie = false;
                    }
                } while (!sortie);


                if (nb_joueurs <= 4 && nb_joueurs > 0)
                {
                    joueurs = new Joueur[nb_joueurs];
                    for (int i = 0; i < joueurs.Length; i++)
                    {
                        bool joueur_valide = false;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("Saisir le pseudo du joueur " + (i + 1));
                            string joueur = Console.ReadLine();
                            if (!joueur.Contains('_') && !joueur.Contains('|') && !joueur.Contains(';') && !joueur.Contains(',') && !joueur.Contains(' ') && !joueur.Contains('-'))
                            {
                                joueur_valide = true;
                                joueurs[i] = new Joueur(joueur);
                            }
                            else
                            {
                                Console.WriteLine("Le nom du joueur ne doit pas contenir de carctère spéciaux comme ( _  ; |  ,  -)" +
                                    "                                                           Appuyer sur entrée pour contiuer");
                                Console.ReadKey();
                                Console.Clear();
                            }
                           
                        } while (!joueur_valide);
                        
                    }
                    nb_joueurs_valide = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Le nombre de joueurs doit être compris entre 1 et 4");
                    Thread.Sleep(1000);
                }

                Console.Clear();
            } while (!nb_joueurs_valide);
            return joueurs;
        }
        /// <summary>
        /// Méthode liée à l'évènement Elapsed du timer Tour, permet d'arrêter la saisie du mot si le temps alloué est écoulé 
        /// <br />
        /// </summary>
        /// <returns></returns>
        public void Tour_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tour.Stop();
            temps_écoulé = true;
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Le temps alloué pour placer votre mot est écoulé ");
            Console.WriteLine("Appuyez sur entrée pour continuer");
        }
    }
}
