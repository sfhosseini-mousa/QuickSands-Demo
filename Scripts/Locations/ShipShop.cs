using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands{
    public class ShipShop : MonoBehaviour
    {
        [SerializeField] private GameObject buyBtn;
        [SerializeField] private GameObject upgradeBtn;
        [SerializeField] private Transform playerVehiclePosition;
        [SerializeField] private Transform upgradeVehiclePosition;
        private GameObject playerVehiclePrefab;
        private GameObject upgradeVehiclePrefab;
        private Transform instantiatedPlayerVehicle;
        private Transform instantiatedUpgradeVehicle;
        private int price;
        private Vehicle upgradeVehicle;
        [SerializeField] private Text money;
        [SerializeField] private Text currentVehicleHp;
        [SerializeField] private Text currentVehicleCargo;
        [SerializeField] private Text currentVehicleName;
        [SerializeField] private Text upgradeVehicleHp;
        [SerializeField] private Text upgradeVehicleCargo;
        [SerializeField] private Text upgradeVehiclePrice;
        [SerializeField] private Text upgradeVehicleName;
        [SerializeField] private GameObject[] currentVehicleStats = new GameObject[6];
        [SerializeField] private GameObject[] upgradeVehicleStats = new GameObject[8];

        void Start()
        {

            PlayerInventory.LoadPlayerInventory();
            PlayerInventory.Money += 100000;
            money.text = System.Convert.ToString(PlayerInventory.Money);
            PlayerInventory.SavePlayerInventory();
            upgradeBtn.SetActive(false);
            buyBtn.SetActive(false);

            if(Player.HasVehicle)
            {
                SetCurrentVehicleStats();
                upgradeBtn.SetActive(true);
                InstantiateVehicles();
                if(Player.CurrentVehicle.Name != "Leviathan")
                    SetUpgradeVehicleStats();
            }
            else
            {
                foreach (var item in currentVehicleStats)
                {
                    item.SetActive(false);
                }

                buyBtn.SetActive(true);
                upgradeVehiclePrefab = (GameObject)Resources.Load("Scout2Vehicle", typeof(GameObject));
                instantiatedUpgradeVehicle = Instantiate(upgradeVehiclePrefab.transform, upgradeVehiclePosition.position, Quaternion.identity);
                upgradeVehicle = new Vehicle(VehicleClassDB.getVehicle(0));
                price = VehicleClassDB.getVehicle(0).Price;
                SetUpgradeVehicleStats();
            }
        }

        

        public void InstantiateVehicles(){
            switch (Player.CurrentVehicle.Name)
            {
                case "Scout":
                    playerVehiclePrefab = (GameObject)Resources.Load("Scout2Vehicle", typeof(GameObject));
                    upgradeVehiclePrefab = (GameObject)Resources.Load("Warthog3Vehicle", typeof(GameObject));
                    upgradeVehicle = new Vehicle(VehicleClassDB.getVehicle(1));
                    price = VehicleClassDB.getVehicle(1).Price;
                    break;
                case "Warthog":
                    playerVehiclePrefab = (GameObject)Resources.Load("Warthog3Vehicle", typeof(GameObject));
                    upgradeVehiclePrefab = (GameObject)Resources.Load("Goliath4Vehicle", typeof(GameObject));
                    upgradeVehicle = new Vehicle(VehicleClassDB.getVehicle(2));
                    price = VehicleClassDB.getVehicle(2).Price;
                    break;
                case "Goliath":
                    playerVehiclePrefab = (GameObject)Resources.Load("Goliath4Vehicle", typeof(GameObject));
                    upgradeVehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));
                    upgradeVehicle = new Vehicle(VehicleClassDB.getVehicle(3));
                    price = VehicleClassDB.getVehicle(3).Price;
                    break;
                case "Leviathan":
                    playerVehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));
                    upgradeVehiclePosition.position = new Vector3(180f, 0.08f, 0f);
                    foreach (var item in upgradeVehicleStats)
                    {
                        item.SetActive(false);
                    }
                    upgradeVehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));
                    upgradeBtn.SetActive(false);
                    break;
                default:
                    break;
            }
            instantiatedPlayerVehicle = Instantiate(playerVehiclePrefab.transform, playerVehiclePosition.position, Quaternion.identity);
            instantiatedUpgradeVehicle = Instantiate(upgradeVehiclePrefab.transform, upgradeVehiclePosition.position, Quaternion.identity);
        }

        public void SetUpgradeVehicleStats(){
            upgradeVehicleName.text = upgradeVehicle.Name;
            upgradeVehicleCargo.text = System.Convert.ToString(upgradeVehicle.Capacity);
            upgradeVehicleHp.text = System.Convert.ToString(upgradeVehicle.VehicleHP);
            upgradeVehiclePrice.text = System.Convert.ToString(upgradeVehicle.Price);
        }

        public void SetCurrentVehicleStats(){
            currentVehicleName.text = Player.CurrentVehicle.Name;
            currentVehicleCargo.text = System.Convert.ToString(Player.CurrentVehicle.Capacity);
            currentVehicleHp.text = System.Convert.ToString(Player.CurrentVehicle.VehicleHP);

        }

        public void upgradeBtnOnClick(){
            if(PlayerInventory.Money >= price)
            {
                PlayerInventory.Money -= price;
                money.text = System.Convert.ToString(PlayerInventory.Money);
                PlayerInventory.SavePlayerInventory();
                
                if(Player.HasVehicle){
                    Destroy(instantiatedUpgradeVehicle.gameObject);
                    Destroy(instantiatedPlayerVehicle.gameObject);
                    switch (Player.CurrentVehicle.Name)
                    {
                        case "Scout":
                            Player.CurrentVehicle = new Vehicle(VehicleClassDB.getVehicle(1));
                            break;
                        case "Warthog":
                            Player.CurrentVehicle = new Vehicle(VehicleClassDB.getVehicle(2));
                            break;
                        case "Goliath":
                            Player.CurrentVehicle = new Vehicle(VehicleClassDB.getVehicle(3));
                            break;
                        default:
                            break;
                    }
                }
                else{
                    Destroy(instantiatedUpgradeVehicle.gameObject);
                    Player.HasVehicle = true;
                    Player.CurrentVehicle = new Vehicle(VehicleClassDB.getVehicle(0));
                    buyBtn.SetActive(false);
                    upgradeBtn.SetActive(true);
                    foreach (var item in currentVehicleStats)
                    {
                        item.SetActive(true);
                    }
                }
                Player.SavePlayer();
                InstantiateVehicles();
                SetCurrentVehicleStats();
                SetUpgradeVehicleStats();
            }
            else{
                //no money? sorry baby
            }
        }
    }
}