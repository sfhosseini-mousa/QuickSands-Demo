using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public class QuestBoard
    {
        private List<Quest> quests = new List<Quest>();
        private System.Random random = new System.Random();
        private BattleQuest battleQuest;
       
        public void populateQuestList()
        {
            //generate 4 delivery quests
            for (int i = 0; i < 4; i++)
                quests.Add(new DeliveryQuest());

            //create a new list
            List<Nest> falseNestList = new List<Nest>();

            //get the list of nests
            foreach (Nest nest in NestDB.getNestList())
            {
                //check if any nests are false
                if (!nest.ActiveStatus)
                //if false add to new list
                    falseNestList.Add(nest);
            }

            //so long as theres at least 1 false acviteStatus nest
            if(falseNestList.Count > 0) {

                //generate a quest only from the available locations
                battleQuest = new BattleQuest(falseNestList[random.Next(0,falseNestList.Count)]);
                //add new battleQuest
                quests.Add(battleQuest);
            }
            else{
                //otherwise nests all are active
                //no battlequests available
                //generate 5th delivery quest
                quests.Add(new DeliveryQuest());
            }
        }     
    }
}
