using System;
using AsciiArt;

namespace jeu_du_pendu
{
    class Program
    {
        static void AfficherMot(string mot, List<char> lettres)
        {
            for (int i = 0; i < mot.Length; i++)
            {                
                char lettre = mot[i];
                if (lettres.Contains(lettre))
                {
                    Console.Write($"{lettre} ");
                }
                else
                {
                    Console.Write("_ ");
                }
            }
            Console.WriteLine();
        }

        static bool ToutesLettresDevinees(string mot, List<char> lettres)
        {
            foreach (char lettre in lettres)
            {
                mot = mot.Replace(lettre.ToString(), "");
            }

            if (mot == "")
            {
                return true;
            }
            return false;
        }

        static char DemanderUneLettre(string message = "Rentrez une lettre: ")
        {
            while(true)
            {
                Console.Write(message);
                string reponse = Console.ReadLine();
                if (reponse.Length == 1)
                {
                    reponse = reponse.ToUpper();
                    return reponse[0];
                }            
                Console.WriteLine("ERREUR: Vous devez rentrer une lettre");  
            }
        }

        static void DevinerMot(string mot)
        {           
            var lettresDevinees = new List<char>();
            var lettresExclues = new List<char>();
            const int NB_VIES = 6;
            int viesRestantes = NB_VIES;

            while (viesRestantes > 0)
            {
                Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);
                Console.WriteLine();

                AfficherMot(mot, lettresDevinees);
                Console.WriteLine();
                var lettre = DemanderUneLettre();
                Console.Clear();

                if (mot.Contains(lettre))
                {
                    Console.WriteLine("Cette lettre est dans le mot");
                    lettresDevinees.Add(lettre);
                    if (ToutesLettresDevinees(mot, lettresDevinees))
                    {
                        break;
                    }
                }
                else
                {                 
                    if (!lettresExclues.Contains(lettre))
                    {
                        lettresExclues.Add(lettre);
                        viesRestantes--;                    
                    }
                    Console.WriteLine($"Vies restantes: {viesRestantes}");
                }
                if (lettresExclues.Count > 0)
                {
                    Console.WriteLine($"Le mot ne contient pas les lettres: {String.Join(", ", lettresExclues)}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);

            if (viesRestantes == 0)
            {
                Console.WriteLine($"PERDU ! Le mot était: {mot}");
            }
            else
            {
                AfficherMot(mot, lettresDevinees);
                Console.WriteLine();

                Console.WriteLine("GAGNE !");
            }
        }

        static string[] ChargerLesMots(string nomFichier)
        {
            try
            {
                return File.ReadAllLines(nomFichier);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur de lecture du fichier: {nomFichier} ({ex.Message})");
            }

            return null;
        }

        static bool DemanderDeRejouer()
        {           
            char reponse = DemanderUneLettre("Voulez-vous rejouer (o / n): ");
            if (reponse == 'o' || reponse == 'O')
            {
                return true;
            }
            else if (reponse == 'n' || reponse == 'N')
            {
                return false;
            }
            else
            {
                Console.WriteLine("Erreur: Vous devez répondre avec o ou n");
                return DemanderDeRejouer();
            }
        }

        static void Main(string[] args)
        {
            var mots = ChargerLesMots("mots.txt");

            if (mots == null ||  mots.Length == 0)
            {
                Console.WriteLine("La liste de mots est vide");
            }
            else
            {               
                while (true)
                {
                    Random random = new Random();
                    int index = random.Next(mots.Length);
                    string mot = mots[index].Trim().ToUpper();                       
                    DevinerMot(mot);
                    if (!DemanderDeRejouer())
                    {
                        break;
                    }
                    Console.Clear();
                }
                Console.WriteLine("Merci et à bientôt.");
            }
        }
    }
}