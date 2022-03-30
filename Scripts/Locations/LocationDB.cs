using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands {
    public static class LocationDB {

        private static List<Location> locations = new List<Location>();



        static LocationDB() 
        {
            locations = new List<Location>() {
                //string locationName, int townTier, int territory, int[] nearbyTowns
                new Location(1, "Norwich", 1, 1, new int[]{2}),
                new Location(2, "Westray", 2, 1, new int[]{1, 3, 7}),
                new Location(3, "Veden", 3, 1, new int[]{2, 4}),
                new Location(4, "Tunstead", 1, 2, new int[]{3, 5}),
                new Location(5, "Gillamoor", 2, 2, new int[]{4, 6, 10}),
                new Location(6, "Helm Crest", 2, 2, new int[]{5, 7, 8 }),
                new Location(7, "Dalhurst", 2, 3, new int[]{6, 8, 2}),
                new Location(8, "Kaiser", 3, 3, new int[]{6, 7, 9}),
                new Location(9, "Braedon", 2, 3, new int[]{8}),
                new Location(10, "Fara", 3, 2, new int[]{5})
            };

            foreach (Location location in locations)
            {
                int amount = 21;
                for (int i = 0; i < TradeableDatabase.getTradeableList().Count; i++)
                {
                    if(UnityEngine.Random.Range(0, 101) > 60)
                        location.TradePrices.Add(TradeableDatabase.getTradeableList()[i].Price + UnityEngine.Random.Range(0, amount));
                    else
                        location.TradePrices.Add(TradeableDatabase.getTradeableList()[i].Price - UnityEngine.Random.Range(0, amount));
                    amount += 5;
                }
            }

            // add clock to list in range  if check town level case 
            ChangeItemStocks();

        }







        //get database
        public static List<Location> getLocationList() {
            return locations;
        }

        //get Location at position
        public static Location getLocation(int position) {
            return locations[position];
        }

        //clear
        public static void clearLocationList() {
            locations.Clear();
        }


        public static void ChangePrices()
        {
            string destinationName = "";
            if (Player.LocationToTravelTo != null)
                destinationName = Player.LocationToTravelTo.LocationName;

            foreach (Location location in locations)
            {
                int amount = 21;
                if(location.LocationName != destinationName)
                {
                    for (int i = 0; i < TradeableDatabase.getTradeableList().Count; i++)
                    {
                        if (UnityEngine.Random.Range(0, 101) > 60)
                            location.TradePrices.Add(TradeableDatabase.getTradeableList()[i].Price + UnityEngine.Random.Range(0, amount));
                        else
                            location.TradePrices.Add(TradeableDatabase.getTradeableList()[i].Price - UnityEngine.Random.Range(0, amount));
                        amount += 5;
                    }
                }
            }
        }

        public static void ChangeItemStocks()
        {
            //How much of each item spawns in the Trade Goods depends on town tier

            //cloth, rations, leather, spice, coal, steel, tools, silver, gold, diamond
            int[][] minQty = new int[][] {
                new int[] { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, //village tier 1
                new int[] { 5, 5, 1, 10, 10, 2, 1, 0, 0, 0 }, //town    tier 2
                new int[] { 10, 5, 5, 20, 15, 5, 4, 4, 4, 1 }, //capital tier 3
            };

            int[][] maxQty = new int[][] {
                new int[] { 10, 10, 5, 15, 15, 3, 3, 5, 3, 0 }, //village
                new int[] { 15, 15, 10, 30, 30, 10, 10, 10, 10, 10 }, //town
                new int[] { 20, 20, 20, 60, 50, 20, 20, 20, 20, 20 }, //capital
            };

            foreach (Location location in locations)
            {
                for (int i = 0; i < TradeableDatabase.getTradeableList().Count; i++)
                {
                    location.ItemStock.Add(Random.Range(minQty[location.TownTier - 1][i], maxQty[location.TownTier - 1][i]));
                }
            }
        }
    }
}
