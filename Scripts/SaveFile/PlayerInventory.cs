using System.Collections;
using System.Collections.Generic;

namespace Sands
{
    public static class PlayerInventory 
    {
        private static List<InventoryTradeable> tradeableInventory;
        private static double money;
        static PlayerInventory(){
            tradeableInventory = new List<InventoryTradeable>(){
                new InventoryTradeable(TradeableDatabase.getTradeable(0), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(1), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(2), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(3), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(4), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(5), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(6), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(7), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(8), 0),
                new InventoryTradeable(TradeableDatabase.getTradeable(9), 0)
            };
            money = 0;
        }

        //adding items to lists based on type
        public static void AddToInventory(int tradeableId, int count){
            foreach (var tradeable in tradeableInventory)
            {
                if(tradeable.OwnedTradeable.Id == tradeableId){
                    tradeable.Count += count;
                }
            }
        }

        public static void RemoveFromInventory(int tradeableId, int count){
            foreach (var tradeable in tradeableInventory)
            {
                if(tradeable.OwnedTradeable.Id == tradeableId){
                    tradeable.Count -= count;
                }
            }
        }

        public static void SavePlayerInventory() {

            SaveSystem.SavePlayerInventory();
        }

        public static void LoadPlayerInventory() {

            PlayerData data = SaveSystem.LoadPlayerInventory();
            TradeableInventory = data.PlayerTradeableInventory;
            Money = data.PlayerMoney;
            
        }

        //getters and setters

        public static List<InventoryTradeable> TradeableInventory{
            get{
                return tradeableInventory;
            }
            set{
                tradeableInventory = value;
            }
        }

        public static double Money{
            get{
                return money;
            }
            set{
                money = value;
            }
        }

    }
}

