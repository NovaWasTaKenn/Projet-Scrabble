using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace Scrabble
{
    class Program
    {
        
        static void Main(string[] args)
        {
           
            #region Jeu

            
            
            #region Titre
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("                                                ____________");
            Console.WriteLine("                                                |          |    ");
                Console.Write("                                                |");
            Console.ResetColor();
                                                                         Console.Write(" SCRABBLE ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("|\n");
            Console.WriteLine("                                                |__________|    ");
            Console.ResetColor();
            Thread.Sleep(600);
            Console.Clear();
            #endregion

            #region Règles
            Console.WriteLine();
            Console.WriteLine("Tout d'abord, un petit rappel des règles.");
            Console.WriteLine();
            Console.WriteLine("Cependant, si vous les connaissez déjà vous pouvez passer en appuyant sur espace sinon appuyez sur entrée");
            if(Console.ReadKey().Key != ConsoleKey.Spacebar)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Dans le jeu du Scrabble, vous devez placer des jetons " +
                    "sur un plateau pour former des mots.\n" +
                    "Chaque jeton rapporte un certain nombre de points ce qui permet de " +
                    "déterminer le score.");
                Console.WriteLine();
                Console.WriteLine("Plateau :\n" +
                    " - Le plateau de jeu est un tableau de 15x15 cases, chaque case a un bonus qui permet d'augmenter votre score." +
                    "");
                Console.WriteLine();
                Console.WriteLine("Bonus :\n" +
                    " - Chaque bonus ne compte que pour le premier mot placé dessus.\n" +
                    " - Il y différents types de bonus représentés sur le plateau par des couleurs : \n" +
                    "   - Le vert représente une case sans bonus\n" +
                    "   - le bleu clair représente le bonus lettre compte double\n" +
                    "   - le bleu foncé représente le bonus lettre compte triple\n" +
                    "   - le rose représente le bonus mot compte double\n" +
                    "   - le rouge représente le bonus mot compte triple\n");
                Console.WriteLine("Jetons : \n" +
                    " - Le sac de jeton contient 102 jetons\n" +
                    " - Parmis ces jetons, on trouve les 26 lettres de l'alphabet et 2 jetons joker qui valent 0 points\n" +
                    " - Pour utiliser un joker vous devez en avoir un dans votre main\n" +
                    " - Lors de l'entrée du mot, entrez le mot que vous voulez placer.\n" +
                    "Si vous avez suffisamment de joker le programme utilisera vos jokers à la place des lettres que vous n'avez pas");
                Console.WriteLine();
                Console.WriteLine("Déroulement d'un tour : ");
                Console.WriteLine(" - Vous devez tout d'abord tirer des jetons de manière à avoir 7 jetons dans votre main.\n" +
                    "Le programme fait cela automatiquement, vous n'avez rien à faire !\n" +
                    " - Vous devez ensuite rentrer un mot, sa position et sa direction.\n" +
                    " - Si la position et le mot sont valides, le programme compte le score du mot et l'ajoute à votre score.\n" +
                    " - Le tour passe ensuite au prochain joueur.\n" +
                    " - Vous pouvez accéder à un menu lors du placement du mot ou à la fin de chaque tour.\n" +
                    " - Vous pouvez sauvegarder le jeu en accédant à ce menu");
                Console.WriteLine();
                Console.WriteLine("Conditions de validité du mot : \n" +
                    " - Pour que le mot soit considéré comme valide, il faut que : \n" +
                    @"    - Le mot appartienne au dictionnaire inclus dans le jeu (Le fichier bin\debug\netcoreapp3.1\Francais.txt)" + "\n"+
                    "    - Soit le mot croise un autre mot de manière valide \n      (Une ou plusieurs des lettres du nouveau mot sont déjà placées et appartiennent à un autre mot)\n" +
                    "    - Soit le mot est placé perpendiculairement au début ou à la fin d'un mot et complète ce mot existant pour former un mot valide\n" +
                    "    - Chaque lettre qui doit être placée sur le plateau doit être dans votre main");
                Console.WriteLine();
                Console.WriteLine("Fin de partie : \n" +
                    " - La partie se termine quand le sac de jeton est vide ou  si vous décidez d'arrêter le jeu à partir du menu\n" +
                    " - Le joueur ayant le score le plus élevé a gagné.");

                Console.WriteLine();
                Console.WriteLine("Pour fermer les règles et continuer apuyez sur une touche");
                Console.ReadKey();
                Console.Clear();
            }
            #endregion

            #region variables
            string pth_jeton = "Jetons.txt";
            bool stop = false;
            bool Menu_Continuer_Tour;
            bool mot_placé;
            bool sauvegarde = false;
            Random rnd = new Random();
            Jeu jeu = null;
            #endregion

            #region Creation de la partie
            StreamReader sr = new StreamReader("Sauvegarde.txt");
            if (sr.ReadLine() != null)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Une sauvegarde d'une partie a été trouvée. Voulez vous charger cette sauvegarde ?" +
                    "                                                                oui : o             non : n");
                char entrée = Console.ReadKey().KeyChar;   
                Thread.Sleep(500);
                Console.Clear();
                if (entrée == 'o')
                {
                    
                    sr.Close();
                    jeu = new Jeu("Francais.txt", pth_jeton, "Bonus.txt", true);
                }
                else
                {
                    jeu = new Jeu("Francais.txt", pth_jeton, "Bonus.txt");
                }
            }
            else
            {
                jeu = new Jeu("Francais.txt", pth_jeton, "Bonus.txt");
            }
            sr.Close();
            #endregion


           
                    
            while (jeu.Sac_jetons.Sac_lst.Count > 0 && !stop)
            {
                Menu_Continuer_Tour = false;
                mot_placé = false;


                Console.Clear();
                #region Affichage tableau, joueur
                    
                jeu.Plateau.Affichage_plateau();
                Console.WriteLine();
                Console.WriteLine("Tour de : " + jeu.Joueurs[jeu.Count_joueurs].Nom);
                #endregion

                #region pioche
                int main_count = jeu.Joueurs[jeu.Count_joueurs].Main_Count();
                if (main_count<7)
                {
                    Console.WriteLine("Pioche . . .");
                    
                    
                    for (int i = 0; i < 7 - main_count && jeu.Sac_jetons.Sac_lst.Count != 0; i++)
                    {
                        Jeton jeton = jeu.Sac_jetons.Pioche_Jeton(rnd);
                        jeu.Joueurs[jeu.Count_joueurs].Add_Main_Courante(jeton);
                    }
                    Thread.Sleep(1100);
                    
                }

                #endregion

           #region entrée du mot, tests, process du mot
                int tests;
                do
                {

                    #region entrée mot
                    string mot = "";
                    bool sortie = false;
                    do
                    {

                        try
                        {
                            #region changement de l'affichage
                            Console.Clear();
                            jeu.Plateau.Affichage_plateau();
                            Console.WriteLine();
                            Console.WriteLine("Tour de : " + jeu.Joueurs[jeu.Count_joueurs].Nom);
                            Console.WriteLine();
                            #endregion
                            Console.WriteLine("Votre main :");
                            Console.WriteLine(jeu.Joueurs[jeu.Count_joueurs].Affichage_Main());
                            Console.WriteLine();
                            Console.WriteLine("Saisissez le mot que vous voulez placer ou appuyez sur entrée pour accéder au menu");
                            jeu.Temps_écoulé = false;
                            jeu.Tour.Start();
                            mot = Console.ReadLine();
                            jeu.Tour.Stop();
                            sortie = true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    } while (!sortie);
                    
                    #endregion
                    if (mot != "" && !jeu.Temps_écoulé)
                    {
                        #region entrée position direction
                        int ligne = 0;
                        int colonne = 0;
                        char direction = 'd';
                        try
                        {
                                
                            #region Changement de l'affichage
                            Console.Clear();
                            jeu.Plateau.Affichage_plateau();
                            Console.WriteLine();
                            Console.WriteLine("Tour de : " + jeu.Joueurs[jeu.Count_joueurs].Nom);
                            Console.WriteLine();
                            Console.WriteLine("Vous placez le mot : " + mot);
                            Console.WriteLine();
                            #endregion
                            bool boucle = false;
                            do
                            {
                                boucle = false;
                                try
                                {
                                    Console.WriteLine("Saisissez l'index des lignes");
                                    ligne = Convert.ToInt32(Console.ReadLine());
                                    Console.WriteLine("Saisissez l'index des colonnes");
                                    colonne = Convert.ToInt32(Console.ReadLine());
                                    if(ligne <0 || colonne < 0) { Console.WriteLine("La ligne et la colonne doivent être positive"); boucle = true; }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    boucle = true;
                                }
                            } while (boucle);
                            

                                
                            #region Changement de l'affichage
                            Console.Clear();
                            Console.WriteLine();
                            jeu.Plateau.Affichage_plateau();
                            Console.WriteLine();
                            Console.WriteLine("Tour de : " + jeu.Joueurs[jeu.Count_joueurs].Nom);
                            Console.WriteLine();
                            Console.WriteLine("Vous placez le mot : " + mot + " à la position : ligne " + ligne + "; colonne " + colonne);
                            Console.WriteLine();
                            #endregion
                            do
                            {
                                boucle = false;
                                try
                                {
                                    Console.WriteLine("Saisissez la direction dans laquelle vous voulez placer le mot \n (d: vers la droite, b: vers le bas");
                                    direction = Convert.ToChar(Console.ReadKey().KeyChar);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    boucle = true;
                                }
                            } while (boucle);
                            

                            #region Changement de l'affichage
                            Console.Clear();
                            Console.WriteLine();
                            jeu.Plateau.Affichage_plateau();
                            Console.WriteLine();
                            Console.WriteLine("Tour de : " + jeu.Joueurs[jeu.Count_joueurs].Nom);
                            Console.WriteLine();
                            string direction_affichage = "";
                            switch (direction)
                            {
                                case 'd':
                                    direction_affichage = "la droite";
                                    break;
                                case 'b':
                                    direction_affichage = "le bas";
                                    break;
                            }
                            Console.WriteLine("Vous placez le mot " + mot + " à la position : ligne " + ligne + "; colonne " + colonne+" vers "+direction_affichage);
                            Console.WriteLine();
                            #endregion

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        #endregion


                        #region tests et process du mot   //Pour bien faire mettre ca dans une fonction
                        List<int> croisements = jeu.Plateau.Croisement_Valide(mot, ligne, colonne, direction);
                        List<string[]> adjacent_perpendiculaire_valide = jeu.Plateau.Adjacent_perpendiculaire_valide(mot, ligne, colonne, direction, jeu.Dico);
                        tests = jeu.Plateau.Test_Plateau(mot, ligne, colonne, direction, jeu.Dico, jeu.Joueurs[jeu.Count_joueurs], croisements, jeu.Premier_mot, adjacent_perpendiculaire_valide);

                        if (tests == 2 || tests == 7)
                        {
                            Console.WriteLine("Mot valide");                                                                                                          
                            jeu.Plateau.Process_Mot_valide(mot, ligne, colonne, direction, jeu.Joueurs[jeu.Count_joueurs], croisements, pth_jeton);

                                
                            jeu.Plateau.Score(mot, croisements, ligne, colonne, direction, jeu.Joueurs[jeu.Count_joueurs]);
                            if(adjacent_perpendiculaire_valide.Count > 0) 
                            {
                                jeu.Plateau.Process_Mot_Adjacent(adjacent_perpendiculaire_valide, jeu.Joueurs[jeu.Count_joueurs], croisements);
                            }
                            if (jeu.Premier_mot) { jeu.Premier_mot = false; }
                            mot_placé = true;
                            Thread.Sleep(700);
                            #region Refresh affichage
                            Console.Clear();
                            jeu.Plateau.Affichage_plateau();
                            Console.WriteLine();
                            Console.WriteLine("Tour de : " + jeu.Joueurs[jeu.Count_joueurs].Nom);
                            Console.WriteLine();
                            string direction_affichage = "";
                            switch (direction)
                            {
                                case 'd':
                                    direction_affichage = "la droite";
                                    break;
                                case 'b':
                                    direction_affichage = "le bas";
                                    break;
                            }
                            Console.WriteLine("Vous placez le mot " + mot + " à la position : ligne " + ligne + "; colonne " + colonne + " vers " + direction_affichage);
                            Console.WriteLine();
                            #endregion
                        }
                        else
                        {
                            switch (tests)
                            {
                                case 0:
                                    Console.WriteLine("La position du mot n'est pas valide ( code erreur : " + tests + ")");
                                    break;
                                case 1:
                                    Console.WriteLine("Le mot n'appartient pas au dictionnaire ( code erreur : " + tests + ")");
                                    break;
                                case 4:
                                    Console.WriteLine("Le mot est trop long pour l'emplacement selectionné ou il y a un croisement invalide ( code erreur : " + tests + ")");
                                    break;
                                case 3:
                                    Console.WriteLine("Les lettres requises ne sont pas dans votre main ( code erreur : " + tests + ")");
                                    break;
                                case 6:
                                    Console.WriteLine("Le mot n'appartient pas au dictionnaire ( code erreur : " + tests + ")");
                                    break;
                                case 5:
                                    Console.WriteLine("La position de départ du premier mot doit être 7;7 ( code erreur : " + tests + ")");
                                    break;
                                case 8:
                                    Console.WriteLine("Les lettres requises ne sont pas dans votre main ( code erreur : " + tests +")");
                                    break;
                            }
                            Console.WriteLine("Appuyez sur une touche pour continuer");
                            Console.ReadKey();
                        }
                        #endregion


                    }
                    else
                    {
                        if(jeu.Temps_écoulé && mot != "")
                        {
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine("N'ESSAYEZ PAS DE TRICHER !!!");
                            Thread.Sleep(700);
                        }
                        tests = 2;
                    }
                } while (tests != 2 && tests != 7);
                #endregion


                #region Menu
                bool sortie_menu = false;
                do
                {

                    #region Affichage tableau, joueur
                    Console.Clear();
                    jeu.Plateau.Affichage_plateau();
                    Console.WriteLine();
                    Console.WriteLine("Tour de : " + jeu.Joueurs[jeu.Count_joueurs].Nom);
                    #endregion
                    Console.WriteLine();
                    Console.WriteLine("Continuer le tour : 1 \nPasser le tour : 2 \nQuitter sans sauvegarder : 3 \nQuitter et sauvegarder : 4  \nAfficher les scores et les mots trouvés : 5 \nAfficher les règles : 6\n");
                    char entrée = Console.ReadKey(true).KeyChar;
                    switch (entrée)
                    {
                        case '1':
                            if (mot_placé || jeu.Temps_écoulé)
                            {
                                Menu_Continuer_Tour = false;
                            }
                            else
                            {
                                Menu_Continuer_Tour = true;
                            }
                            sortie_menu = true;
                            break;
                        case '2':
                            Menu_Continuer_Tour = false;
                            sortie_menu = true;
                            break;
                        case '3':
                            stop = true;
                            sortie_menu = true;
                            break;
                        case '4':
                            stop = true;
                            sortie_menu = true;
                            sauvegarde = true;
                            jeu.Sauvegarde(mot_placé);
                            break;
                        case '5':
                            #region Changement de l'affichage
                            Console.Clear();
                            jeu.Plateau.Affichage_plateau();
                            Console.WriteLine();
                            Console.WriteLine("Tour de : " + jeu.Joueurs[jeu.Count_joueurs].Nom);
                            Console.WriteLine();

                            #endregion
                            Console.WriteLine("Voici les mots trouvés par les joueurs et leurs scores");
                            Console.WriteLine();
                            for (int i = 0; i < jeu.Joueurs.Length; i++)
                            {
                                Console.WriteLine(jeu.Joueurs[i].ToString());
                                Console.WriteLine();
                            }

                            Console.WriteLine("Appuyer sur entrée pour retourner au menu");
                            Console.ReadKey(true);
                            break;
                        case '6':
                            #region règles
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Dans le jeu du Scrabble, vous devez placer des jetons " +
                                "sur un plateau pour former des mots.\n" +
                                "Chaque jeton rapporte un certain nombre de points ce qui permet de " +
                                "déterminer le score.");
                            Console.WriteLine();
                            Console.WriteLine("Plateau :\n" +
                                " - Le plateau de jeu est un tableau de 15x15 cases, chaque case a un bonus qui permet d'augmenter votre score." +
                                "");
                            Console.WriteLine();
                            Console.WriteLine("Bonus :\n" +
                                " - Chaque bonus ne compte que pour le premier mot placé dessus.\n" +
                                " - Il y différents types de bonus représentés sur le plateau par des couleurs : \n" +
                                "   - Le vert représente une case sans bonus\n" +
                                "   - le bleu clair représente le bonus lettre compte double\n" +
                                "   - le bleu foncé représente le bonus lettre compte triple\n" +
                                "   - le rose représente le bonus mot compte double\n" +
                                "   - le rouge représente le bonus mot compte triple\n");
                            Console.WriteLine();
                            Console.WriteLine("Déroulement d'un tour : ");
                            Console.WriteLine(" - Vous devez tout d'abord tirer des jetons de manière à avoir 7 jetons dans votre main.\n" +
                                "Le programme fait cela automatiquement, vous n'avez rien à faire !\n" +
                                " - Vous devez ensuite rentrer un mot, sa position et sa direction.\n" +
                                " - Si la position et le mot sont valides, le programme compte le score du mot et l'ajoute à votre score.\n" +
                                " - Le tour passe ensuite au prochain joueur.\n" +
                                " - Vous pouvez accéder à un menu lors du placement du mot ou à la fin de chaque tour.\n" +
                                " - Vous pouvez sauvegarder le jeu en accédant à ce menu");
                            Console.WriteLine();
                            Console.WriteLine("Conditions de validité du mot : \n" +
                                " - Pour que le mot soit considéré comme valide, il faut que : \n" +
                                @"    - Le mot appartienne au dictionnaire inclus dans le jeu (Le fichier bin\debug\netcoreapp3.1\Francais.txt)" + "\n" +
                                "    - Soit le mot croise un autre mot de manière valide \n      (Une ou plusieurs des lettres du nouveau mot sont déjà placées et appartiennent à un autre mot)\n" +
                                "    - Soit le mot est placé perpendiculairement au début ou à la fin d'un mot et complète ce mot existant pour former un mot valide\n" +
                                "    - Chaque lettre qui doit être placée sur le plateau doit être dans votre main");
                            Console.WriteLine();
                            Console.WriteLine("Fin de partie : \n" +
                                " - La partie se termine quand le sac de jeton est vide ou si vous décidez d'arrêter le jeu depuis le menu\n" +
                                " - Le joueur ayant le score le plus élevé a gagné.");

                            Console.WriteLine();

                            #endregion

                            Console.WriteLine("Pour fermer les règles et retourner au menu appuyez sur entrée");
                            Console.ReadKey(true);
                            Console.Clear();
                            break;
                        case '8':
                            
                            break;
                        default:
                            Console.WriteLine("Cette entrée n'est pas supportée, les charactères à saisir pour chaque option du menu sont affichés à coté de chaque option");
                            Console.WriteLine("Appuyer sur entrée pour retourner au menu");
                            Console.ReadKey(true);
                            break;
                           
                    }
                } while (!sortie_menu);
                
                #endregion
                if (!Menu_Continuer_Tour)
                {
                    jeu.Count_joueurs++;
                    if (jeu.Count_joueurs == jeu.Joueurs.Length) { jeu.Count_joueurs = 0; }
                }
                
            }
            #region fin de partie
            
            Console.Clear();
            jeu.Plateau.Affichage_plateau();
            Console.WriteLine();
            Console.WriteLine("La partie est finie");
            Console.WriteLine("Les scores finaux sont : ");
            Console.WriteLine();
            for (int i = 0; i < jeu.Joueurs.Length; i++)
            {
                Console.WriteLine(jeu.Joueurs[i].ToString());
                Console.WriteLine();
            }
            Console.WriteLine();
            int max = 0;
            int index = 0;
            for (int i = 0; i < jeu.Joueurs.Length; i++)
            {
                if (jeu.Joueurs[i].Score > max)
                {
                    max = jeu.Joueurs[i].Score;
                    index = i;
                }
            }
            if (sauvegarde)
            {
                Console.WriteLine(jeu.Joueurs[index].Nom + " est vainqueur !! ... Pour l'instant");
            }
            else
            {
                Console.WriteLine(jeu.Joueurs[index].Nom + " est vainqueur !!!!");
            }
            
            
            
            #endregion
        }
    }

           


            #endregion

}



