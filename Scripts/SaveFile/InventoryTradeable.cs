using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands
{
    [System.Serializable]
    public class InventoryTradeable
    {
        Tradeable ownedTradeable;
        int count;

        public InventoryTradeable(Tradeable tradeable, int count){
            this.ownedTradeable = tradeable;
            this.count = count;
        }

        public Tradeable OwnedTradeable{
            get{
                return ownedTradeable;
            }
            set{
                ownedTradeable = value;
            }
        }

        public int Count{
            get{
                return count;
            }
            set{
                count = value;
            }
        }
    }
}