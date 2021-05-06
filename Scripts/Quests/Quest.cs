using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands {
    [System.Serializable]
    public class Quest {
        protected string questDescription;
        
        protected Location questLocation;

        protected int questReward;
        protected string distanceNote;

        public Quest() {}

        
        /////////// GETTERS AND SETTERS //////////
        public string QuestDescription {
            get {
                return questDescription;
            }
            set {
                questDescription = value;
            }
        }

        public Location QuestLocation {
            get {
                return questLocation;
            }
            set {
                questLocation = value;
            }
        }

        public int QuestReward {
            get {
                return questReward;
            }
            set {
                questReward = value;
            }
        }
    }
}  
