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

        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
        }

        public void ActivateMonsters()
        {
            IScheduleable scheduleable = Game.SchedulingSystem.Get();
            if (scheduleable is Player)
            {
                IsPlayerTurn = true;
                if (Game.Player.Health > -5)
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

        // Return value is true if the player was able to move
        // false when the player couldn't move, such as trying to move into a wall
        public bool MovePlayer(Direction direction)
        {
            int x = Game.Player.X;
            int y = Game.Player.Y;

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
                //Inventory
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

            if (monster != null)
            {
                Attack(Game.Player, monster, 1);
                return true;
            }

            return false;
        }

        public void Attack(Actor attacker, Actor defender, int attackMode)
        {
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

                ResolveDamage(defender, damage);
                Game.Player.IsAttacking = false;
            }
            else
            {
                Game.Player.IsAttacking = true;
                Game.Player.Target = defender;
                Game.MessageLog.Add(" ");
                Game.MessageLog.Add("[1] Attaquer avec votre arme [2] Faire une blague");
            }
        }

        // The attacker rolls based on his stats to see if he gets any hits
        private static int ResolveAttack(Actor attacker, Actor defender, int attackMode, StringBuilder attackMessage, out bool isJoke)
        {
            int hits = 0;
            isJoke = false;
            attackMessage.AppendFormat("{0} attaque {1}", attacker.Name, defender.Name, attackMode);

            if (attacker is Player player)
            {
                if (defender.Health / (defender.MaxHealth * 1.0) <= 0.2)
                {
                    if (attackMode == 1)
                    {
                        hits = 0;
                        Game.MessageLog.Add("Ce n'est pas tres efficace");
                        defender.Health = (int)(defender.MaxHealth / 3.0);
                    }
                    else if (player.Inventory.OfType<Joke>().Any() && attackMode == 2)
                    {
                        hits = 500;
                        Game.MessageLog.Add(player.Inventory.OfType<Joke>().First().Description);
                        player.Inventory.Remove(player.Inventory.OfType<Joke>().First());
                    }
                    else if (!player.Inventory.OfType<Joke>().Any() && attackMode == 2)
                    {
                        hits = 0;
                        Game.MessageLog.Add("AH JE N'AI PLUS DE BLAGUES POUR LES CONJURER !");
                        Task.Delay(2000);
                    }
                }    
                else
                {
                    if (attackMode == 1)
                    {
                        hits = player.PAttack + player.Weapon.PAttackBoost;
                    }
                    
                    else if (player.Inventory.OfType<Joke>().Any() && attackMode == 2)
                    {
                        hits = 0;
                        Game.MessageLog.Add(player.Inventory.OfType<Joke>().First().Description);
                        player.Inventory.Remove(player.Inventory.OfType<Joke>().First());
                    }
                    else if (!player.Inventory.OfType<Joke>().Any() && attackMode == 2)
                    {
                        hits = 0;
                        Game.MessageLog.Add("AH! Je n'ai plus de blagues!");
                        Task.Delay(2000);
                    }
                }
            } 

            if(attacker is Ghost ghost)
            {
                hits = ghost.FAttack;
            }

            if(attacker is Boss boss)
            {
                hits = boss.PAttack;
            }

            return hits;
        }

        // The defender rolls based on his stats to see if he blocks any of the hits from the attacker
        private static int ResolveDefense(Actor defender, Actor attacker, int hits, StringBuilder attackMessage, StringBuilder defenseMessage)
        {
            int blocks = 0;

            if (hits == 500)
                blocks = 0;
            else if (hits > 0 && hits != 500)
            {
                if (attacker is Player || attacker is Boss)
                    blocks = (int)(hits * defender.Defense / 100.0);
                else if (attacker is Ghost)
                    blocks = (int)(hits * defender.Resistance / 100.0);
                else if (defender.Agility >= Dice.Roll("1D100"))
                    blocks = hits;

                if(blocks > 0)
                    defenseMessage.AppendFormat("{0} bloque ", blocks);
            }
            else
            {
                attackMessage.Append("et n'inflige pas de degats.");
            }

            return blocks;
        }

        // Apply any damage that wasn't blocked to the defender
        private static void ResolveDamage(Actor defender, int damage)
        {
            if (damage > 0)
            {
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

        // Remove the defender from the map and add some messages upon death.
        private static void ResolveDeath(Actor defender)
        {
            if (defender is Player)
            {
                Game.Player.IsDead = true;
                Game.MessageLog.Add($"  {defender.Name} est mort, VOUS AVEZ PERDUUUUUUUU !");
            }
            else if (defender is Monster monster)
            {
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


                Game.DungeonMap.RemoveMonster(monster);
                
                Game.MessageLog.Add($"  {monster.Name} est mort");
              
            }
        }
    }
}
