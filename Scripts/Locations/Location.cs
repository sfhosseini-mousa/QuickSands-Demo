using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    [System.Serializable]
    public class Location
    {

        private int id;
        private string locationName;
        private int townTier;
        private int[] nearbyTowns;
        private int territory;
        private List<double> tradePrices;

        List<int> itemStock;


            





        //default constructor
        public Location() { }

        //6 argument constructor
        public Location(int id, string locationName, int townTier, int territory, int[] nearbyTowns)
        {
            this.id = id;
            this.locationName = locationName;
            this.townTier = townTier;
            this.territory = territory;
            this.nearbyTowns = nearbyTowns;
            this.tradePrices = new List<double>();
            this.itemStock = new List<int>();
        }

        //memento copy constructor
        public Location(LocationMemento locationMemento)
        {
            this.id = locationMemento.Id;
            this.locationName = locationMemento.LocationName;
            this.townTier = locationMemento.TownTier;
            this.territory = locationMemento.Territory;
            this.nearbyTowns = locationMemento.NearbyTowns;
            
        }
        
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public string LocationName
        {
            get
            {
                return locationName;
            }
            set
            {
                locationName = value;
            }
        }
        public int TownTier
        {
            get
            {
                return townTier;
            }
            set
            {
                townTier = value;
            }
        }

        public int Territory
        {
            get
            {
                return territory;
            }
            set
            {
                territory = value;
            }
        }

        public int[] NearbyTowns
        {
            get
            {
                return nearbyTowns;
            }
            set
            {
                nearbyTowns = value;
            }
        }

        public List<double> TradePrices{
            get
            {
                return tradePrices;
            }
            set
            {
                tradePrices = value;
            }
        }

        public List<int> ItemStock
        {
            get
            {
                return itemStock;
            }
            set
            {
                itemStock = value;
            }
        }
    }
}