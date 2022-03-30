using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    public class Town : MonoBehaviour
    {
        [SerializeField] private Text locationName;
        [SerializeField] private Text Money;
        [SerializeField] private GameObject[] bannerLocations;
        [SerializeField] private Transform vehicleBS;           //location of the vehicle in the scene

        [SerializeField] private GameObject[] QuestHeroStations;
        [SerializeField] private GameObject[] QuestHeroButtons;
        private int[] transportQuestsIndexes = new int[4];
        [SerializeField] private GameObject hireQuestHeroPopUp;
        [SerializeField] private GameObject thankYouQuestHeroPopUp;
        [SerializeField] private Text hireQuestHeroPopUpError;
        private int selectedHero = 0;
        private static bool[] clickedQuestHeores = new bool[4];
        private GameObject[] questHeroPrefabs = new GameObject[4];
        [SerializeField] private Transform questHeroInPopUpPos;
        private GameObject instQuestHeroInPopUp;

        

        void Start()
        {
            Player.LoadPlayer();
            locationName.text = Player.CurrentLocation.LocationName;
            Money.text = System.Convert.ToString(PlayerInventory.Money);
            InstantiateBanners();
            InstantiatePlayerVehicle();
            InstantiateQuestHeroes();
        }

        

        //loads the right banners into the scene
        void InstantiateBanners()
        {
            GameObject banner = null;

            switch (Player.CurrentLocation.Territory)
            {
                case 1:
                    banner = (GameObject)Resources.Load("BannerBlue", typeof(GameObject));
                    break;
                case 3:
                    banner = (GameObject)Resources.Load("BannerRed", typeof(GameObject));
                    break;
                case 2:
                    banner = (GameObject)Resources.Load("BannerGreen", typeof(GameObject));
                    break;
                default:
                    break;
            }

            foreach (var loc in bannerLocations)
            {
                GameObject instantiatedBanner = Instantiate(banner);
                instantiatedBanner.transform.position = loc.transform.position;
                instantiatedBanner.transform.localScale = new Vector3(2f, 2f, 2f);
            }
        }

        //instantiates the player's vehicle into the scene
        void InstantiatePlayerVehicle()
        {
            GameObject vehiclePrefab;
            GameObject instantiatedVehicle = null;

            if (Player.HasVehicle)
            {
                switch (Player.CurrentVehicle.Name)
                {
                    case "Scout":
                        vehiclePrefab = (GameObject)Resources.Load("Scout2Vehicle", typeof(GameObject));
                        instantiatedVehicle = Instantiate(vehiclePrefab, vehicleBS.position, Quaternion.Euler(0, 180, 0));

                        break;

                    case "Warthog":
                        vehiclePrefab = (GameObject)Resources.Load("Warthog3Vehicle", typeof(GameObject));
                        instantiatedVehicle = Instantiate(vehiclePrefab, vehicleBS.position, Quaternion.Euler(0, 180, 0));

                        break;

                    case "Goliath":
                        vehiclePrefab = (GameObject)Resources.Load("Goliath4Vehicle", typeof(GameObject));
                        instantiatedVehicle = Instantiate(vehiclePrefab, vehicleBS.position, Quaternion.Euler(0, 180, 0));

                        break;

                    case "Leviathan":
                        vehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));
                        instantiatedVehicle = Instantiate(vehiclePrefab, vehicleBS.position, Quaternion.Euler(0, 180, 0));

                        break;
                    default:
                        break;
                }

                instantiatedVehicle.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                instantiatedVehicle.GetComponent<SkeletonAnimation>().AnimationName = "";
            }
        }


        void InstantiateQuestHeroes()
        {
            int i = 0;
            int j = 0;
            foreach (var quest in Player.AcceptedQuests)
            {
                if (quest.QuestName == "Transport" && quest.Completed && quest.QuestLocation.LocationName == Player.CurrentLocation.LocationName && ((TransportQuest)quest).HeroJoinChecked == false)
                {
                    //look into the quest for the specific hero from that quest

                    switch (((TransportQuest)quest).QuestHero.Id)
                    {
                        case 1:
                            questHeroPrefabs[i] = (GameObject)Resources.Load("Warrior", typeof(GameObject));
                            break;
                        case 2:
                            questHeroPrefabs[i] = (GameObject)Resources.Load("Mage", typeof(GameObject));
                            break;
                        case 3:
                            questHeroPrefabs[i] = (GameObject)Resources.Load("Ranger", typeof(GameObject));
                            break;
                        case 4:
                            questHeroPrefabs[i] = (GameObject)Resources.Load("Wizard", typeof(GameObject));
                            break;
                        case 5:
                            questHeroPrefabs[i] = (GameObject)Resources.Load("Spearman", typeof(GameObject));
                            break;
                        default:
                            break;
                    }
                    
                    //instatiate at the next available hero station
                    GameObject instQuestHero = Instantiate(questHeroPrefabs[i], QuestHeroStations[i].transform);

                    instQuestHero.transform.localScale = new Vector3(3, 3, 1);
                    instQuestHero.transform.localPosition = Vector3.zero;
                    //change position of the next available button

                    QuestHeroButtons[i].transform.position = QuestHeroStations[i].transform.position + new Vector3(0,1,0);

                    if (clickedQuestHeores[i])
                        QuestHeroButtons[i].GetComponent<Selectable>().interactable = false;

                    transportQuestsIndexes[i] = j;
                    i++;
                }
                j++;
            }
        }

        public void QuestHeroOnClick()
        {
            selectedHero = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 1;
            if(Random.Range(0,100) < 10)
            {
                hireQuestHeroPopUp.SetActive(true);
            }
            else
            {
                thankYouQuestHeroPopUp.SetActive(true);
            }

            instQuestHeroInPopUp = Instantiate(questHeroPrefabs[selectedHero], questHeroInPopUpPos);
            instQuestHeroInPopUp.transform.localScale = new Vector3(3, 3, 1);
            instQuestHeroInPopUp.transform.localPosition = Vector3.zero;

            clickedQuestHeores[selectedHero] = true;
            QuestHeroButtons[selectedHero].GetComponent<Selectable>().interactable = false;
            ((TransportQuest)Player.AcceptedQuests[transportQuestsIndexes[selectedHero]]).HeroJoinChecked = true;
            Player.SavePlayer();
        }

        public void DestoyinstQuestHeroInPopUp()
        {
            Destroy(instQuestHeroInPopUp);
        }

        public void HireQuestHeroPopUpAcceptOnClick()
        {
            if (Player.CurrentVehicle.PartySize > Player.CurrentVehicle.Passangers.Count)
            {
                //switch (((TransportQuest)Player.AcceptedQuests[transportQuestsIndexes[selectedHero]]).QuestHero.Id)
                //{
                //    case 1:
                //        HeroPartyDB.addHero(new Warrior((WarriorMemento)((TransportQuest)Player.AcceptedQuests[transportQuestsIndexes[selectedHero]]).QuestHero));
                //        break;
                //    case 2:
                //        HeroPartyDB.addHero(new Mage((MageMemento)((TransportQuest)Player.AcceptedQuests[transportQuestsIndexes[selectedHero]]).QuestHero));
                //        break;
                //    case 3:
                //        HeroPartyDB.addHero(new Ranger((RangerMemento)((TransportQuest)Player.AcceptedQuests[transportQuestsIndexes[selectedHero]]).QuestHero));
                //        break;
                //    case 4:
                //        HeroPartyDB.addHero(new Wizard((WizardMemento)((TransportQuest)Player.AcceptedQuests[transportQuestsIndexes[selectedHero]]).QuestHero));
                //        break;
                //    case 5:
                //        HeroPartyDB.addHero(new Spearman((SpearmanMemento)((TransportQuest)Player.AcceptedQuests[transportQuestsIndexes[selectedHero]]).QuestHero));
                //        break;
                //    default:
                //        break;
                //}
                ((TransportQuest)Player.AcceptedQuests[transportQuestsIndexes[selectedHero]]).AddQuestHero();
                QuestHeroButtons[selectedHero].GetComponent<Selectable>().interactable = false;
            }
            else
            {
                hireQuestHeroPopUpError.text = "Party Full";
            }
        }

        //adding money for the demo
        public void AddMoney()
        {
            PlayerInventory.LoadPlayerInventory();
            PlayerInventory.Money += 100000;
            Money.text = System.Convert.ToString(PlayerInventory.Money);
            PlayerInventory.SavePlayerInventory();
        }
    }
}