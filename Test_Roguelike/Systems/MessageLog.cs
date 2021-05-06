using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;

namespace Test_Roguelike.Systems
{
    
    // Represente la liste des messages a afficher (systeme semblable a une file d'attente qui quand on l'actualise, affiche les derniers messages)
    public class MessageLog
    {
        // Taille de l'affichage 
        private static readonly int _maxLines = 14;

        // Creation de la queue
        private readonly Queue<string> _lines;

        public MessageLog()
        {
            _lines = new Queue<string>();
        }

        // Ajout d'un message à la liste des message 
        public void Add(string message)
        {
            _lines.Enqueue(message);

            // Si on depasse la limite de l'affichage, les messages les plus anciens sortent de la queue
            if (_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }
        }

        // Affiche les lignes de la queue dans la console
        public void Draw(RLConsole console)
        {
            console.Clear();
            string[] lines = _lines.ToArray();
            for (int i = 0; i < lines.Length; i++)
            {
                console.Print(1, i + 1, lines[i], RLColor.White);
            }
        }
    }
}

