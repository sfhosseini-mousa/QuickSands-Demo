using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands
{
    public class GeneralGoods : MonoBehaviour
    {
        private string selectedItem;
        [SerializeField] private GameObject[] tradeableButtons = new GameObject[10];
        [SerializeField] private Text[] tradeablePrices = new Text[10];
        [SerializeField] private Text[] tradeablesOwned = new Text[10];
        [SerializeField] private GameObject buyPopUp;
        [SerializeField] private GameObject sellPopUp;
        [SerializeField] private Text goodNameBuy;
        [SerializeField] private Text goodNameSell;
        [SerializeField] private Text goodPriceBuy;
        [SerializeField] private Text goodPriceSell;
        [SerializeField] private Text goodCountBuy;
        [SerializeField] private Text goodCountSell;
        [SerializeField] private Text townName;
        [SerializeField] private GameObject Gauge;
        [SerializeField] private GameObject countSlider;
        [SerializeField] private GameObject sellCountSlider;
        private Location playerLocation;
        private float capacity = 0;
        private float carrying = 0;

        [SerializeField] private Text money;
        [SerializeField] private Text errorTextBuy;
        [SerializeField] private Text errorTextSell;

        private Tradeable myTradeable;

        public void Start(){
            selectedItem = "Cloth";
            Player.LoadPlayer();
            money.text = System.Convert.ToString(PlayerInventory.Money);
            foreach (var item in LocationDB.getLocationList())
            {
                if(item.LocationName == Player.CurrentLocation.LocationName){
                    playerLocation = item;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                tradeablePrices[i].text = System.Convert.ToString(playerLocation.TradePrices[i]);
            }

            for (int i = 0; i < 10; i++)
            {
                tradeablesOwned[i].text = "(" + System.Convert.ToString(PlayerInventory.TradeableInventory[i].Count) + ")";
            }

            townName.text = playerLocation.LocationName;

            if(buyPopUp.activeInHierarchy){
                goodNameBuy.text = selectedItem;
                goodCountBuy.text = "0";
                goodPriceBuy.text = "0";
            }
            goodNameBuy.text = selectedItem;
            goodCountBuy.text = "0";
            goodPriceBuy.text = "0";

            foreach (var hero in HeroPartyDB.getHeroList())
            {
                capacity += hero.Capacity;
            }
            if(Player.HasVehicle)
                capacity += Player.CurrentVehicle.Capacity;

            calculateCarrying();

            Gauge.GetComponent<Image>().fillAmount = carrying / capacity;
            myTradeable = TradeableDatabase.getTradeable(0);
            setCountSlider();
        }

        public void ItemOnClick(){
            selectedItem = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
            foreach (var tradeable in TradeableDatabase.getTradeableList())
            {
                if (tradeable.ItemName == selectedItem)
                {
                    myTradeable = tradeable;
                }
            }
            if(buyPopUp.activeInHierarchy){
                goodNameBuy.text = selectedItem;
                goodCountBuy.text = "0";
                goodPriceBuy.text = "0";
                setCountSlider();

                if(((int)capacity - (int)carrying) / myTradeable.Weight == 0)
                    errorTextBuy.text = "Max Loot";
                else
                    errorTextBuy.text = "";
            }
            if(sellPopUp.activeInHierarchy){
                goodNameSell.text = selectedItem;
                goodCountSell.text = "0";
                goodPriceSell.text = "0";
                setSellCountSlider();
            }
        }

        public void setCountSlider(){
            int maxValue = ((int)capacity - (int)carrying) / myTradeable.Weight;
            countSlider.GetComponent<Slider>().maxValue = maxValue;
            countSlider.GetComponent<Slider>().value = 0;

            
        }

        public void setSellCountSlider(){
            sellCountSlider.GetComponent<Slider>().maxValue = PlayerInventory.TradeableInventory[myTradeable.Id - 1].Count;
            sellCountSlider.GetComponent<Slider>().value = 0;

            if(PlayerInventory.TradeableInventory[myTradeable.Id - 1].Count == 0)
                errorTextSell.text = "No Loot";
            else
                errorTextSell.text = "";
        }

        public void calculateCarrying(){
            carrying = 0;
            foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
            {
                carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
            }
        }
        
        public void BuySellOnClick(){
            foreach (var tradeable in TradeableDatabase.getTradeableList())
            {
                if (tradeable.ItemName == selectedItem)
                {
                    myTradeable = tradeable;
                }
            }
            if(buyPopUp.activeInHierarchy){
                goodNameBuy.text = selectedItem;
                goodCountBuy.text = "0";
                goodPriceBuy.text = "0";
                setCountSlider();
            }
            if(sellPopUp.activeInHierarchy){
                goodNameSell.text = selectedItem;
                goodCountSell.text = "0";
                goodPriceSell.text = "0";
                setSellCountSlider();
            }
        }

        public void sliderOnValueChanged()
        {
            goodCountBuy.text = System.Convert.ToString((int)countSlider.GetComponent<Slider>().value);
            goodPriceBuy.text = System.Convert.ToString(playerLocation.TradePrices[myTradeable.Id - 1] * (int)countSlider.GetComponent<Slider>().value);
        }

        public void sellsliderOnValueChanged()
        {
            goodCountSell.text = System.Convert.ToString((int)sellCountSlider.GetComponent<Slider>().value);
            goodPriceSell.text = System.Convert.ToString(playerLocation.TradePrices[myTradeable.Id - 1] * (int)sellCountSlider.GetComponent<Slider>().value);
        }

        public void confirmSellOnClick(){
            if((int)sellCountSlider.GetComponent<Slider>().value != 0){
                PlayerInventory.RemoveFromInventory(myTradeable.Id, (int)(sellCountSlider.GetComponent<Slider>().value));
                PlayerInventory.Money += playerLocation.TradePrices[myTradeable.Id - 1] * (int)sellCountSlider.GetComponent<Slider>().value;
                PlayerInventory.SavePlayerInventory();
                goodCountSell.text = "0";
                goodPriceSell.text = "0";

                tradeablesOwned[myTradeable.Id - 1].text = "(" + (int)(PlayerInventory.TradeableInventory[myTradeable.Id - 1].Count) + ")";

                calculateCarrying();
                
                setSellCountSlider();
                money.text = System.Convert.ToString(PlayerInventory.Money);
                Gauge.GetComponent<Image>().fillAmount = carrying / capacity;
            }
        }

        public void confirmBuyOnClick(){
            if((int)countSlider.GetComponent<Slider>().value != 0){
                if(PlayerInventory.Money >= playerLocation.TradePrices[myTradeable.Id - 1] * (int)countSlider.GetComponent<Slider>().value){
                    PlayerInventory.AddToInventory(myTradeable.Id, (int)(countSlider.GetComponent<Slider>().value));
                    PlayerInventory.SavePlayerInventory();
                    goodCountBuy.text = "0";
                    goodPriceBuy.text = "0";

                    tradeablesOwned[myTradeable.Id - 1].text = "(" + (int)(PlayerInventory.TradeableInventory[myTradeable.Id - 1].Count) + ")";

                    calculateCarrying();
                    
                    setCountSlider();
                    money.text = System.Convert.ToString(PlayerInventory.Money);
                    Gauge.GetComponent<Image>().fillAmount = carrying / capacity;
                }
                else{
                    errorTextBuy.text = "No Gold";
                }
            }
        }
    }
}