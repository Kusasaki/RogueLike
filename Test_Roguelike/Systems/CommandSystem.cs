using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RogueSharp.DiceNotation;
using RLNET;
using Test_Roguelike.Core;
using Test_Roguelike.Core.Monsters;
using Test_Roguelike.Core.Items;
using Test_Roguelike.Interfaces;

namespace Test_Roguelike.Systems
{
    public class CommandSystem
    {
        public bool IsPlayerTurn { get; set; }

        //Termine le tour du joueur
        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
        }

        //Fait rentrer les Actors dans la file d'attente de la partie
        public void ActivateMonsters()
        {
            IScheduleable scheduleable = Game.SchedulingSystem.Get();
            if (scheduleable is Player)
            {
                IsPlayerTurn = true;
                if (Game.Player.Health >= 0)
                    Game.SchedulingSystem.Add(Game.Player);
            }
            else
            {
                Monster monster = scheduleable as Monster;

                if (monster != null)
                {
                    monster.PerformAction(this);
                    Game.SchedulingSystem.Add(monster);
                }

                ActivateMonsters();
            }
        }

        //Permet de déplacer les monstres dans le niveau et attaquer le joueur s'il faut
        public void MoveMonster(Monster monster, Cell cell)
        {
            if (!Game.DungeonMap.SetActorPosition(monster, cell.X, cell.Y))
            {
                if (Game.Player.X == cell.X && Game.Player.Y == cell.Y)
                {
                    Attack(monster, Game.Player, 0);
                }
            }
        }

        //Permet au joueur de se déplacer, ramasser des objets et attaquer
        //Renvoie un bool pour confirmer que le joueur a bien fait une action
        public bool MovePlayer(Direction direction)
        {
            int x = Game.Player.X;
            int y = Game.Player.Y;

            //Deplacement 
            switch (direction)
            {
                case Direction.Up:
                    {
                        y = Game.Player.Y - 1;
                        break;
                    }
                case Direction.Down:
                    {
                        y = Game.Player.Y + 1;
                        break;
                    }
                case Direction.Left:
                    {
                        x = Game.Player.X - 1;
                        break;
                    }
                case Direction.Right:
                    {
                        x = Game.Player.X + 1;
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            Item item = Game.DungeonMap.GetItemAt(x, y);
            if (item != null)
            {
                //Gère le ramassage des objets selon leurs type et certaines conditions
                if (item is Weapon weapon)
                {
                    Game.Player.Weapon = weapon;
                    Game.MessageLog.Add("Vous avez recupere un/une" + item.ToString());
                    Game.DungeonMap.RemoveItem(item);
                }
                else if (item is Potion potion)
                {
                    Game.Player.Consume(potion);
                    Game.MessageLog.Add("Vous avez recupere un/une" + item.ToString());
                    Game.DungeonMap.RemoveItem(item);
                }
                //La machine est l'item final qui doit être détruit pour gagner la partie
                else if (item is Machine machine && Game.Player.Inventory.OfType<Key>().Any(k => k.Level == 10))
                {
                    machine.Destroyed = true;
                    Game.Player.Inventory.Add(item);
                    Game.MessageLog.Add("Vous avez recupere un/une" + item.ToString());
                    Game.DungeonMap.RemoveItem(item);
                }
                else if (item is Key || item is Joke)
                {
                    Game.Player.Inventory.Add(item);
                    Game.MessageLog.Add("Vous avez recupere un/une" + item.ToString());
                    Game.DungeonMap.RemoveItem(item);
                }
                return true;
            }

            if (Game.DungeonMap.SetActorPosition(Game.Player, x, y))
            {
                return true;
            }
            Monster monster = Game.DungeonMap.GetMonsterAt(x, y);

            //attaque le monstre s'il sur la case vers laquelle on se dirige
            if (monster != null)
            {
                Attack(Game.Player, monster, 1);
                return true;
            }

            return false;
        }

        //Gere les attaques des joueurs et des monstres
        public void Attack(Actor attacker, Actor defender, int attackMode)
        {
            //Execute l'action du joueur ou du monstre
            if (Game.Player.IsAttacking || attacker is Monster)
            {
                StringBuilder attackMessage = new StringBuilder();
                StringBuilder defenseMessage = new StringBuilder();
                if (attacker is Monster)
                    attackMode = 0;
                int hits = ResolveAttack(attacker, defender, attackMode, attackMessage, out bool isJoke);

                int blocks = ResolveDefense(defender, attacker, hits, attackMessage, defenseMessage);

                Game.MessageLog.Add(attackMessage.ToString());
                if (!string.IsNullOrWhiteSpace(defenseMessage.ToString()))
                {
                    Game.MessageLog.Add(defenseMessage.ToString()); 
                    Game.MessageLog.Add(" ");
                }

                int damage = hits - blocks;

                ResolveDamage(defender, damage, isJoke);
                Game.Player.IsAttacking = false;
            }
            //Demande au joueur comment il veut attaquer -> Gestion dans le Main de Game.cs
            else
            {
                Game.Player.IsAttacking = true;
                Game.Player.Target = defender;
                Game.MessageLog.Add(" ");
                Game.MessageLog.Add("[1] Attaquer avec votre arme [2] Faire une blague");
            }
        }

        // Gestion des degats selon l'attaque du joueur ou du monstre
        private static int ResolveAttack(Actor attacker, Actor defender, int attackMode, StringBuilder attackMessage, out bool isJoke)
        {
            int hits = 0;
            isJoke = false;
            attackMessage.AppendFormat("{0} attaque {1}", attacker.Name, defender.Name, attackMode);

            if (attacker is Player player)
            {
                //Un joueur ne peux pas tuer un monstre avec une attaque physique, il doit l'achever avec une blague
                if (defender.Health / (defender.MaxHealth * 1.0) <= 0.3)
                {
                    if (attackMode == 1)
                    {
                        Game.MessageLog.Add("Ce n'est pas tres efficace");
                    }
                    else if (player.Inventory.OfType<Joke>().Any() && attackMode == 2)
                    {
                        hits = 500;
                        isJoke = true;
                        Game.MessageLog.Add(player.Inventory.OfType<Joke>().First().Description);
                        player.Inventory.Remove(player.Inventory.OfType<Joke>().First());
                    }
                    else if (!player.Inventory.OfType<Joke>().Any() && attackMode == 2)
                    {
                        Game.MessageLog.Add("AH JE N'AI PLUS DE BLAGUES POUR LES CONJURER !");
                    }
                }    
                //En temps normal, ce sont les degats de base du joueur qui comptent et ils ne peuvent pas mourir d'une blague s'il ne sont pas affaiblis
                else
                {
                    if (attackMode == 1)
                    {
                        hits = player.PAttack + player.Weapon.PAttackBoost;
                    }
                    
                    else if (player.Inventory.OfType<Joke>().Any() && attackMode == 2)
                    {
                        Game.MessageLog.Add(player.Inventory.OfType<Joke>().First().Description);
                        player.Inventory.Remove(player.Inventory.OfType<Joke>().First());
                    }
                    else if (!player.Inventory.OfType<Joke>().Any() && attackMode == 2)
                    {
                        Game.MessageLog.Add("AH! Je n'ai plus de blagues!");
                    }
                }
            } 

            //Les fantomes infligent des degats grace a leur statistique
            if(attacker is Ghost ghost)
            {
                hits = ghost.FAttack;
            }
            //Les boss attaquent avec des degats physiques
            if(attacker is Boss boss)
            {
                hits = boss.PAttack;
            }

            return hits;
        }

        //Gere la defense des acteurs
        private static int ResolveDefense(Actor defender, Actor attacker, int hits, StringBuilder attackMessage, StringBuilder defenseMessage)
        {
            int blocks = 0;

            //Un monstre se fait obliterer par une blague
            if (hits == 500 && defender is Monster)
                blocks = 0;
            //Sinon l'attaque est amortie par les statistiques de défense des acteurs
            else if (hits > 0)
            {
                if (attacker is Player || attacker is Boss)
                    blocks = (int)(hits * defender.Defense / 100.0);
                else if (attacker is Ghost)
                    blocks = (int)(hits * defender.Resistance / 100.0);

                //Un acteur peut esquiver une attaque selon son agilite
                else if (defender.Agility >= Dice.Roll("1D100"))
                    blocks = hits;

                if(blocks > 0)
                    defenseMessage.AppendFormat("{0} bloque ", blocks);
            }
            else
            {
                attackMessage.Append(" et n'inflige pas de degats.");
            }

            return blocks;
        }

        // Gere les dégats reçus par les acteurs
        private static void ResolveDamage(Actor defender, int damage, bool isJoke)
        {
            if (damage > 0)
            {
                //Un monstre ne peut pas mourir d'une attaque physique et se régénère si on essaye de le tuer de cette façon
                if(defender is Monster && !isJoke)
                {
                    if(defender.Health - damage < 0)
                        defender.Health = (int)(defender.MaxHealth / 4);
                    else
                        defender.Health = defender.Health - damage;
                }
                //Sinon on applique les degats avec la prise en compre de la defense
                else
                    defender.Health = defender.Health - damage;

                Game.MessageLog.Add($"  {defender.Name} a subi {damage} points de degat");

                if (defender.Health <= 0)
                {
                    ResolveDeath(defender);
                }
            }
            else
            {
                Game.MessageLog.Add($"  {defender.Name} a esquive le coup");
            }
        }

        // Gestion de la mort d'un acteur
        private static void ResolveDeath(Actor defender)
        {
            //Renvoie le statut mort -> Game.cs -> Redemarre le jeu
            if (defender is Player)
            {
                Game.Player.IsDead = true;
                Game.MessageLog.Add($"  {defender.Name} est mort, VOUS AVEZ PERDUUUUUUUU !");
            }
            else if (defender is Monster monster)
            {
                //Recupère les items sur le monstre pour les mettre dans l'inventaire du joueur ou les consommer selon leur classe
                if (monster.Inventory != null)
                    foreach (Item item in monster.Inventory)
                    {
                        if (item is Potion potion)
                        {
                            Game.Player.Consume(potion);
                        }
                        else if (item is Weapon weapon)
                        {
                            Game.Player.Weapon = weapon;
                        }
                        else
                        {
                            Game.Player.Inventory.Add(item);
                        }
                        Game.MessageLog.Add("Vous avez recupere un/une" + item.ToString());
                    }

                //Enleve le monstre de la carte
                Game.DungeonMap.RemoveMonster(monster);
                Game.Player.Heal((int)Math.Floor(Game.Player.MaxHealth * 3.0 / 100));
                
                Game.MessageLog.Add($"  {monster.Name} est mort");
              
            }
        }
    }
}
