using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class Usable : Item
    {
        private int effectAmount { get; set; }
        public Usable(int id, string itemName, int weight, double price, int effectAmount) : base(id, itemName, weight, price)
        {
            this.effectAmount = effectAmount;
        }
    }
}