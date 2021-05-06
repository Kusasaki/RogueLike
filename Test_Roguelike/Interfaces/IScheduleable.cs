using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Roguelike.Interfaces
{
    //On en a besoin pour faire le lien entre Actor et SchedulingSystem
    public interface IScheduleable
    {
        int Time { get; }
    }


}
