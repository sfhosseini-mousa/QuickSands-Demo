using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Sands
{

    public class Map : MonoBehaviour
    {
        [SerializeField] private GameObject[] locations = new GameObject[10];
        private List<string> connectedLocationsNames;
        [SerializeField] private Text clickedLocationName;
        [SerializeField] private Text currentLocation;
        private Location clickedlocation;
        private GameObject tradeTip;
        private GameObject goButton;
        public void Start()
        {

            tradeTip = GameObject.FindGameObjectWithTag("tradeTip");
            //tradeTip.SetActive(false);
           
            goButton = GameObject.FindGameObjectWithTag("goBtn");
            goButton.GetComponent<Selectable>().interactable = false;
            Player.LoadPlayer();
            connectedLocationsNames = new List<string>();
            foreach (var location in Player.CurrentLocation.NearbyTowns)
            {
                connectedLocationsNames.Add(LocationDB.getLocation(location - 1).LocationName);
            }

            foreach (GameObject location in locations)
            {
                if (location.name == Player.CurrentLocation.LocationName)
                {
                    location.GetComponent<Image>().color = new Color(0.1f, 0.7f, 0.0f);
                }
                if (connectedLocationsNames.Contains(location.name))
                {
                    location.GetComponent<Image>().color = Color.cyan;
                }
            }

            currentLocation.text = Player.CurrentLocation.LocationName;
            clickedLocationName.text = "";
        }


        public void onLocationClick() {

            foreach (var item in LocationDB.getLocationList())
            {
                if (item.LocationName == EventSystem.current.currentSelectedGameObject.name)
                    clickedlocation = item;
            }
            Debug.Log(clickedlocation.LocationName);

            if (!connectedLocationsNames.Contains(clickedlocation.LocationName))
                goButton.GetComponent<Selectable>().interactable = false;
            else
                goButton.GetComponent<Selectable>().interactable = true;


            clickedLocationName.text = clickedlocation.LocationName;
            if (connectedLocationsNames.Contains(clickedlocation.LocationName)) {
                Debug.Log("connected");
            }

            
        }

        public void goBtnClicked() {
            Player.LocationToTravelTo = clickedlocation;
            Player.SavePlayer();
            InnHeroes.GenerateHeroes();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Travel");
        }

    }
}