using UnityEngine;
using System.Collections.Generic;

namespace Sands
{
    [System.Serializable]
    public class TransportQuest : Quest
    {

        public int[] LocalTownIDs { get; set; }
        public int DestinationID{ get; set; }

        public HeroMemento QuestHero{ get; set; }

        public bool HeroJoinChecked { get; set; }


        //default constructor
        public TransportQuest(){ }

        public TransportQuest(int i) {

            //get current location
            //get connected cities

            LocalTownIDs = Player.CurrentLocation.NearbyTowns;

            //choose destination town from connected
           
            DestinationID = LocalTownIDs[Random.Range(0,LocalTownIDs.Length)];

            questLocation = LocationDB.getLocation(DestinationID - 1);

            int rand = Random.Range(0, 5);

            switch (rand)
            {
                case 0:
                    QuestHero = new WarriorMemento((Warrior)HeroClassDB.getHero(rand));
                    break;
                case 1:
                    QuestHero = new MageMemento((Mage)HeroClassDB.getHero(rand));
                    break;
                case 2:
                    QuestHero = new RangerMemento((Ranger)HeroClassDB.getHero(rand));
                    break;
                case 3:
                    QuestHero = new WizardMemento((Wizard)HeroClassDB.getHero(rand));
                    break;
                case 4:
                    QuestHero = new SpearmanMemento((Spearman)HeroClassDB.getHero(rand));
                    break;
                default:
                    break;
            }
            QuestHero.IsQuestHero = true;
            QuestHero.SkinTire = Random.Range(1,4);

            this.questName = "Transport";
            this.questDescription = "Transport Hero to " + questLocation.LocationName;
            this.questReward = UnityEngine.Random.Range(300, 501);
            this.distanceNote = "Next Town";
        }


        public void AddQuestHero()
        {
            switch (QuestHero.Id)
            {
                case 1:
                    HeroPartyDB.addHero(new Warrior((WarriorMemento)QuestHero));
                    break;
                case 2:
                    HeroPartyDB.addHero(new Mage((MageMemento)QuestHero));
                    break;
                case 3:
                    HeroPartyDB.addHero(new Ranger((RangerMemento)QuestHero));
                    break;
                case 4:
                    HeroPartyDB.addHero(new Wizard((WizardMemento)QuestHero));
                    break;
                case 5:
                    HeroPartyDB.addHero(new Spearman((SpearmanMemento)QuestHero));
                    break;
                default:
                    break;
            }
            HeroPartyDB.SaveParty();
        }

        public void RemoveQuestHero() {

            for (int i=0; i < HeroPartyDB.getHeroList().Count; i++)
            {
                if (HeroPartyDB.getHeroList()[i].Id == QuestHero.Id && HeroPartyDB.getHeroList()[i].SkinTire == QuestHero.SkinTire)
                {
                    HeroPartyDB.getHeroList().RemoveAt(i);
                    HeroPartyDB.SaveParty();
                    break;
                }
            }
        }

    }
}