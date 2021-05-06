using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands
{
    public class Town : MonoBehaviour
    {
        [SerializeField] private Text locationName;
        [SerializeField] private Text Money;
        void Start()
        {
            Player.LoadPlayer();
            locationName.text = Player.CurrentLocation.LocationName;
            Money.text = System.Convert.ToString(PlayerInventory.Money);
        }
    }
}