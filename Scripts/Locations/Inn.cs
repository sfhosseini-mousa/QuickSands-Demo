using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands{
    public class Inn : MonoBehaviour
    {
        [SerializeField] private Transform[] heroHS = new Transform[3];
        [SerializeField] private GameObject[] hireButtons = new GameObject[3];
        [SerializeField] private GameObject hirePopUp;
        private GameObject[] heroPrefabs = new GameObject[3];
        private Transform[] heroTransforms = new Transform[3];
        private int[] price = new int[3];
        [SerializeField] private UnityEngine.UI.Text heroLevel;
        [SerializeField] private UnityEngine.UI.Text heroPrice;
        [SerializeField] private UnityEngine.UI.Text errorText;
        [SerializeField] private Text money;
        [SerializeField] private Text partySize;
        [SerializeField] private Text maxPartySize;
        private int selectedHeroIndex;
        
        void Start()
        {
            PlayerInventory.LoadPlayerInventory();
            money.text = System.Convert.ToString(PlayerInventory.Money);
            partySize.text = System.Convert.ToString(HeroPartyDB.getHeroList().Count);

            if(Player.HasVehicle)
                maxPartySize.text = System.Convert.ToString(Player.CurrentVehicle.PartySize);
            else
                maxPartySize.text = "1";

            hirePopUp.SetActive(false);
            foreach (var button in hireButtons)
            {
                button.SetActive(false);
            }

            InnHeroes.GenerateHeroes();
            InstatiateHeroes();
        }
        
        public void InstatiateHeroes(){
            for (int i = 0; i < InnHeroes.InnHeroesList.Count; i++)
            {
                heroPrefabs[i] = (GameObject)Resources.Load(InnHeroes.InnHeroesList[i].GetType().Name, typeof(GameObject));
                InnHeroes.InnHeroesList[i].setSkin(heroPrefabs[i]);
                int skinTire = InnHeroes.InnHeroesList[i].SkinTire;
                switch (skinTire)
                {
                    case 1:
                        price[i] = 1000;
                        break;
                    case 2:
                        price[i] = 2500;
                        break;
                    case 3:
                        price[i] = 4000;
                        break;
                    case 4:
                        price[i] = 6000;
                        break;
                    case 5:
                        price[i] = 10000;
                        break;
                    default:
                    break;
                }
                heroTransforms[i] = Instantiate(heroPrefabs[i].transform, heroHS[i].position, Quaternion.identity);
                hireButtons[i].SetActive(true);
            }
        }

        public void HeroOnClick(){
            hirePopUp.SetActive(true);
            selectedHeroIndex = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 1;
            heroLevel.text = System.Convert.ToString(InnHeroes.InnHeroesList[selectedHeroIndex].SkinTire);
            heroPrice.text = System.Convert.ToString(price[selectedHeroIndex]);
        }

        public void popUpCloseOnClick(){
            hirePopUp.SetActive(false);
            errorText.text = "";
        }

        public void popUpYesOnClick(){
            if(Player.HasVehicle && HeroPartyDB.getHeroList().Count < 5 && Player.CurrentVehicle.PartySize > HeroPartyDB.getHeroList().Count){
                PlayerInventory.LoadPlayerInventory();
                if(PlayerInventory.Money >= price[selectedHeroIndex]){
                    PlayerInventory.Money -= price[selectedHeroIndex];
                    PlayerInventory.SavePlayerInventory();

                    switch (InnHeroes.InnHeroesList[selectedHeroIndex].GetType().Name)
                    {
                        case "Warrior":
                            HeroPartyDB.addHero(new Warrior((Warrior)InnHeroes.InnHeroesList[selectedHeroIndex]));
                            break;

                        case "Mage":
                            HeroPartyDB.addHero(new Mage((Mage)InnHeroes.InnHeroesList[selectedHeroIndex]));
                            break;

                        case "Ranger":
                            HeroPartyDB.addHero(new Ranger((Ranger)InnHeroes.InnHeroesList[selectedHeroIndex]));
                            break;

                        default:
                            break;
                    }
                    HeroPartyDB.SaveParty();
                    money.text = System.Convert.ToString(PlayerInventory.Money);
                    partySize.text = System.Convert.ToString(HeroPartyDB.getHeroList().Count);
                    hirePopUp.SetActive(false);
                    hireButtons[selectedHeroIndex].SetActive(false);
                }
                else{
                    errorText.text = "Not Enough Gold";
                }
            }
            else if(HeroPartyDB.getHeroList().Count == 5){
                errorText.text = "Maximum number of Heroes";
            }
            else if(!Player.HasVehicle){
                errorText.text = "You do not own a Vehicle";
            }
            else{
                errorText.text = "Vehicle is Full";
            }
            
        }
    }
}