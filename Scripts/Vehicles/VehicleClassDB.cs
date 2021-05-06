using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands {
    public static class VehicleClassDB {

        private static List<Vehicle> vehicles = new List<Vehicle>();


        static VehicleClassDB() 
        {
            vehicles = new List<Vehicle>() {
                // int id, int maxHP, int currentHP, int speed, int partySize, int capacity, int price
                
                //NEXT TIME BALANCE THESE
                   new Scout("Scout", 1, 100, 100, 100, 2, 500, 5000),
                   new Warthog("Warthog", 2, 300, 300, 70, 3, 1000, 10000),
                   new Goliath("Goliath", 3, 500, 500, 80, 4, 1500, 20000),
                   new Leviathan("Leviathan", 4, 800, 800, 60, 5, 2500, 30000)
            };
        }
        

        //get database
        public static List<Vehicle> getVehicleList() {
            return vehicles;
        }

        //get vehicle at position
        public static Vehicle getVehicle(int position) {
          return vehicles[position];
        }

        //clear
        public static void clearVehicleList() {
            vehicles.Clear();
        }
    }
}
