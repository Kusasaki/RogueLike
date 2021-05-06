using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using Test_Roguelike.Interfaces;


namespace Test_Roguelike.Systems
{
    public class SchedulingSystem
    {
        //Creation d'un emploi du temps sur le modele roguesharp
        private int _time;
        private readonly SortedDictionary<int, List<IScheduleable>> _scheduleables;

        public SchedulingSystem()
        {
            _time = 0;
            _scheduleables = new SortedDictionary<int, List<IScheduleable>>();
        }

        // Ajout d'un objet à la liste selon son "horaire" d'activité -> il pourra agir
        public void Add(IScheduleable scheduleable)
        {
            int key = _time + scheduleable.Time;
            if (!_scheduleables.ContainsKey(key))
            {
                _scheduleables.Add(key, new List<IScheduleable>());
            }
            _scheduleables[key].Add(scheduleable);
        }

        // Enleve un acteur de l'ordre des tours -> gestions des morts
        public void Remove(IScheduleable scheduleable)
        {
            KeyValuePair<int, List<IScheduleable>> scheduleableListFound
              = new KeyValuePair<int, List<IScheduleable>>(-1, null);

            foreach (var scheduleablesList in _scheduleables)
            {
                if (scheduleablesList.Value.Contains(scheduleable))
                {
                    scheduleableListFound = scheduleablesList;
                    break;
                }
            }
            if (scheduleableListFound.Value != null)
            {
                scheduleableListFound.Value.Remove(scheduleable);
                if (scheduleableListFound.Value.Count <= 0)
                {
                    _scheduleables.Remove(scheduleableListFound.Key);
                }
            }
        }

        // Retourne le premier acteur de la liste, l'en eleve pur qu'il ne rejoue pas de suite  
        //et fait avancer le temps si necessaire pour avancer la partie et assuerer la cohérence des timescodes en key.
        public IScheduleable Get()
        {
            var firstScheduleableGroup = _scheduleables.First();
            var firstScheduleable = firstScheduleableGroup.Value.First();
            Remove(firstScheduleable);
            _time = firstScheduleableGroup.Key;
            return firstScheduleable;
        }

        // Retourne l'"horaire" de l'horloge 
        public int GetTime()
        {
            return _time;
        }

        // Remet à zero l'emploi du temps 
        public void Clear()
        {
            _time = 0;
            _scheduleables.Clear();
        }
    }
}
