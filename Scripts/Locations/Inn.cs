using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//COMMENTED BY FARAMARZ HOSSEINI


namespace Sands
{
    public class Inn : MonoBehaviour
    {
        [SerializeField] private GameObject hirePopUp;
        private GameObject[] heroPrefabs = new GameObject[3];
        private Transform[] heroTransforms = new Transform[3];
        private int[] price = new int[3];                                       //price of each hero
        [SerializeField] private Text heroLevel;
        [SerializeField] private Text heroPrice;
        [SerializeField] private Text errorText;
        [SerializeField] private Text money;
        [SerializeField] private Text partySize;
        [SerializeField] private Text maxPartySize;
        private int selectedHeroIndex;
        [SerializeField] private GameObject[] bannerLocations;
        private static bool[] hired = new bool[3];                              //checks if each hero is hired previously
        private static Location savedLocation;                                  //remembers the players previous location
        [SerializeField] private AudioSource hireSound;
        [SerializeField] private GameObject heroCardSpawn;
        private GameObject[] heroCards = new GameObject[3];
        private HeroStatChecker statChecker = new HeroStatChecker();
        [SerializeField] private DanielLochner.Assets.SimpleScrollSnap.SimpleScrollSnap simpleScrollSnap;

        void Start()
        {
            PlayerInventory.LoadPlayerInventory();
            money.text = System.Convert.ToString(PlayerInventory.Money);

            //set the party size in the scene
            partySize.text = System.Convert.ToString(HeroPartyDB.getHeroList().Count);

            //set the maximum party size
            if(Player.HasVehicle)
                maxPartySize.text = System.Convert.ToString(Player.CurrentVehicle.PartySize);
            else
                maxPartySize.text = "1";

            hirePopUp.SetActive(false);


            CheckLocation();
            InstatiateHeroes();
            StartCoroutine(AnimateCards());
            InstantiateBanners();
        }
        
        void CheckLocation()
        {
            try
            {
                if(savedLocation.LocationName != Player.CurrentLocation.LocationName)
                {
                    for (int i = 0; i < hired.Length; i++)
                    {
                        hired[i] = false;
                    }

                    savedLocation = Player.CurrentLocation;
                }
            }
            catch (System.Exception)
            {
                savedLocation = Player.CurrentLocation;
            }
        }

        //instantiates up to 3 heroes for the Inn
        public void InstatiateHeroes(){

            GameObject heroCardPrefab = (GameObject)Resources.Load("Hero Card", typeof(GameObject));

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
                    default:
                    break;
                }

                heroCards[i] = Instantiate(heroCardPrefab);


                //heroCards[i].transform.SetParent(heroCardSpawn.transform);
                //heroCards[i].transform.localScale = Vector3.one;

                HeroCard heroCard = heroCards[i].GetComponent<HeroCard>();

                heroCard.heroNameAndLevel.text = InnHeroes.InnHeroesList[i].GetType().Name + " Level " + skinTire;
                heroCard.heroHP.text = statChecker.GetHeroHealth(InnHeroes.InnHeroesList[i]).ToString();
                heroCard.heroDamage.text = statChecker.GetHeroDamage(InnHeroes.InnHeroesList[i]).ToString();
                heroCard.heroPrice.text = price[i] + " Coin";
                heroTransforms[i] = Instantiate(heroPrefabs[i], heroCard.heroSpawn.transform).transform;
                heroTransforms[i].localPosition = Vector3.zero;
                heroTransforms[i].localScale = new Vector3(155f, 155f, 0f);

                simpleScrollSnap.Add(heroCards[i], i);
                Destroy(heroCards[i]);
            }

            for (int i = 0; i < simpleScrollSnap.Panels.Length; i++)
            {
                int index = i;
                simpleScrollSnap.Panels[i].GetComponent<HeroCard>().hireBtn.onClick.AddListener(() =>
                {
                    HeroOnClick(index);
                });

                if (hired[i])
                {
                    DeactivateHeroCard(i);
                }
            }
        }

        private IEnumerator AnimateCards()
        {
            for (int i = 0; i < InnHeroes.InnHeroesList.Count; i++)
            {
                yield return new WaitForSeconds(0.4f);
                simpleScrollSnap.GoToNextPanel();
            }
        }

        private void DeactivateHeroCard(int i)
        {
            simpleScrollSnap.Panels[i].GetComponent<HeroCard>().hireBtn.GetComponent<Selectable>().interactable = false;
            simpleScrollSnap.Panels[i].GetComponent<HeroCard>().hireBtn.GetComponentInChildren<Text>().text = "Hired";
        }

        //sets the values of hirePopUp
        public void HeroOnClick(int index){
            selectedHeroIndex = index;
            if (!hired[selectedHeroIndex])
            {
                hirePopUp.SetActive(true);
                heroLevel.text = System.Convert.ToString(InnHeroes.InnHeroesList[selectedHeroIndex].SkinTire);
                heroPrice.text = System.Convert.ToString(price[selectedHeroIndex]);
            }
        }

        public void popUpCloseOnClick(){
            hirePopUp.SetActive(false);
            errorText.text = "";
        }

        public void popUpYesOnClick(){
            //checks if everything is ready to hire a new hero
            if(Player.HasVehicle && HeroPartyDB.getHeroList().Count < 5 && Player.CurrentVehicle.PartySize > HeroPartyDB.getHeroList().Count){
                PlayerInventory.LoadPlayerInventory();
                if(PlayerInventory.Money >= price[selectedHeroIndex]){
                    hireSound.Play();

                    //deducts the money from the player
                    PlayerInventory.Money -= price[selectedHeroIndex];
                    PlayerInventory.SavePlayerInventory();

                    //adds the hero to the party
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

                        case "Spearman":
                            HeroPartyDB.addHero(new Spearman((Spearman)InnHeroes.InnHeroesList[selectedHeroIndex]));
                            break;

                        case "Wizard":
                            HeroPartyDB.addHero(new Wizard((Wizard)InnHeroes.InnHeroesList[selectedHeroIndex]));
                            break;

                        default:
                            break;
                    }

                    HeroPartyDB.SaveParty();

                    hired[selectedHeroIndex] = true;

                    DeactivateHeroCard(selectedHeroIndex);

                    money.text = System.Convert.ToString(PlayerInventory.Money);
                    partySize.text = System.Convert.ToString(HeroPartyDB.getHeroList().Count);
                    
                    hirePopUp.SetActive(false);
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

        //based on the location of the palyer sets the banners up
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
            }
        }
    }
}