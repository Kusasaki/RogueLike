using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Roguelike.Core.Items
{
    public class Joke : Item
    {
        public int Damage { get; }
        public string Description { get; }
        public string Category { get; }

        public Joke(int dmg, string desc, string category)
        {
            Damage = dmg;
            Category = category;
            Description = desc;
            Symbol = 'J';
        }

        public Joke(int category)
        {
            Symbol = 'J';
            Damage = 8; //FIXME
            List<string> blagues = new List<string>();
            switch (category)
            {
                case 1: //ENSC
                    blagues.Add("Un informaticien c'est quelqu'un qui vous dit quelque chose de precis, exact, mais qui ne vous sert a rien");
                    blagues.Add("De toute facon, si le client est chiant, on l'invite pas a la soutenance et puis voila hein.");
                    blagues.Add("Quelle est la difference entre Carla Bruni et un UML ?");
                    blagues.Add("Je suis paye pour dire ce que je pense donc mon opinion vaut quelque chose");
                    blagues.Add("Moi je m'en fous, je peux attendre, j'ai mon cafe");
                    Category = "ENSC";
                    break;

                case 2: //Carambar
                    blagues.Add("Comment appelle t-on une fleur qui prend sa graine a moitie ? Une migraine.");
                    blagues.Add("Que dit un oignon quand il se cogne ? Ail");
                    blagues.Add("Quelle est la blague a deux balles ? Pan Pan !");
                    blagues.Add("A combien rouliez-vous ? demande le gendarme. A deux seulement, mais si vous voulez monter, il reste de la place");
                    blagues.Add("Quel est le comble pour un professeur de geographie ? C'est de perdre le nord");
                    Category = "Carambar";
                    break;

                case 3: //Calembour
                    blagues.Add("Demandez nos exquis mots");
                    blagues.Add("De deux choses lune, l'autre c'est le soleil");
                    blagues.Add("Pas de chauve a Ajaccio, mais a Calvi si");
                    blagues.Add("Les mots rendent les cris vains.");
                    blagues.Add("Moi je m'en fous, je peux attendre, j'ai mon cafe");
                    Category = "Calembour";
                    break;

                case 4: //Toto
                    blagues.Add("– Toto si je te donne 50 gâteaux et tu en manges 48 tu as donc ? – Mal au ventre");
                    blagues.Add("– Toto si tu as 10 bonbons et que Mathieu t'en prends un combien il t'en reste ? – 10 bonbons et un coup pour Mathieu");
                    blagues.Add("– Toto 3 et 3 ça fait quoi ? – Match nul monsieur! ");
                    blagues.Add("– Connais-tu la blague de toto en Amerique ? – Non ? – Moi non plus il n'y avait plus de place dans l'avion!");
                    blagues.Add("Le professeur demande a Toto de conjuguer le verbe manger, Toto dit : – Euh… je mange, je mangerai, J'AI PLUS FAIM!");
                    Category = "Toto";
                    break;

                default:
                    break;
            }
            Random rnd = new Random();
            
            Description = blagues[rnd.Next(0, blagues.Count)];
        }
        
        

        public override string ToString()
        {
            return " blague : " + Description;
        }
    }
}
