using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands {
    public static class Player {

        private static Location currentLocation;
        private static Location locationToTravelTo;
        private static List<Quest> acceptedQuests = new List<Quest>();

        private static Vehicle currentVehicle;
        private static bool hasVehicle;
        
        static Player() {}

       
        public static Location CurrentLocation {
            get {
                return currentLocation;
            }
            set {
                currentLocation = value;
            }
        }

        public static Location LocationToTravelTo {
            get {
                return locationToTravelTo;
            }
            set {
                locationToTravelTo = value;
            }
        }

        public static Vehicle CurrentVehicle {
            get {
                return currentVehicle;
            }
            set {
                currentVehicle = value;
            }
        }

        public static bool HasVehicle {
            get {
                return hasVehicle;
            }
            set {
                hasVehicle = value;
            }
        }

        public static List<Quest> AcceptedQuests {
            get {
                return acceptedQuests;
            }
            set {
                acceptedQuests = value;
            }
        }


    //need a check for if the current location has changed
    //if it has then check the quests if their delivery location is the current location
    //then check if the items are in your inventory
    //if all prerequisites are met subtract the items from inventory
    //add the reward value to the players money

        //get Quest at position
        public static Quest getAcceptedQuest(int position) {
            return acceptedQuests[position];
        }

        public static void SavePlayer() {
            SaveSystem.SavePlayer();
        }

        public static void LoadPlayer() {
            PlayerData data = SaveSystem.LoadPlayer();

            CurrentLocation = new Location(data.CurrentLocation);

            try
            {
                LocationToTravelTo = new Location(data.LocationToTravelTo);
            }
            catch (System.Exception)
            {
                
                locationToTravelTo = null;
            }
                
            AcceptedQuests = data.AcceptedQuests;
            if(data.HasVehicle){
                if (data.CurrentVehicle.Name.Equals("Scout"))
                {
                    CurrentVehicle = new Scout(data.CurrentVehicle);
                }
                else if (data.CurrentVehicle.Name.Equals("Warthog"))
                {
                    CurrentVehicle = new Warthog(data.CurrentVehicle);
                }
                else if (data.CurrentVehicle.Name.Equals("Goliath"))
                {
                    CurrentVehicle = new Goliath(data.CurrentVehicle);
                }
                else if (data.CurrentVehicle.Name.Equals("Leviathan"))
                {
                    CurrentVehicle = new Leviathan(data.CurrentVehicle);
                }
            }
            HasVehicle = data.HasVehicle;
        }
    }
}  
