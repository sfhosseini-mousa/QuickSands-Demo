using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sands
{

    public static class SaveSystem
    {
        static string path = Application.persistentDataPath + "/player.savefile";
        private static PlayerData pdata;
        //The constructor calls the LoadAll function to load all the data - runs on start of the program
        static SaveSystem()
        {
            pdata = new PlayerData();
            LoadAll();
            SaveAll();
        }

        //Saves all the data
        public static void SaveAll()
        {
            SavePlayer();
            SaveNests();
            SaveParty();
            SavePlayerInventory();
        }

        public static void LoadAll()
        {
            Player.LoadPlayer();
            HeroPartyDB.LoadParty();
            NestDB.LoadNests();
            PlayerInventory.LoadPlayerInventory();
        }

        //Save and load functions
        public static void SavePlayer()
        {

            BinaryFormatter formatter = new BinaryFormatter();


            FileStream stream = new FileStream(path, FileMode.Create);


            pdata.AcceptedQuests = Player.AcceptedQuests;
            pdata.CurrentLocation = new LocationMemento(Player.CurrentLocation);
            try
            {
                pdata.LocationToTravelTo = new LocationMemento(Player.LocationToTravelTo);
            }
            catch (System.Exception)
            {
                pdata.LocationToTravelTo = null;
            }

            if(Player.HasVehicle){
                if (Player.CurrentVehicle.Name.Equals("Scout"))
                {
                    pdata.CurrentVehicle = new ScoutMemento(Player.CurrentVehicle);
                }
                else if (Player.CurrentVehicle.Name.Equals("Warthog"))
                {
                    pdata.CurrentVehicle = new WarthogMemento(Player.CurrentVehicle);
                }
                else if (Player.CurrentVehicle.Name.Equals("Goliath"))
                {
                    pdata.CurrentVehicle = new GoliathMemento(Player.CurrentVehicle);
                }
                else if (Player.CurrentVehicle.Name.Equals("Leviathan"))
                {
                    pdata.CurrentVehicle = new LeviathanMemento(Player.CurrentVehicle);
                }
            }
            else{
                pdata.CurrentVehicle = new VehicleMemento();
            }


            pdata.HasVehicle = Player.HasVehicle;

            formatter.Serialize(stream, pdata);
            stream.Close();
        }

        public static PlayerData LoadPlayer()
        {

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                pdata.CurrentLocation = data.CurrentLocation;
                pdata.LocationToTravelTo = data.LocationToTravelTo;
                pdata.HasVehicle = data.HasVehicle;
                stream.Close();
                return data;
            }
            else
            {
                Debug.Log("Save file not found in " + path);
                
                pdata.CurrentLocation = new LocationMemento(LocationDB.getLocation(0));
                pdata.HasVehicle = false;
                
                
                return pdata;
                
            }
            
        }

        public static void SaveNests()
        {

            BinaryFormatter formatter = new BinaryFormatter();


            FileStream stream = new FileStream(path, FileMode.Create);
            pdata.Nests = new List<NestMemento>();
            foreach (Nest nest in NestDB.getNestList())
            {
                pdata.Nests.Add(new NestMemento(nest));
            }
            formatter.Serialize(stream, pdata);
            stream.Close();


        }

        public static PlayerData LoadNests()
        {

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                pdata.Nests = data.Nests;
                
                stream.Close();

                return pdata;

            }
            else
            {

                pdata.Nests = new List<NestMemento>();

                return pdata;
            }
        }

        public static void SaveParty()
        {
            BinaryFormatter formatter = new BinaryFormatter();


            FileStream stream = new FileStream(path, FileMode.Create);
            
            pdata.HeroParty = new List<HeroMemento>();

            foreach (Hero hero in HeroPartyDB.getHeroList())
            {
                if (hero.GetType().Name.Equals("Warrior"))
                {
                    pdata.HeroParty.Add(new WarriorMemento((Warrior)hero));
                }
                else if (hero.GetType().Name.Equals("Mage"))
                {
                    pdata.HeroParty.Add(new MageMemento((Mage)hero));
                }
                else if (hero.GetType().Name.Equals("Ranger"))
                {
                    pdata.HeroParty.Add(new RangerMemento((Ranger)hero));
                }
            }

            formatter.Serialize(stream, pdata);
            stream.Close();


        }

        public static PlayerData LoadParty()
        {

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                pdata.HeroParty = data.HeroParty;
                stream.Close();

                return data;

            }
            else
            {
                
                pdata.HeroParty = new List<HeroMemento>();

                return pdata;
            }
        }

        public static void SavePlayerInventory()
        {

            BinaryFormatter formatter = new BinaryFormatter();


            FileStream stream = new FileStream(path, FileMode.Create);
            pdata.PlayerTradeableInventory = PlayerInventory.TradeableInventory;
            pdata.PlayerMoney = PlayerInventory.Money;
            
            formatter.Serialize(stream, pdata);
            stream.Close();


        }

        public static PlayerData LoadPlayerInventory()
        {

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                pdata.PlayerTradeableInventory = data.PlayerTradeableInventory;
                pdata.PlayerMoney = data.PlayerMoney;
                
                stream.Close();

                return pdata;
            }
            else
            {
                pdata.PlayerTradeableInventory = new List<InventoryTradeable>(){
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
                pdata.PlayerMoney = 500.0;
                return pdata;
            }
        }

        public static PlayerData Pdata{
            get{
                return pdata;
            }
        }
    }
}