using UnityEngine;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    [System.Serializable]
    public class FactionQuest : Quest
    {
        public int NumToKill { get; set; }
        public string ChosenFaction { get; set; }
        public int ChosenFactionNum { get; set; }
        public int KillCounter { get; set; } = 0;

        public FactionQuest() { }

        public FactionQuest(int i)
        {
            QuestName = "Faction";
            NumToKill = Random.Range(5, 11);

            string[] factionsToPickFrom = new string[2];

            switch (Player.CurrentLocation.Territory)
            {
                case 1:
                    factionsToPickFrom[0] = "Fara Empire";
                    factionsToPickFrom[1] = "Kaiserreich";
                    break;
                case 2:
                    factionsToPickFrom[0] = "Republic of Veden";
                    factionsToPickFrom[1] = "Kaiserreich";
                    break;
                case 3:
                    factionsToPickFrom[0] = "Fara Empire";
                    factionsToPickFrom[1] = "Republic of Veden";
                    break;
                default:
                    break;
            }

            ChosenFaction = factionsToPickFrom[Random.Range(0, 2)];

            switch (ChosenFaction)
            {
                case "Republic of Veden":
                    ChosenFactionNum = 1;
                    break;
                case "Fara Empire":
                    ChosenFactionNum = 2;
                    break;
                case "Kaiserreich":
                    ChosenFactionNum = 3;
                    break;
                default:
                    break;
            }

            QuestDescription = "Defeat " + NumToKill + " Caravans from the " + ChosenFaction + ".";

            QuestReward = Random.Range(3000, 5001);

            QuestLocation = Player.CurrentLocation;
        }

        public void IncreaseCounter(int factionCode)
        {
            if (factionCode == ChosenFactionNum)
            {
                KillCounter++;
                if (KillCounter >= NumToKill && !Completed)
                    Completed = true;
            }
        }

    }
}
