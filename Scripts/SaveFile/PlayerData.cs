using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class PlayerData
    {
        
        //the parties current location
        private LocationMemento currentLocation;
        private LocationMemento locationToTravelTo;

        //a list of accepted quests
        private List<Quest> acceptedQuests;

        //your parties current vehicle
        private VehicleMemento currentVehicle;

        //a check for if you have a vehicle
        private bool hasVehicle;

        //a list of nests that tell us if theyre active or not
        private List<NestMemento> nests = new List<NestMemento>();
        
        //a list of your partys heroes
        private List<HeroMemento> heroParty = new List<HeroMemento>();

        //player inventory
        private List<InventoryTradeable> playerTradeableInventory;

        private double playerMoney;

        //empty constructor
        public PlayerData(){ }

        //Getters and setters
        public LocationMemento CurrentLocation {
            get {
                return currentLocation;
            }
            set {
                currentLocation = value;
            }
        }

        public LocationMemento LocationToTravelTo {
            get {
                return locationToTravelTo;
            }
            set {
                locationToTravelTo = value;
            }
        }

        public List<Quest> AcceptedQuests {
            get {
                return acceptedQuests;
            }
            set {
                acceptedQuests = value;
            }
        }
        
        public VehicleMemento CurrentVehicle {
            get {
                return currentVehicle;
            }
            set {
                currentVehicle = value;
            }
        }

        public bool HasVehicle {
            get {
                return hasVehicle;
            }
            set {
                hasVehicle = value;
            }
        }

        public List<NestMemento> Nests {
            get {
                return nests;
            }
            set {
                nests = value;
            }
        }

        public List<HeroMemento> HeroParty {
            get {
                return heroParty;
            }
            set {
                heroParty = value;
            }
        }

        public List<InventoryTradeable> PlayerTradeableInventory{
            get{
                return playerTradeableInventory;
            }
            set{
                playerTradeableInventory = value;
            }
        }

        public double PlayerMoney {
            get {
                return playerMoney;
            }
            set {
                playerMoney = value;
            }
        }
    }
}
