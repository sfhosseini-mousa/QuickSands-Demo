using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public class BattleQuest : Quest
    {
       
        private System.Random random = new System.Random();
        public BattleQuest()
        {
            this.questLocation = NestDB.getNest(random.Next(0, 3));
            this.questDescription = "Defeat the " + questLocation.LocationName + " Nest";

            if (Player.CurrentLocation.Territory != questLocation.Territory)
            {
                /////////IF DOESN'T WORK REMOVE MULTIPLIER IT'S ABOUT DOWNCASTING A PARENT TO A CHILD LOCATION TO NEST
                this.questReward = random.Next(400*((Nest)this.questLocation).Multiplier, 501*((Nest)this.questLocation).Multiplier);
                this.distanceNote = "Far away";
            }
            else
            {
                this.questReward = random.Next(300*((Nest)this.questLocation).Multiplier, 401*((Nest)this.questLocation).Multiplier);
                this.distanceNote = "Nearby";
            }
        }
        

        public BattleQuest(Nest questLocation)
        {
            this.questLocation = questLocation;

            Nest nest = (Nest)this.questLocation;
            this.questDescription = "Defeat the " + questLocation.LocationName + " Nest";

            if (Player.CurrentLocation.Territory != questLocation.Territory)
            {
                /////////IF DOESN'T WORK REMOVE MULTIPLIER IT'S ABOUT DOWNCASTING A PARENT TO A CHILD LOCATION TO NEST
                this.questReward = random.Next(400*nest.Multiplier, 501*nest.Multiplier);
                this.distanceNote = "Far away";
            }
            else
            {
                this.questReward = random.Next(300*nest.Multiplier, 401*nest.Multiplier);
                this.distanceNote = "Nearby";
            }
        }
    }
}