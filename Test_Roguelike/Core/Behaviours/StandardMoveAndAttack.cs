using System.Linq;
using RogueSharp;
using Test_Roguelike.Core;
using Test_Roguelike.Interfaces;
using Test_Roguelike.Systems;

namespace Test_Roguelike.Core.Behaviours
{
    //Comportement des ennemis
    public class StandardMoveAndAttack : IBehavior
    {
        public bool Act(Monster monster, CommandSystem commandSystem)
        {
            DungeonMap dungeonMap = Game.DungeonMap;
            Player player = Game.Player;
            FieldOfView monsterFov = new FieldOfView(dungeonMap);

            //Si le monstre entre dans le champ de vision du joueur, un message s'affiche dans la console de message
            if (!monster.TurnsAlerted.HasValue)
            {
                //ComputeFov : fonction RogueSharp qui calcule le champ de vision du monstre
                monsterFov.ComputeFov(monster.X, monster.Y, monster.Awareness, true);
                if (monsterFov.IsInFov(player.X, player.Y))
                {
                    Game.MessageLog.Add($"{monster.Name} veut votre mort");
                    monster.TurnsAlerted = 1;
                }
            }

            if (monster.TurnsAlerted.HasValue)
            {
                //On remet les cases du héros et de l'ennemi en IsWalkable
                //Sinon on finit par construire un mur
                dungeonMap.SetIsWalkable(monster.X, monster.Y, true);
                dungeonMap.SetIsWalkable(player.X, player.Y, true);

                PathFinder pathFinder = new PathFinder(dungeonMap);
                Path path = null;

                try
                {
                    path = pathFinder.ShortestPath(dungeonMap.GetCell(monster.X, monster.Y),
                    dungeonMap.GetCell(player.X, player.Y));
                }
                catch (PathNotFoundException)
                {
                    //Si le monstre ne trouve pas de chemin entre lui et le joueur, il attend
                    Game.MessageLog.Add($"{monster.Name} bug");
                }

                //La série de déplacement est finie, donc on met les cases du joueur et du monstre en !IsWalkable
                dungeonMap.SetIsWalkable(monster.X, monster.Y, false);
                dungeonMap.SetIsWalkable(player.X, player.Y, false);

                //Le monstre a trouvé un chemin vers le joueur
                if (path != null)
                {
                    try
                    {
                        //Le monstre avance vers le joueur
                        commandSystem.MoveMonster(monster, (Cell)path.StepForward());
                    }
                    catch (NoMoreStepsException)
                    {
                        //Le monstre est trop loin du joueur
                        Game.MessageLog.Add($"{monster.Name} est frustre");
                    }
                }

                monster.TurnsAlerted++;

                //Après 15 déplacements et si le monstre n'est plus dans le champ de vision du joueur, il n'est plus en alerte
                //Et il arrête de le suivre
                if (monster.TurnsAlerted > 15)
                {
                    monster.TurnsAlerted = null;
                }
            }
            return true;
        }
    }
}
