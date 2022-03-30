using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    //shows the prices of each town in a pop up
    public class TradeTip : MonoBehaviour
    {
        private GameObject tradeTip;
        private Location clickedLocation;                           //the location player hovers over
        [SerializeField] private Text locationNameText;
        [SerializeField] private Text[] tradePrices = new Text[10]; //the prices as text in the scene
        [SerializeField] private Text[] profitPrices = new Text[10]; //the profit of trades as text in the scene
        private Vector3 position;
        [SerializeField] private RectTransform TradeTipWindow;      //the position of the TradeTip window
        [SerializeField] private Text factionName;

        void Start()
        {
            //find tradeTip from the scene
            tradeTip = GameObject.FindGameObjectWithTag("tradeTip");
        }

        //when the player hovers over a town move tradeTip to the position
        public void OnLocationClick()
        {
            tradeTip.SetActive(true);
            locationNameText.text = name;
            foreach (var location in LocationDB.getLocationList())
            {
                if (location.LocationName == name)
                    clickedLocation = location;
            }


            var locationName = name;
            tradeTip.SetActive(true);
            

            for (int i = 0; i < clickedLocation.TradePrices.Count; i++)
            {
                tradePrices[i].text = System.Convert.ToString((int)(clickedLocation.TradePrices[i] - clickedLocation.TradePrices[i] * 15/100));
            }

            //set the faction of tradeTip
            if (clickedLocation.Territory == 1)
            {
                factionName.text = "Republic of Veden";
                factionName.color = Color.blue;  //blue
            }
            else if (clickedLocation.Territory == 2)
            {
                factionName.text = "Fara Empire";
                factionName.color = Color.green;
            }
            else if (clickedLocation.Territory == 3)
            {
                factionName.text = "The Kaiserreich";
                factionName.color = Color.red;
            }
            else
                factionName.text = "123 Fakestreet";


            for (int i = 0; i < clickedLocation.TradePrices.Count; i++)
            {

                if (Player.CurrentLocation.LocationName == clickedLocation.LocationName)
                {

                    profitPrices[i].text = "";


                }
                else
                {
                    int profit = (int)(clickedLocation.TradePrices[i] - clickedLocation.TradePrices[i] * 15 / 100) - (int)LocationDB.getLocation(Player.CurrentLocation.Id - 1).TradePrices[i];
                    profitPrices[i].text = System.Convert.ToString(profit);

                    if (profit > 0)
                    {
                        profitPrices[i].text = "+" + profitPrices[i].text;
                        profitPrices[i].color = Color.green;
                    }
                    else
                    {
                        profitPrices[i].color = Color.red;
                    }

                }
            }
        }
    }

}