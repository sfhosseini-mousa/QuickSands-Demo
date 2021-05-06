using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Spine.Unity;

namespace Sands
{
    public enum BattleState2 { START, PLAYERTURN, PLAYERBUSY, ENEMYTURN, ENEMYBUSY, WON, LOST }
    
    public class BattleSystem2 : MonoBehaviour
    {
        GameObject defeatPopUp;
        GameObject victoryPopUp;
        GameObject lootPopUp;
        public BattleState2 battleState;
        private List<CharAction> playersCharActions;
        private List<CharAction> enemysCharActions;
        [SerializeField] private Text dialogueText;
        [SerializeField] private Text lootText;
        private Vehicle playerVehicle = new Vehicle();
        private Vehicle enemyVehicle = new Vehicle();
        private GameObject[] heroPrefabs;
        private GameObject[] enemyPrefabs = new GameObject[1];
        private GameObject heroVehiclePrefab;
        private GameObject enemyVehiclePrefab;
        private Transform[] instantiatedHeroes;
        private Transform[] instantiatedEnemies;
        private Transform instantiatedHeroVehicle;
        private Transform instantiatedEnemyVehicle;
        public BattleHUD playerHUD;
        public BattleHUD enemyHUD;
        //loot pop up variables
        [SerializeField] Text lootOneName;
        [SerializeField] Text lootTwoName;
        [SerializeField] Text lootOneAmount;
        [SerializeField] Text lootTwoAmount;
        [SerializeField] Text inventoryOneAmount;
        [SerializeField] Text inventoryTwoAmount;
        Tradeable item1;
        Tradeable item2;
        int randomID = 0;
        int randomID2 = 0;
        int randomAmount = 0;
        int randomAmount2 = 0;
        System.Random randy = new System.Random();
        //
        private Hero enemyHero;


        private CharAction activeChar;
        public static BattleSystem2 instance;

        private int numberOfBattles;
        private int battlesWon = 1;

        private int allCharsUsedTurns = 0;

        [SerializeField] private GameObject fillHeals;
        [SerializeField] private GameObject fillCargo;
        private float numberOfHeals;

        private double loot;

        List<CharAction> charactersActions;


        //Object Transform Hero positions on Vehicles
        [SerializeField] private Transform heroBS1;
        [SerializeField] private Transform heroBS2;
        [SerializeField] private Transform heroBS3;
        [SerializeField] private Transform heroBS4;
        [SerializeField] private Transform heroBS5;

        //Object Transform Enemy positions on Vehicles
        [SerializeField] private Transform EnemyBS1;
        [SerializeField] private Transform EnemyBS2;
        [SerializeField] private Transform EnemyBS3;
        [SerializeField] private Transform EnemyBS4;
        [SerializeField] private Transform EnemyBS5;
        
        //Vehicle Transforms
        [SerializeField] private Transform HeroVehicleBS;
        [SerializeField] private Transform EnemyVehicleBS;
        [SerializeField] private GameObject Gauge;

        //BattleStation List to store the GameObject Positions
        private List<Transform> heroBSList = new List<Transform>();
        private List<Transform> enemyBSList = new List<Transform>();
        private int capacity = 0;
        public static BattleSystem2 GetInstance()
        {
            return instance;
        }

        
        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
           // SaveSystem.SavePlayerInventory();
            battleState = BattleState2.START;

            StartCoroutine(SetupBattle());

            defeatPopUp = GameObject.FindGameObjectWithTag("DefeatScreen");
            defeatPopUp.SetActive(false);
            victoryPopUp = GameObject.FindGameObjectWithTag("VictoryScreen");
            victoryPopUp.SetActive(false);
            lootPopUp = GameObject.FindGameObjectWithTag("LootScreen");
            lootPopUp.SetActive(false);

        }

        IEnumerator SetupBattle()
        {
            fillHeals.GetComponent<Image>().fillAmount = 1;


            playersCharActions = SpawnCharacter(true);
            enemysCharActions = SpawnCharacter(false);

            //dialogue control                   
            dialogueText.text = "An enemy approaches!!";

            //update stats being displayed
            //reference to the BattleHUD Script
            if (SaveSystem.Pdata.HasVehicle)
                playerHUD.SetHUDVehicleHP(playerVehicle);
            else
                playerHUD.SetHUDUnitHPHero(instantiatedHeroes[0].GetComponent<Hero>());



            if (enemyVehicle != null)
                enemyHUD.SetHUDVehicleHP(enemyVehicle);
            else
                enemyHUD.SetHUDUnitHPHero(enemyHero);


            //wait 2 seconds
            yield return new WaitForSeconds(1f);
            //start with the players turn
            SetActiveChar(playersCharActions[0]);

            StartCoroutine(PlayerTurn());
        }

        IEnumerator PlayerTurn()
        {
            yield return new WaitForSeconds(1f);
            battleState = BattleState2.PLAYERTURN;
            dialogueText.text = "Choose an action:";
        }

        private void SetActiveChar(CharAction newChar)
        {
            activeChar = newChar;
        }

        private void chooseNextActiveChar()
            
        {
            
            if (battleState != BattleState2.WON && battleState != BattleState2.LOST)
            {
                //if the last to act was the first hero, who was in a vehicle, and everyone attacked
                if (activeChar == playersCharActions[0] && SaveSystem.Pdata.HasVehicle && allCharsUsedTurns == playerVehicle.Passangers.Count)
                {
                    SetActiveChar(enemysCharActions[0]);
                    battleState = BattleState2.ENEMYTURN;
                    allCharsUsedTurns = 0;

                }//if the last to act was the first and only hero
                else if(activeChar == playersCharActions[0]){
                    SetActiveChar(enemysCharActions[0]);
                    battleState = BattleState2.ENEMYTURN;
                    allCharsUsedTurns = 0;
                }//if its the enemies turn and they all attacked
                else if(allCharsUsedTurns == enemyVehicle.Passangers.Count)
                {

                    SetActiveChar(playersCharActions[0]);
                    battleState = BattleState2.PLAYERTURN;
                    allCharsUsedTurns = 0;
                }
            }
        }

        private List<CharAction> SpawnCharacter(bool isPlayerTeam)
        {
            charactersActions = new List<CharAction>();
            //instantiatedHeroes is an array that gives us access to the prefabs that we instantiated inside the scene
            //List instantiatedHeroes size set to the party member count
            //Same Storage for Enemy Party
            

            //attempting to ca
            



            //When vehicle is added (SaveFile Player hasVehicle bool true) 
            //move onto the switch case that enables and moves/transforms battlestation positions to their
            // proper coordinates based on vehicle capacity



            //IF Players Side (LEFT)
            if (isPlayerTeam)
            {
                numberOfHeals = 3;
                numberOfBattles = UnityEngine.Random.Range(1, 4);
                Debug.Log(numberOfBattles);
                //ADD Hero Battle Stations to a List
                heroBSList.Add(heroBS1);
                heroBSList.Add(heroBS2);              
                heroBSList.Add(heroBS3);           
                heroBSList.Add(heroBS4);             
                heroBSList.Add(heroBS5);
                enemyBSList.Add(EnemyBS1);
                enemyBSList.Add(EnemyBS2);
                enemyBSList.Add(EnemyBS3);
                enemyBSList.Add(EnemyBS4);
                enemyBSList.Add(EnemyBS5);

                HeroPartyDB.LoadParty();


                //IF PLAYER HAS VEHICLE
                if (SaveSystem.Pdata.HasVehicle)
                {
                    //prefabResources holds the hero prefabs from the Resources Folder
                    //set its size to the size of all the possible heroes in the HeroDB (allows addition of new heroes "future proof")
                    Player.LoadPlayer();
                    playerVehicle = Player.CurrentVehicle;
                    
                    switch (playerVehicle.Name)
                    {
                        case "Scout":
                            heroVehiclePrefab = (GameObject)Resources.Load("Scout2Vehicle", typeof(GameObject));
                            instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                            instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                            heroBSList[0].transform.position = new Vector3(-4.164998f, 1.404377f, 0f);
                            heroBSList[1].transform.position = new Vector3(-3.156f, 1.36614f, 0f);
                            break;
                        case "Warthog":
                            heroVehiclePrefab = (GameObject)Resources.Load("Warthog3Vehicle", typeof(GameObject));
                            instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                            instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                            heroBSList[0].transform.position = new Vector3(-2.3f, 2.040558f, 0f);
                            heroBSList[1].transform.position = new Vector3(-3.5688f, 1.428f, 0f);
                            heroBSList[2].transform.position = new Vector3(-4.55f, 1.395107f, 0f);
                            break;
                        case "Goliath":
                            heroVehiclePrefab = (GameObject)Resources.Load("Goliath4Vehicle", typeof(GameObject));
                            instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                            instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                            heroBSList[0].transform.position = new Vector3(-2.17f, 1.963147f, 0f);
                            heroBSList[1].transform.position = new Vector3(-2.98f, 2.001312f, 0f);
                            heroBSList[2].transform.position = new Vector3(-4.1f, 2.291212f, 0f);
                            heroBSList[3].transform.position = new Vector3(-4.890064f, 2.324204f, 0f);
                            break;
                        case "Leviathan":
                            heroVehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));
                            instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                            instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                            heroBSList[0].transform.position = new Vector3(-1.409f, 1.258098f, 0f);
                            heroBSList[1].transform.position = new Vector3(-2.362f, 1.3f, 0f);
                            heroBSList[2].transform.position = new Vector3(-3.44f, 2.119831f, 0f);
                            heroBSList[3].transform.position = new Vector3(-4.177f, 2.119828f, 0f);
                            heroBSList[4].transform.position = new Vector3(-4.94f, 2.119828f, 0f);
                            break;
                        
                        default:
                        break;
                    }
                    
                    //instantiate hero vehicle

                    Debug.Log(playerVehicle.Name);
                    heroPrefabs = new GameObject[HeroPartyDB.getHeroList().Count];
                    instantiatedHeroes = new Transform[HeroPartyDB.getHeroList().Count];


                    //run through prefabResources
                    //load the prefabs from Resources folder into the prefabResources list based on the length of DB

                    //characterTransform is an array that gives us access to the prefabs that we instantiated inside the scene
                    //run through characterTransform list based on its length
                    for (int i = 0; i < heroPrefabs.Length && i < playerVehicle.PartySize; i++)
                    {
                        //using the name of the prefab based on name of hero inside DB
                        heroPrefabs[i] = (GameObject)Resources.Load(HeroPartyDB.getHero(i).GetType().Name, typeof(GameObject));
                        HeroPartyDB.getHero(i).setSkin(heroPrefabs[i]);
                        //takes prefab from prefabResources at its current iteration, that same prefabs position and rotation
                        //gives us access to those prefabs
                        instantiatedHeroes[i] = Instantiate(heroPrefabs[i].transform, heroBSList[i].position, Quaternion.identity);
                        instantiatedHeroes[i].GetComponent<Hero>().MaxHP += ArmorDatabase.getArmor(HeroPartyDB.getHero(i).SkinTire - 1).Health;
                        instantiatedHeroes[i].GetComponent<Hero>().CurrentHP += ArmorDatabase.getArmor(HeroPartyDB.getHero(i).SkinTire - 1).Health;
                        instantiatedHeroes[i].GetComponent<Hero>().Damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(i).SkinTire - 1).Damage;
                        playerVehicle.addPassangers(instantiatedHeroes[i]);
                        //Adding instantiated Characters to playerUnit list 
                        charactersActions.Add(instantiatedHeroes[i].GetComponent<CharAction>());
                        charactersActions[i].PlayIdleAnim();
                    }
                }
                //IF THERE IS NO VEHICLE
                else 
                {
                    heroBSList[0].transform.position = new Vector3(-5f, 0f, 0f);
                    heroPrefabs = new GameObject[1];

                    
                    heroPrefabs[0] = (GameObject)Resources.Load(HeroPartyDB.getHero(0).GetType().Name, typeof(GameObject));
                    HeroPartyDB.getHero(0).setSkin(heroPrefabs[0]);
                    instantiatedHeroes = new Transform[1];
                    
                    instantiatedHeroes[0] = Instantiate(heroPrefabs[0].transform, heroBSList[0].position, Quaternion.identity);
                    instantiatedHeroes[0].GetComponent<Hero>().MaxHP += ArmorDatabase.getArmor(HeroPartyDB.getHero(0).SkinTire - 1).Health + 300;
                    instantiatedHeroes[0].GetComponent<Hero>().CurrentHP += ArmorDatabase.getArmor(HeroPartyDB.getHero(0).SkinTire - 1).Health +300;
                    instantiatedHeroes[0].GetComponent<Hero>().Damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(0).SkinTire - 1).Damage + 200;

                    Debug.Log(instantiatedHeroes[0].GetComponent<Hero>().MaxHP  + "Max HERO HP when instantiated" );

                    charactersActions.Add(instantiatedHeroes[0].GetComponent<CharAction>());
                    
                    charactersActions[0].PlayIdleAnim();
                }
            }
            else 
            {
                if (enemyPrefabs.Length != 1)
                {
                    foreach (var enemy in instantiatedEnemies)
                    {
                      Destroy(enemy.gameObject);
                    }
                    Destroy(instantiatedEnemyVehicle.gameObject);
                }
                SpawnEnemy(); 
            }
            
            return charactersActions;
        }

        void SpawnEnemy(){
            int partySize = 0;
            if(Player.HasVehicle){
                partySize = Player.CurrentVehicle.PartySize;
            }
            
            if(!Player.HasVehicle)
                enemyVehicle = new Vehicle(VehicleClassDB.getVehicle(0));              
            else if(partySize == 2)
                enemyVehicle = new Vehicle(VehicleClassDB.getVehicle((UnityEngine.Random.Range(0, 2))));
            else if(partySize == 3)
                enemyVehicle = new Vehicle(VehicleClassDB.getVehicle((UnityEngine.Random.Range(0, 3))));
            else if(partySize >= 4)
                enemyVehicle = new Vehicle(VehicleClassDB.getVehicle((UnityEngine.Random.Range(0, 4))));
                
            switch (enemyVehicle.Name)
                    {
                        case "Scout":
                            enemyVehiclePrefab = (GameObject)Resources.Load("Scout2Vehicle", typeof(GameObject));
                            instantiatedEnemyVehicle = Instantiate(enemyVehiclePrefab.transform, EnemyVehicleBS.position, Quaternion.Euler(0, 180, 0));
                            instantiatedEnemyVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                            enemyBSList[0].transform.position = new Vector3(4.86f, 1.371412f, 0f);
                            enemyBSList[1].transform.position = new Vector3(3.8199f, 1.4043f, 0f);
                            
                            break;
                        case "Warthog":
                            enemyVehiclePrefab = (GameObject)Resources.Load("Warthog3Vehicle", typeof(GameObject));
                            instantiatedEnemyVehicle = Instantiate(enemyVehiclePrefab.transform, EnemyVehicleBS.position, Quaternion.Euler(0, 180, 0));
                            instantiatedEnemyVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                            enemyBSList[0].transform.position = new Vector3(3f, 2.04f, 0f);
                            enemyBSList[1].transform.position = new Vector3(4.2f, 1.389902f, 0f);
                            enemyBSList[2].transform.position = new Vector3(5.19f, 1.39f, 0f);
                            break;
                        case "Goliath":
                            enemyVehiclePrefab = (GameObject)Resources.Load("Goliath4Vehicle", typeof(GameObject));
                            instantiatedEnemyVehicle = Instantiate(enemyVehiclePrefab.transform, EnemyVehicleBS.position, Quaternion.Euler(0, 180, 0));
                            instantiatedEnemyVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                            enemyBSList[0].transform.position = new Vector3(2.85f, 1.9683f, 0f);
                            enemyBSList[1].transform.position = new Vector3(3.6753f, 1.9683f, 0f);
                            enemyBSList[2].transform.position = new Vector3(4.74f, 2.2939f, 0f);
                            enemyBSList[3].transform.position = new Vector3(5.64f, 2.2939f, 0f);
                            break;
                        case "Leviathan":
                            enemyVehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));
                            instantiatedEnemyVehicle = Instantiate(enemyVehiclePrefab.transform, EnemyVehicleBS.position, Quaternion.Euler(0, 180, 0));
                            instantiatedEnemyVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                            enemyBSList[0].transform.position = new Vector3(2.109f, 1.264f, 0f);
                            enemyBSList[1].transform.position = new Vector3(3.052f, 1.3f, 0f);
                            enemyBSList[2].transform.position = new Vector3(4.08f, 2.1528f, 0f);
                            enemyBSList[3].transform.position = new Vector3(5f, 2.1198f, 0f);
                            enemyBSList[4].transform.position = new Vector3(5.76f, 2.1146f, 0f);
                            break;
                        
                        default:
                        break;
                    }
            enemyPrefabs = new GameObject[enemyVehicle.PartySize];

            instantiatedEnemies = new Transform[enemyPrefabs.Length];
            //characterTransform is an array that gives us access to the prefabs that we instantiated inside the scene
            //run through characterTransform list based on its length
            for (int i = 0; i < instantiatedEnemies.Length; i++)
            {
                //using the name of the prefab based on name of hero inside DB
                enemyPrefabs[i] = (GameObject)Resources.Load(HeroClassDB.getHero(UnityEngine.Random.Range(3, 6)).GetType().Name, typeof(GameObject));
                enemyPrefabs[i].GetComponent<Hero>().SkinTire = UnityEngine.Random.Range(1,5);
                enemyPrefabs[i].GetComponent<Hero>().setSkin(enemyPrefabs[i]);
                //takes prefab from prefabResources at its current iteration, that same prefabs position and rotation
                //gives us access to those prefabs
                instantiatedEnemies[i] = Instantiate(enemyPrefabs[i].transform, enemyBSList[i].position, Quaternion.Euler(0, 180, 0));
                instantiatedEnemies[i].GetComponent<Hero>().MaxHP += ArmorDatabase.getArmor(enemyPrefabs[i].GetComponent<Hero>().SkinTire - 1).Health;
                instantiatedEnemies[i].GetComponent<Hero>().CurrentHP += ArmorDatabase.getArmor(enemyPrefabs[i].GetComponent<Hero>().SkinTire - 1).Health;
                instantiatedEnemies[i].GetComponent<Hero>().Damage += WeaponDatabase.getWeapon(enemyPrefabs[i].GetComponent<Hero>().SkinTire - 1).Damage;
                enemyVehicle.addPassangers(instantiatedEnemies[i]);
                //enemyUnits.Add(instantiatedEnemies[i].GetComponent<Hero>());
                charactersActions.Add(instantiatedEnemies[i].GetComponent<CharAction>());
                charactersActions[i].PlayIdleAnim();
            }
        }

        public void CalculateDamage()
        {

            if (battleState == BattleState2.PLAYERBUSY)
            {
                bool isEnemyDead;

                //IF PLAYER HAS VEHICLE DAMAGE ENEMY VEHICLE WITH SUM DAMAGE
                //test to see if the enemy is still alive
                    
                isEnemyDead = enemyVehicle.TakeDamage(instantiatedHeroes[allCharsUsedTurns-1].GetComponent<Hero>().getDamageWithCrit());
                
                
                //update enemy health
                enemyHUD.SetHP(enemyVehicle.CurrentHP);

                //if dead you won, if not enemy Turn
                if (isEnemyDead)
                {
                    battleState = BattleState2.WON;
                    EndBattle();
                }
                else if(allCharsUsedTurns == instantiatedHeroes.Length)
                {
                    battleState = BattleState2.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                    allCharsUsedTurns = 0;
                }

            }
            else if (battleState == BattleState2.ENEMYBUSY)
            {

                bool isPlayerDead;


                //if player HAS VEHICLES
                if(SaveSystem.Pdata.HasVehicle) {

                    //test to see if the enemy is still alive
                    isPlayerDead = playerVehicle.TakeDamage(instantiatedEnemies[allCharsUsedTurns-1].GetComponent<Hero>().getDamageWithCrit());
                
                
                    //update enemy health
                    //reference to the BattleHUD Script
                    playerHUD.SetHP(playerVehicle.CurrentHP);

                //IF PLAYER NO VEHICLE
                } else {
                            Debug.Log(allCharsUsedTurns);
                        isPlayerDead = instantiatedHeroes[0].GetComponent<Hero>().TakeDamage(instantiatedEnemies[allCharsUsedTurns - 1].GetComponent<Hero>().getDamageWithCrit());
                        playerHUD.SetHP(instantiatedHeroes[0].GetComponent<Hero>().CurrentHP);
                }
               
               
                //if dead you won, if not enemy Turn
                if (isPlayerDead)
                {
                    battleState = BattleState2.LOST;
                    EndBattle();
                }
                else if(allCharsUsedTurns == instantiatedEnemies.Length)
                {

                    StartCoroutine(PlayerTurn());
                    allCharsUsedTurns = 0;
                }
            } 
        }

        

        public void OnAttackButton()
        {
           
            if (battleState != BattleState2.PLAYERTURN)
            {
                return;
            }
            else
            {
                battleState = BattleState2.PLAYERBUSY;
                if (SaveSystem.Pdata.HasVehicle)
                {
                    foreach (Transform heroes in playerVehicle.Passangers)
                    {                                           
                       
 //TODO: NEEDS CHANGING WHEN WE HAVE VEHICLE PREFAB        ''enemysCharActions[0]''               
                        allCharsUsedTurns++;                    
                        heroes.GetComponent<CharAction>().Attack(() =>
                        {
                            chooseNextActiveChar();
                        });
                        
                    }
                }
                else
                {
                    allCharsUsedTurns++;
                    battleState = BattleState2.PLAYERBUSY;
                    playersCharActions[0].Attack(() =>
                    {
                        chooseNextActiveChar();
                    });
                }


            }
        }


         IEnumerator EnemyTurn()
        {

            while (battleState == BattleState2.ENEMYTURN)
            {
                yield return new WaitForSeconds(2f);
                dialogueText.text = "The enemy is attacking!";

                yield return new WaitForSeconds(1f);

                //HERE YOU CAN EXPAND THE BEHAVIOUR OF THE ENEMY BASED ON % OF HEALTH OR OTHER FACTORS
                battleState = BattleState2.ENEMYBUSY;
                foreach (Transform heroes in enemyVehicle.Passangers)
                {                                           
                    allCharsUsedTurns++;                  //TODO: NEEDS CHANGING
                    heroes.GetComponent<CharAction>().Attack(() =>
                    {
                        chooseNextActiveChar();
                    });
                      
                }
                /////////////

            }
        }


        //IEnumerator 
        public void CalculateHeal()
        {

            if (battleState == BattleState2.PLAYERBUSY)
            {

                

                if (SaveSystem.Pdata.HasVehicle)
                    {

                        //heals player for a 4th of his health
                        playerVehicle.Heal(playerVehicle.SumHP / 4); // was 25

                        //reference to the BattleHUD Script
                        playerHUD.SetHP(playerVehicle.CurrentHP);

                        //yield return new WaitForSeconds(2f);

                        dialogueText.text = "Your Vehicle is less damaged!";

                        Debug.Log(dialogueText.text);
                        
                        battleState = BattleState2.ENEMYTURN;
                        numberOfHeals--;
                    fillHeals.GetComponent<Image>().fillAmount = numberOfHeals / 3;
                    StartCoroutine(EnemyTurn());

                        //IF NO VEHICLE HEAL
                    }
                    else
                    {

                        //heals player for a 4th of his health
                        instantiatedHeroes[0].GetComponent<Hero>().Heal(instantiatedHeroes[0].GetComponent<Hero>().MaxHP / 4); // was 25

                        //reference to the BattleHUD Script
                        playerHUD.SetHP(instantiatedHeroes[0].GetComponent<Hero>().CurrentHP);

                        //yield return new WaitForSeconds(2f);

                        dialogueText.text = "You're less dead!";
                        
                        Debug.Log(dialogueText.text);
                        numberOfHeals--;
                    fillHeals.GetComponent<Image>().fillAmount = numberOfHeals / 3;
                    battleState = BattleState2.ENEMYTURN;
                        StartCoroutine(EnemyTurn());



                    }
                
                //IF HAS VEHICLE HEAL
                

            }
            // else if (battleState == BattleState2.ENEMYTURN)
            // {

            //     ////heals player for a 4th of his health
            //     //enemyUnit.Heal(enemyUnit.maxHP / 4); // was 25

            //     //enemyHUD.SetHP(enemyUnit.currentHP);
            //     //dialogueText.text = "You feel your strength renewed!";

            //     //// yield return new WaitForSeconds(2f);

            //     //??????
            //     StartCoroutine(PlayerTurn());
            // }
        }


        public void OnHealButton()
        {
            if (battleState != BattleState2.PLAYERTURN)
                return;
            else if(numberOfHeals <= 0){
                dialogueText.text = "You are out of heals!";
                return;
            }
            else
            {

                //if player HAS VEHICLE
                if (SaveSystem.Pdata.HasVehicle)
                {
                    battleState = BattleState2.PLAYERBUSY;
                    //add int to vehicle current health

                    foreach (Transform heroes in playerVehicle.Passangers)
                    {
                        allCharsUsedTurns++;                    //TODO: NEEDS CHANGING
                        heroes.GetComponent<CharAction>().Heal(() =>
                        {
                            chooseNextActiveChar();
                        });
                    }
                    allCharsUsedTurns = 0;
                    //NO VEHICLE            
                }
                else
                {
                    battleState = BattleState2.PLAYERBUSY;
                    //add int to party member current hp
                    allCharsUsedTurns++;
                    playersCharActions[0].Heal(() =>
                    {
                        chooseNextActiveChar();
                    });
                }
            }
        }


        public void runAway()
        {
            if (battleState != BattleState2.PLAYERTURN)
                return;
            else
            {
                int chance = playerVehicle.Speed / 2;
                int random = UnityEngine.Random.Range(1, 100);
                if (random <= chance)
                {
                    dialogueText.text = "You got away safely";
                    battlesWon++;
                    System.Threading.Thread.Sleep(1000);
                    allCharsUsedTurns = 0;
                    battleState = BattleState2.ENEMYTURN;
                    Debug.Log("Success");
                    clickNextBattle();
                    
                }
                else
                {
                    allCharsUsedTurns = 0;
                    battleState = BattleState2.ENEMYTURN;
                    Debug.Log("Fail");
                    StartCoroutine(EnemyTurn());

                    dialogueText.text = "There  is no escape!";


                }
            }
        }


        /// /// /// /// /// /// /// END /// /// /// /// /// /// /// ///

        void EndBattle()
        {

            if (battleState == BattleState2.WON)
            {
                loot = UnityEngine.Random.Range(100, 201);
                lootText.text = Convert.ToString(loot);
                victoryPopUp.SetActive(true);
                dialogueText.text = "You won the battle!";
                battlesWon++;
                
            }
            else if (battleState == BattleState2.LOST)
            {
                defeatPopUp.SetActive(true);
                dialogueText.text = "You were defeated.";
            }
        }

        public void clickNextBattle(){
            
            PlayerInventory.Money += loot;

            if(battlesWon == numberOfBattles)
            PlayerInventory.SavePlayerInventory();


            victoryPopUp.SetActive(false);
            lootPopUp.SetActive(false);

            if(battlesWon <= numberOfBattles){
                SpawnCharacter(false);
                enemyHUD.SetHUDVehicleHP(enemyVehicle);
                allCharsUsedTurns = 0;
                StartCoroutine(PlayerTurn());
            }
            else
            {
                Player.LoadPlayer();
                Player.CurrentLocation = Player.LocationToTravelTo;
                Player.LocationToTravelTo = null;
                Player.SavePlayer();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
            }            
        }


        public void generateLootScreen()
        {
            capacity = 0;
            foreach (var item in HeroPartyDB.getHeroList())
            {
                capacity += item.Capacity;
            }
            int carrying = 0;
            foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
            {
                carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
            }

            if (Player.HasVehicle)
                capacity += Player.CurrentVehicle.Capacity;

            Gauge.GetComponent<Image>().fillAmount = (float)carrying / (float)capacity;


            lootPopUp.SetActive(true);
            victoryPopUp.SetActive(false);

            int partySize = 1;

            do
            {
                    if (Player.HasVehicle)
                {
                    partySize = Player.CurrentVehicle.PartySize;
                }
                if (partySize == 1)
                {
                    randomID = randy.Next(1, 3);
                    randomID2 = randy.Next(1, 3);


                }
                if (partySize == 2)
                {
                    randomID = randy.Next(1, 5);
                    randomID2 = randy.Next(1, 5);


                }
                if (partySize == 3)
                {
                    randomID = randy.Next(1, 7);
                    randomID2 = randy.Next(1, 7);


                }
                if (partySize == 4 || partySize == 5)
                {
                    randomID = randy.Next(1, 11);
                    randomID2 = randy.Next(1, 11);


                }
            } while (randomID == randomID2);



                randomAmount = randy.Next(1, 5);
                randomAmount2 = randy.Next(1, 5);
            

            //loot items and amount
            item1 =  TradeableDatabase.getTradeable(randomID - 1);
            item2 =  TradeableDatabase.getTradeable(randomID2 - 1);


            lootOneName.text = item1.ItemName;
            lootTwoName.text = item2.ItemName;
            lootOneAmount.text = randomAmount.ToString();
            lootTwoAmount.text = randomAmount2.ToString();
            inventoryOneAmount.text = System.Convert.ToString(PlayerInventory.TradeableInventory[randomID - 1].Count);
            inventoryTwoAmount.text = System.Convert.ToString(PlayerInventory.TradeableInventory[randomID2 - 1].Count);


        }

        public void addLootOneToInventory()
        {
            int carrying = 0;
            foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
            {
                carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
            }

            if(carrying + item1.Weight <= capacity)
            {
                //put loot from loot screen in player inventory
                if (randomID != 0 && randomAmount != 0) 
                {
                    PlayerInventory.AddToInventory(randomID, 1);
                    randomAmount--;
                    lootOneAmount.text = randomAmount.ToString();
                    inventoryOneAmount.text = System.Convert.ToString(PlayerInventory.TradeableInventory[randomID - 1].Count);
                    carrying = 0;
                    foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
                    {
                        carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
                    }
                    Gauge.GetComponent<Image>().fillAmount = (float)carrying / (float)capacity;
                }
            }
            else{
                //set text to error
            }
            
            
        }

        public void addLootTwoToInventory()
        {
            int carrying = 0;
            foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
            {
                carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
            }

            if(carrying + item1.Weight <= capacity)
            {
                //put loot from loot screen in player inventory
                if (randomID2 != 0 && randomAmount2 != 0)
                {
                    PlayerInventory.AddToInventory(randomID2, 1);
                    randomAmount2--;
                    lootTwoAmount.text = randomAmount2.ToString();
                    inventoryTwoAmount.text = System.Convert.ToString(PlayerInventory.TradeableInventory[randomID2 - 1].Count);
                    carrying = 0;
                    foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
                    {
                        carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
                    }
                    Gauge.GetComponent<Image>().fillAmount = (float)carrying / (float)capacity;
                }
            }
            else{
                //set text to error
            }
        }


        public void removeLootOneFromInventory()
        {
            if (randomID != 0 && PlayerInventory.TradeableInventory[randomID - 1].Count != 0)
            {
                randomAmount++;
                lootOneAmount.text = randomAmount.ToString();
                PlayerInventory.RemoveFromInventory(randomID, 1);
                inventoryOneAmount.text = System.Convert.ToString(PlayerInventory.TradeableInventory[randomID - 1].Count);

                int carrying = 0;
                foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
                {
                    carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
                }
                Gauge.GetComponent<Image>().fillAmount = (float)carrying / (float)capacity;
            }

        }
        public void removeLootTwoFromInventory()
        {
            if (randomID2 != 0 && PlayerInventory.TradeableInventory[randomID2 - 1].Count != 0)
            {
                randomAmount2++;              
                lootTwoAmount.text = randomAmount2.ToString();
                PlayerInventory.RemoveFromInventory(randomID2, 1);
                inventoryTwoAmount.text = System.Convert.ToString(PlayerInventory.TradeableInventory[randomID2 - 1].Count);

                int carrying = 0;
                foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
                {
                    carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
                }
                Gauge.GetComponent<Image>().fillAmount = (float)carrying / (float)capacity;
            }
        }

        public void takeLoot()
        {
            clickNextBattle();
        }

        public void clickAcceptDefeat(){
            Player.LocationToTravelTo = null;
            Player.SavePlayer();

            foreach (var item in PlayerInventory.TradeableInventory)
            {
                item.Count = 0;
            }

            PlayerInventory.SavePlayerInventory();

          //  PlayerInventory.LoadPlayerInventory();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
        }
    }
}