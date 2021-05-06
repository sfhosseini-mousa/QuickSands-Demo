using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands
{
    public class TradeTip : MonoBehaviour
    {
        private GameObject tradeTip;
        private Location hoveredLocation;
        [SerializeField] private Text locationNameText;
        [SerializeField] private Text[] tradePrices = new Text[10];
        private Vector3 position;
        [SerializeField] private RectTransform TradeTipWindow;
        void Start()
        {
            tradeTip = GameObject.FindGameObjectWithTag("tradeTip");

        }

        public void OnMouseOver()
        {

        
            

            tradeTip.SetActive(true);
            locationNameText.text = name;
            foreach (var location in LocationDB.getLocationList())
            {
                if (location.LocationName == name)
                    hoveredLocation = location;
            }


            var locationName = name;
            tradeTip.SetActive(true);
            
            position = new Vector3(190f, -50f, 0f);
            TradeTipWindow.localPosition = position;

            for (int i = 0; i < hoveredLocation.TradePrices.Count; i++)
            {
                tradePrices[i].text = System.Convert.ToString(hoveredLocation.TradePrices[i]);
            }
        }

        public void OnMouseExit()
        {
            tradeTip.SetActive(false);
        }


    }

}