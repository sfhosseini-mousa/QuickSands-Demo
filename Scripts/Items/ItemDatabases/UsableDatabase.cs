using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public static class UsableDatabase
    {

        private static List<Usable> usables = new List<Usable>();

        //get database
        public static List<Usable> getUsableList()
        {
            return usables;
        }

        static UsableDatabase()
        {
            usables = new List<Usable>() {
                //int ID, string itemName, int weight, double price, int effectAmount
                new Usable(1, "Health Potion", 2, 100.0, 100),
                new Usable(2, "Repair Kit", 2, 200.0, 500)
                
            };
        }

        //get Hero at position
        public static Usable getUsable(int position)
        {
            return usables[position];
        }
    }
}