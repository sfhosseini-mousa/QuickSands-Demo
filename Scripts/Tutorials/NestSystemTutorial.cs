using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Spine.Unity;

namespace Sands
{
    public class NestSystemTutorial : MonoBehaviour
    {
        [SerializeField] private AudioSource defeatMusic;       //defeat audio track
        [SerializeField] private AudioSource victoryMusic;      //victory audio track

        private List<CharAction> charactersActions;
        private List<CharAction> playersCharActions;
        private Vehicle playerVehicle = new Vehicle();
        private GameObject[] heroPrefabs;
        private GameObject heroVehiclePrefab;
        private Transform[] instantiatedHeroes;
        private Transform instantiatedHeroVehicle;
        private BattleChecker[] heroBattleCheckers;
        private Hero[] heroes;

        [SerializeField] private LevelLoader levelLoader;

        //Object Transform Hero positions on Vehicles
        [SerializeField] private Transform[] heroBSList = new Transform[5];
        [SerializeField] private Transform HeroVehicleBS;
        private Coroutine moveHeroVehicleInC;
        [SerializeField] private GameObject defeatPopUp;
        [SerializeField] private GameObject victoryPopUp;
        [SerializeField] private GameObject lootPopUp;
        public BattleState2 battleState;

        [SerializeField] private Text dialogueText;
        private GameObject[] enemyPrefabs = new GameObject[1];

        private Transform[] instantiatedEnemies;
        private GameObject[] heroHealthBarPrefabs = new GameObject[5];
        private GameObject[] enemyHealthBarPrefabs = new GameObject[3];
        private Transform[] instantiatedHeroHealthBars = new Transform[5];
        private Transform[] instantiatedEnemyHealthBars = new Transform[3];
        [SerializeField] private GameObject[] heroButtons = new GameObject[5];
        [SerializeField] private GameObject[] enemyButtons = new GameObject[3];

        int specialCounter = 3;
        bool isAbility;
        bool allHeroesDeadFromAbility;
        
        private CharAction activeChar;

        //Object Transform Enemy positions on Vehicles
        [SerializeField] private Transform EnemyBS1;
        [SerializeField] private Transform EnemyBS2;
        [SerializeField] private Transform EnemyBS3;

        //Vehicle Transforms
        [SerializeField] private Transform EnemyHolderBS;

        //BattleStation List to store the GameObject Positions
        private List<Transform> enemyBSList = new List<Transform>();
        private int selectedHero = 0;
        private int selectedHeroToHeal = -1;
        private int selectedEnemy = 0;
        private int enemyToAttack = 0;
      
        private BattleChecker[] enemyBattleCheckers = new BattleChecker[3];
        private Coroutine MoveEnemiesOutC = null;
        private Coroutine MoveEnemiesInC = null;

        private GameObject heroIndicator = null;
        private GameObject enemyIndicator = null;
        private Transform instantiatedHeroIndicator = null;
        private Transform instantiatedEnemyIndicator = null;

        //UI Abilities
        [SerializeField] private GameObject healButton;
        [SerializeField] private GameObject twoForOneButton;
        [SerializeField] private GameObject spearmanAbilityButton;
        [SerializeField] private GameObject wizardAbilityButton;
        [SerializeField] private GameObject blockButton;
        [SerializeField] private GameObject[] battleElements = new GameObject[3];
        private bool shouldHeal = false;
        private bool shouldDefend = false;
        private bool shouldAttackTwo = false;
        private bool shouldAttackThree = false;

        [SerializeField] private GameObject[] backgrounds;

        [SerializeField] private GameObject introText;
        [SerializeField] private GameObject introYear;
        [SerializeField] private GameObject introTextContinue;
        [SerializeField] private GameObject introTextDoomed;


        [SerializeField] private GameObject[] introPopUps;
        [SerializeField] private Transform[] HandPositions;
        private Button[] buttons;
        bool chooseHeroTutorialDone;
        bool chooseEnemyTutorialDone;
        bool pressAttackTutorialDone;
        bool firstEnemyAttackDone;
        bool chooseMageDone;
        bool pressHealDone;
        bool chooseHeroTohealDone;

        private int wave = 1;
        private int bossFight = 0;

        ObjectFader objectFader = new ObjectFader();
        ButtonDeactivator buttonDeactivator = new ButtonDeactivator();
        [SerializeField] HandPointerCreator handPointerCreator;

        void Start()
        {
            buttons = GameObject.FindObjectsOfType<Button>();

            foreach (var item in battleElements)
            {
                item.SetActive(false);
            }

            twoForOneButton.SetActive(false);
            blockButton.SetActive(false);
            healButton.SetActive(false);
            wizardAbilityButton.SetActive(false);
            spearmanAbilityButton.SetActive(false);

            StartCoroutine(StartIntro());
        }

        private IEnumerator StartIntro()
        {
            StartCoroutine(objectFader.AppearText(introText));
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(objectFader.AppearText(introYear));

            yield return new WaitForSeconds(1.0f);

            playersCharActions = InstantiateParty();
            MoveHeroAimation();
            moveHeroVehicleInC = StartCoroutine(MoveHeroVehicleIn());
            battleState = BattleState2.START;

            StartCoroutine(objectFader.DisappearText(introText));
            yield return new WaitForSeconds(.5f);
            StartCoroutine(objectFader.DisappearText(introYear));

            

            foreach (var item in heroButtons)
            {
                item.SetActive(false);
            }

            foreach (var item in enemyButtons)
            {
                item.SetActive(false);
            }

            Invoke("StartSetup", 2.5f);

            //GameObject.FindGameObjectWithTag("Music").GetComponent<ContiniousMusic>().ResetMusic();
        }

        private void StartSetup()
        {
            StartCoroutine(SetupBattle());
        }

        IEnumerator SetupBattle()
        {
            //playersCharActions = SpawnCharacter(true);
            SpawnEnemy();
            //dialogue control                   
            dialogueText.text = "An enemy approaches!!";

            MoveEnemiesInC = StartCoroutine(MoveEnemiesIn());

            yield return new WaitForSeconds(2f);

            foreach (var item in battleElements)
            {
                item.SetActive(true);
            }

            

            InstantiateHeroHealthBars();
            InstantiateEnemyHealthBars();
            SpawnHeroButtons();
            SpawnEnemyButtons();


            //wait 2 seconds
            yield return new WaitForSeconds(1f);
            //start with the players turn
            SetActiveChar(playersCharActions[0]);

            StartCoroutine(PlayerTurn());
        }

        void ChooseHeroTutorial()
        {
            buttonDeactivator.DeactivateAllButtonsExcept("6", buttons);

            introPopUps[0].SetActive(true);

            Coroutine flashHero = StartCoroutine(objectFader.FlashSpineSkeletonMecenim(instantiatedHeroes[0].gameObject));

            handPointerCreator.InstantiateHandPointer(HandPositions[0]);

            heroButtons[0].GetComponent<Button>().onClick.AddListener(() => {
                if (!chooseHeroTutorialDone)
                {
                    chooseHeroTutorialDone = true;
                    StopCoroutine(flashHero);
                    instantiatedHeroes[0].gameObject.GetComponent<SkeletonMecanim>().skeleton.A = 1f;
                    introPopUps[0].SetActive(false);
                    handPointerCreator.DestroyHandPointer();
                    ChooseEnemyTutorial();
                }
            });
        }

        void ChooseEnemyTutorial()
        {
            buttonDeactivator.DeactivateAllButtonsExcept("2", buttons);

            introPopUps[1].SetActive(true);

            Coroutine flashEnemy = StartCoroutine(objectFader.FlashSpineSkeletonAnimation(instantiatedEnemies[1].gameObject));

            handPointerCreator.InstantiateHandPointer(HandPositions[1]);

            enemyButtons[1].GetComponent<Button>().onClick.AddListener(() => {
                if (!chooseEnemyTutorialDone)
                {
                    chooseEnemyTutorialDone = true;
                    StopCoroutine(flashEnemy);
                    instantiatedEnemies[1].gameObject.GetComponent<SkeletonAnimation>().skeleton.A = 1f;
                    introPopUps[1].SetActive(false);
                    handPointerCreator.DestroyHandPointer();
                    PressAttackTutorial();
                }
            });
        }

        void PressAttackTutorial()
        {
            buttonDeactivator.DeactivateAllButtonsExcept("Attack", buttons);

            introPopUps[2].SetActive(true);

            Coroutine flashAttackBtn = StartCoroutine(objectFader.FlashImage(battleElements[1]));

            handPointerCreator.InstantiateHandPointer(HandPositions[2]);

            battleElements[1].GetComponent<Button>().onClick.AddListener(() => {
                if (!pressAttackTutorialDone)
                {
                    pressAttackTutorialDone = true;
                    StopCoroutine(flashAttackBtn);
                    Color color = battleElements[1].GetComponent<Image>().color;
                    color.a = 1f;
                    battleElements[1].GetComponent<Image>().color = color;
                    introPopUps[2].SetActive(false);
                    handPointerCreator.DestroyHandPointer();
                }
            });
        }

        IEnumerator FirstEnemyAttacks()
        {
            yield return new WaitForSeconds(1.8f);

            int attackedHero = 0;

            bool isCrit = false;
            int damage = instantiatedEnemies[enemyToAttack].GetComponent<Enemy>().Damage;
            bool isPlayerDead = instantiatedHeroes[attackedHero].GetComponent<Hero>().TakeDamage(damage);
            instantiatedHeroHealthBars[attackedHero].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedHeroes[attackedHero].GetComponent<Hero>().CurrentHP);
            instantiatedHeroes[attackedHero].GetComponent<CharAnimation>().HitAnim();
            DamagePopUp.Create(instantiatedHeroes[attackedHero].position, damage, isCrit);

            instantiatedEnemies[enemyToAttack].GetComponent<SkeletonAnimation>().AnimationName = "attack";
            yield return new WaitForSeconds(2f);
            instantiatedEnemies[enemyToAttack].GetComponent<SkeletonAnimation>().AnimationName = "idle";

            StartCoroutine(PlayerTurn());
        }

        void ChooseMageTutorial()
        {
            buttonDeactivator.DeactivateAllButtonsExcept("7", buttons);

            introPopUps[3].SetActive(true);

            Coroutine flashHero = StartCoroutine(objectFader.FlashSpineSkeletonMecenim(instantiatedHeroes[1].gameObject));

            handPointerCreator.InstantiateHandPointer(HandPositions[3]);

            heroButtons[1].GetComponent<Button>().onClick.AddListener(() => {
                if (!chooseMageDone)
                {
                    chooseMageDone = true;
                    StopCoroutine(flashHero);
                    instantiatedHeroes[1].gameObject.GetComponent<SkeletonMecanim>().skeleton.A = 1f;
                    introPopUps[3].SetActive(false);
                    handPointerCreator.DestroyHandPointer();
                    PressHealTutorial();
                }
            });
        }

        void PressHealTutorial()
        {
            buttonDeactivator.DeactivateAllButtonsExcept("Heal", buttons);

            introPopUps[4].SetActive(true);

            Coroutine flashHealBtn = StartCoroutine(objectFader.FlashImage(healButton));

            handPointerCreator.InstantiateHandPointer(HandPositions[4]);

            healButton.GetComponent<Button>().onClick.AddListener(() => {
                if (!pressHealDone)
                {
                    pressHealDone = true;
                    StopCoroutine(flashHealBtn);
                    Color color = healButton.GetComponent<Image>().color;
                    color.a = 1f;
                    healButton.GetComponent<Image>().color = color;
                    introPopUps[4].SetActive(false);
                    handPointerCreator.DestroyHandPointer();
                    ChooseHeroToHealTutorial();
                }
            });
        }

        void ChooseHeroToHealTutorial()
        {
            buttonDeactivator.DeactivateAllButtonsExcept("6", buttons);

            introPopUps[0].SetActive(true);

            Coroutine flashHero = StartCoroutine(objectFader.FlashSpineSkeletonMecenim(instantiatedHeroes[0].gameObject));

            handPointerCreator.InstantiateHandPointer(HandPositions[0]);

            heroButtons[0].GetComponent<Button>().onClick.AddListener(() => {
                if (!chooseHeroTohealDone)
                {
                    chooseHeroTohealDone = true;
                    StopCoroutine(flashHero);
                    instantiatedHeroes[0].gameObject.GetComponent<SkeletonMecanim>().skeleton.A = 1f;
                    introPopUps[0].SetActive(false);
                    handPointerCreator.DestroyHandPointer();

                    buttonDeactivator.ActivateAllButtons(buttons);

                    StartCoroutine(EndTutorialWithText());
                }
            });
        }

        IEnumerator EndTutorialWithText()
        {
            yield return new WaitForSeconds(3.0f);
            StartCoroutine(objectFader.AppearText(introTextContinue));
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(objectFader.AppearText(introTextDoomed));

            yield return new WaitForSeconds(1.0f);

            StartCoroutine(objectFader.DisappearText(introTextContinue));
            yield return new WaitForSeconds(.5f);
            StartCoroutine(objectFader.DisappearText(introTextDoomed));
        }

        private void SetActiveChar(CharAction newChar)
        {
            activeChar = newChar;
        }

        private List<CharAction> InstantiateParty()
        {
            heroes = new Hero[5] { new Warrior((Warrior)HeroClassDB.getHero(0)), new Mage((Mage)HeroClassDB.getHero(1)), new Ranger((Ranger)HeroClassDB.getHero(2)), new Wizard((Wizard)HeroClassDB.getHero(3)), new Spearman((Spearman)HeroClassDB.getHero(4)) };

            foreach (var hero in heroes)
            {
                hero.SkinTire = 5;
            }

            charactersActions = new List<CharAction>();

            heroBattleCheckers = new BattleChecker[5];

            //IF PLAYER HAS VEHICLE
            //prefabResources holds the hero prefabs from the Resources Folder
            //set its size to the size of all the possible heroes in the HeroDB (allows addition of new heroes "future proof")
            playerVehicle = VehicleClassDB.getVehicle(3);

                
            heroVehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));
            instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
            instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
            heroBSList[0].transform.position = new Vector3(-1.409f, 1.258098f, 0f);
            heroBSList[1].transform.position = new Vector3(-2.362f, 1.3f, 0f);
            heroBSList[2].transform.position = new Vector3(-3.44f, 2.119831f, 0f);
            heroBSList[3].transform.position = new Vector3(-4.177f, 2.119828f, 0f);
            heroBSList[4].transform.position = new Vector3(-4.94f, 2.119828f, 0f);

                //instantiate hero vehicle

            Debug.Log(playerVehicle.Name);
            heroPrefabs = new GameObject[heroes.Length];
            instantiatedHeroes = new Transform[heroes.Length];


            //run through prefabResources
            //load the prefabs from Resources folder into the prefabResources list based on the length of DB

            //characterTransform is an array that gives us access to the prefabs that we instantiated inside the scene
            //run through characterTransform list based on its length
            for (int i = 0; i < heroPrefabs.Length && i < playerVehicle.PartySize; i++)
            {
                //using the name of the prefab based on name of hero inside DB
                heroPrefabs[i] = (GameObject)Resources.Load(heroes[i].GetType().Name, typeof(GameObject));
                heroes[i].setSkin(heroPrefabs[i]);
                //takes prefab from prefabResources at its current iteration, that same prefabs position and rotation
                //gives us access to those prefabs
                instantiatedHeroes[i] = Instantiate(heroPrefabs[i].transform, heroBSList[i].position, Quaternion.identity);
                instantiatedHeroes[i].GetComponent<Hero>().MaxHP += ArmorDatabase.getArmor(heroes[i].SkinTire - 1).Health;
                instantiatedHeroes[i].GetComponent<Hero>().CurrentHP += ArmorDatabase.getArmor(heroes[i].SkinTire - 1).Health;
                instantiatedHeroes[i].GetComponent<Hero>().Damage += WeaponDatabase.getWeapon(heroes[i].SkinTire - 1).Damage;
                playerVehicle.addPassangers(instantiatedHeroes[i]);
                //Adding instantiated Characters to playerUnit list 
                charactersActions.Add(instantiatedHeroes[i].GetComponent<CharAction>());
                charactersActions[i].PlayIdleAnim();
                heroBattleCheckers[i] = instantiatedHeroes[i].GetComponent<BattleChecker>();
                instantiatedHeroes[i].SetParent(instantiatedHeroVehicle);
                instantiatedHeroes[i].GetComponent<HeroBuff>().AddBuffs(true);
            }

            instantiatedHeroVehicle.position -= new Vector3(10, 0, 0);
 
            return charactersActions;
        }

        //SPAWN ENEMY
        private void SpawnEnemy()
        {
            enemyBSList.Add(EnemyBS1);
            enemyBSList.Add(EnemyBS2);
            enemyBSList.Add(EnemyBS3);

            switch (wave)
            {
                case 1:
                    enemyBSList[0].transform.position = new Vector3(8.3f, 0.084f, 0f);
                    enemyBSList[1].transform.position = new Vector3(10.28f, 0.084f, 0f);

                    enemyBSList[0].transform.SetParent(EnemyHolderBS);
                    enemyBSList[1].transform.SetParent(EnemyHolderBS);

                    enemyPrefabs = new GameObject[2];
                    break;
                case 2:
                    enemyBSList[0].transform.position = new Vector3(7f, 0.084f, 0f);
                    enemyBSList[1].transform.position = new Vector3(9f, 0.084f, 0f);
                    enemyBSList[2].transform.position = new Vector3(11.3f, 0.084f, 0f);

                    enemyBSList[0].transform.SetParent(EnemyHolderBS);
                    enemyBSList[1].transform.SetParent(EnemyHolderBS);
                    enemyBSList[2].transform.SetParent(EnemyHolderBS);

                    enemyPrefabs = new GameObject[3];
                    break;
                case 3:
                    enemyBSList[0].transform.position = new Vector3(8.745f, 0.084f, 0f);

                    enemyBSList[0].transform.SetParent(EnemyHolderBS);

                    enemyPrefabs = new GameObject[1];
                    break;
                default:
                    break;
            }

            
            instantiatedEnemies = new Transform[enemyPrefabs.Length];
            enemyBattleCheckers = new BattleChecker[enemyPrefabs.Length];

            switch (wave)
            {
                case 1:
                    enemyPrefabs[0] = (GameObject)Resources.Load("DeathDrone", typeof(GameObject));
                    enemyPrefabs[1] = (GameObject)Resources.Load("DeathDrone", typeof(GameObject));
                    for (int i = 0; i < instantiatedEnemies.Length; i++)
                    {
                        instantiatedEnemies[i] = Instantiate(enemyPrefabs[i].transform, enemyBSList[i].position, Quaternion.Euler(0, 180, 0));
                        enemyBattleCheckers[i] = instantiatedEnemies[i].GetComponent<BattleChecker>();
                        instantiatedEnemies[i].SetParent(EnemyHolderBS);
                    }

                    break;
                case 2:
                    enemyPrefabs[0] = (GameObject)Resources.Load("DeathDrone", typeof(GameObject));
                    enemyPrefabs[1] = (GameObject)Resources.Load("DeathDrone", typeof(GameObject));
                    enemyPrefabs[2] = (GameObject)Resources.Load("Leviathan", typeof(GameObject));


                    for (int i = 0; i < instantiatedEnemies.Length; i++)
                    {
                        instantiatedEnemies[i] = Instantiate(enemyPrefabs[i].transform, enemyBSList[i].position, Quaternion.Euler(0, 180, 0));
                        enemyBattleCheckers[i] = instantiatedEnemies[i].GetComponent<BattleChecker>();
                        instantiatedEnemies[i].SetParent(EnemyHolderBS);
                    }

                    break;
                case 3:
                    enemyPrefabs[0] = (GameObject)Resources.Load("Dragon", typeof(GameObject));
                    bossFight = 2;
                    instantiatedEnemies[0] = Instantiate(enemyPrefabs[0].transform, enemyBSList[0].position, Quaternion.Euler(0, 180, 0));
                    enemyBattleCheckers[0] = instantiatedEnemies[0].GetComponent<BattleChecker>();
                    instantiatedEnemies[0].SetParent(EnemyHolderBS);

                    break;
                default:
                    break;
            }
        }

        public void HeroOnClick()
        {

            if (battleState != BattleState2.PLAYERTURN)
            {
                if (shouldHeal)
                {
                    shouldHeal = false;
                    selectedHeroToHeal = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 6;
                    heroBattleCheckers[selectedHero].UsedTurn = true;
                    for (int i = 0; i < heroBattleCheckers.Length; i++)
                    {
                        if (heroBattleCheckers[i].IsDead || heroBattleCheckers[i].UsedTurn)
                        {
                            heroButtons[i].SetActive(false);
                        }
                    }

                    StartCoroutine(HealSelectedHero());
                }
                else
                {
                    return;
                }
            }
            else
            {
                selectedHero = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 6;
                instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetLoss(instantiatedEnemies[selectedEnemy].GetComponent<Enemy>().CurrentHP - instantiatedHeroes[selectedHero].GetComponent<Hero>().getDamag());
                InstantiateHeroIndicator();
                DisplayAbilityButton();
            }
        }

        public void EnemyOnClick()
        {
            if (shouldHeal)
            {
                shouldHeal = false;

                for (int i = 0; i < heroBattleCheckers.Length; i++)
                {
                    if (heroBattleCheckers[i].IsDead || heroBattleCheckers[i].UsedTurn)
                    {
                        heroButtons[i].SetActive(false);
                    }
                }

                InstantiateEnemyIndicator();
                InstantiateHeroIndicator();
                battleState = BattleState2.PLAYERTURN;
                dialogueText.text = "Choose an action!";

                instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetLoss(instantiatedEnemies[selectedEnemy].GetComponent<Enemy>().CurrentHP);
                selectedEnemy = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 1;
                instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetLoss(instantiatedEnemies[selectedEnemy].GetComponent<Enemy>().CurrentHP - instantiatedHeroes[selectedHero].GetComponent<Hero>().getDamag());
                InstantiateEnemyIndicator();
            }
            else if (battleState != BattleState2.PLAYERTURN)
            {
                return;
            }
            else
            {
                instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetLoss(instantiatedEnemies[selectedEnemy].GetComponent<Enemy>().CurrentHP);
                selectedEnemy = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 1;
                instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetLoss(instantiatedEnemies[selectedEnemy].GetComponent<Enemy>().CurrentHP - instantiatedHeroes[selectedHero].GetComponent<Hero>().getDamag());
                InstantiateEnemyIndicator();
            }
        }

        IEnumerator PlayerTurn()
        {
            bool allHeroesUsedTurns = true;

            foreach (var hero in heroBattleCheckers)
            {
                if (!hero.IsDead)
                {
                    if (!hero.UsedTurn)
                        allHeroesUsedTurns = false;
                }
            }

            if (allHeroesUsedTurns)
            {
                for (int i = 0; i < heroBattleCheckers.Length; i++)
                {
                    if (!heroBattleCheckers[i].IsDead)
                    {
                        heroButtons[i].SetActive(true);
                    }
                    heroBattleCheckers[i].UsedTurn = false;
                }

            }

            do
            {
                selectedHero = UnityEngine.Random.Range(0, instantiatedHeroes.Length);
            } while (heroBattleCheckers[selectedHero].IsDead || heroBattleCheckers[selectedHero].UsedTurn);

            instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetLoss(instantiatedEnemies[selectedEnemy].GetComponent<Enemy>().CurrentHP - instantiatedHeroes[selectedHero].GetComponent<Hero>().getDamag());
            InstantiateHeroIndicator();
            InstantiateEnemyIndicator();
            DisplayAbilityButton();

            yield return new WaitForSeconds(0.75f);
            battleState = BattleState2.PLAYERTURN;
            DotDamageToEnemies();
            dialogueText.text = "Choose an action:";

            if(!chooseHeroTutorialDone)
                ChooseHeroTutorial();
            else if (!chooseMageDone)
                ChooseMageTutorial();
        }

        IEnumerator EnemyTurn()
        {
            while (battleState == BattleState2.ENEMYTURN)
            {
                yield return new WaitForSeconds(1f);
                dialogueText.text = "The enemy is attacking!";

                yield return new WaitForSeconds(1f);

                //HERE YOU CAN EXPAND THE BEHAVIOUR OF THE ENEMY BASED ON % OF HEALTH OR OTHER FACTORS
                battleState = BattleState2.ENEMYBUSY;

                bool allEnemiesUsedTurn = true;

                foreach (var enemy in enemyBattleCheckers)
                {
                    if (!enemy.UsedTurn && !enemy.IsDead)
                        allEnemiesUsedTurn = false;
                }

                if (allEnemiesUsedTurn)
                {
                    foreach (var enemy in enemyBattleCheckers)
                    {
                        enemy.UsedTurn = false;
                    }
                }

                do
                {
                    enemyToAttack = UnityEngine.Random.Range(0, instantiatedEnemies.Length);
                } while (enemyBattleCheckers[enemyToAttack].IsDead || enemyBattleCheckers[enemyToAttack].UsedTurn);

                instantiatedEnemies[enemyToAttack].GetComponent<BattleChecker>().UsedTurn = true;

                StartCoroutine(BossAbility(bossFight));

                if (!isAbility)
                {
                    instantiatedEnemies[enemyToAttack].GetComponent<AudioPlayer>().PlayAttack();

                    if (shouldDefend)
                    {
                        shouldDefend = false;
                        instantiatedHeroes[selectedHero].GetComponent<CharAnimation>().HealAnim();
                        instantiatedHeroes[selectedHero].GetComponent<AudioPlayer>().PlayAbility();

                        instantiatedEnemies[enemyToAttack].GetComponent<SkeletonAnimation>().AnimationName = "attack";
                        yield return new WaitForSeconds(1.2f);
                        instantiatedEnemies[enemyToAttack].GetComponent<SkeletonAnimation>().AnimationName = "idle";
                        StartCoroutine(PlayerTurn());
                    }

                    else
                    {
                        instantiatedEnemies[enemyToAttack].GetComponent<SkeletonAnimation>().AnimationName = "attack";
                        StartCoroutine(CalculateDamage());
                    }
                }
                else {
                    isAbility = false;

                    yield return new WaitForSeconds(5f);
                    if(!allHeroesDeadFromAbility)
                        StartCoroutine(PlayerTurn());
                }
            }
        }

        public IEnumerator CalculateDamage()
        {

            if (battleState == BattleState2.PLAYERBUSY)
            {
                bool isEnemyDead;
                bool isEnemyTwoDead = false;
                bool isEnemyThreeDead = false;
                int secondAttackedEnemy = -1;
                int thirdAttackedEnemy = -1;
                bool isCrit = false;


                if (shouldAttackThree)
                {
                    shouldAttackThree = false;
                    do
                    {
                        secondAttackedEnemy = UnityEngine.Random.Range(0, instantiatedEnemies.Length);
                    } while (enemyBattleCheckers[secondAttackedEnemy].IsDead || secondAttackedEnemy == selectedEnemy);

                    do
                    {
                        thirdAttackedEnemy = UnityEngine.Random.Range(0, instantiatedEnemies.Length);
                    } while (enemyBattleCheckers[thirdAttackedEnemy].IsDead || thirdAttackedEnemy == selectedEnemy || thirdAttackedEnemy == secondAttackedEnemy);

                    isEnemyDead = instantiatedEnemies[selectedEnemy].GetComponent<Hero>().TakeDamage(EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 3);
                    isEnemyTwoDead = instantiatedEnemies[secondAttackedEnemy].GetComponent<Hero>().TakeDamage(EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 3);
                    isEnemyThreeDead = instantiatedEnemies[thirdAttackedEnemy].GetComponent<Hero>().TakeDamage(EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 3);
                    DamagePopUp.Create(instantiatedEnemies[selectedEnemy].position, EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 3, false);
                    DamagePopUp.Create(instantiatedEnemies[secondAttackedEnemy].position, EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 3, false);
                    DamagePopUp.Create(instantiatedEnemies[thirdAttackedEnemy].position, EncounterSystem.InstantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 3, false);

                    instantiatedEnemyHealthBars[secondAttackedEnemy].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedEnemies[secondAttackedEnemy].GetComponent<Hero>().CurrentHP);
                    instantiatedEnemyHealthBars[thirdAttackedEnemy].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedEnemies[thirdAttackedEnemy].GetComponent<Hero>().CurrentHP);

                    instantiatedEnemies[secondAttackedEnemy].GetComponent<CharAnimation>().HitAnim();
                    instantiatedEnemies[thirdAttackedEnemy].GetComponent<CharAnimation>().HitAnim();
                }
                else if (shouldAttackTwo)
                {
                    shouldAttackTwo = false;
                    do
                    {
                        secondAttackedEnemy = UnityEngine.Random.Range(0, instantiatedEnemies.Length);
                    } while (enemyBattleCheckers[secondAttackedEnemy].IsDead || secondAttackedEnemy == selectedEnemy);

                    isEnemyDead = instantiatedEnemies[selectedEnemy].GetComponent<Enemy>().TakeDamage(instantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 2);
                    isEnemyTwoDead = instantiatedEnemies[secondAttackedEnemy].GetComponent<Enemy>().TakeDamage(instantiatedHeroes[selectedHero].GetComponent<Hero>().Damage / 2);
                    instantiatedEnemyHealthBars[secondAttackedEnemy].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedEnemies[secondAttackedEnemy].GetComponent<Enemy>().CurrentHP);
                }
                else
                {
                    isCrit = false;
                    int damage = instantiatedHeroes[selectedHero].GetComponent<Hero>().getDamageWithCrit(ref isCrit);
                    isEnemyDead = instantiatedEnemies[selectedEnemy].GetComponent<Enemy>().TakeDamage(damage);
                    DamagePopUp.Create(instantiatedEnemies[selectedEnemy].position, damage, isCrit);
                }

                //update enemy health
                instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedEnemies[selectedEnemy].GetComponent<Enemy>().CurrentHP);

                heroBattleCheckers[selectedHero].UsedTurn = true;
                heroButtons[selectedHero].SetActive(false);

                if (isEnemyTwoDead)
                {
                    enemyBattleCheckers[secondAttackedEnemy].IsDead = true;
                    enemyButtons[secondAttackedEnemy].SetActive(false);
                    Destroy(instantiatedEnemies[secondAttackedEnemy].gameObject);
                    Destroy(instantiatedEnemyHealthBars[secondAttackedEnemy].gameObject);
                }

                //if dead you won, if not enemy Turn
                if (isEnemyDead)
                {
                    bool allEnemiesAreDead = true;
                    enemyBattleCheckers[selectedEnemy].IsDead = true;

                    enemyButtons[selectedEnemy].SetActive(false);

                    foreach (var enemy in enemyBattleCheckers)
                    {
                        if (!enemy.IsDead)
                            allEnemiesAreDead = false;
                    }

                    if (allEnemiesAreDead)
                    {
                        Destroy(instantiatedEnemies[selectedEnemy].gameObject);
                        Destroy(instantiatedEnemyHealthBars[selectedEnemy].gameObject);
                        battleState = BattleState2.WON;
                        DeathEnemyAnimation();
                        DestroyAllHealthBars();
                        StartCoroutine(EndBattle());
                    }
                    else
                    {
                        Destroy(instantiatedEnemies[selectedEnemy].gameObject);
                        Destroy(instantiatedEnemyHealthBars[selectedEnemy].gameObject);
                        battleState = BattleState2.ENEMYTURN;
                        instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetLoss(instantiatedEnemies[selectedEnemy].GetComponent<Enemy>().CurrentHP);

                        do
                        {
                            selectedEnemy = UnityEngine.Random.Range(0, instantiatedEnemies.Length);
                        } while (enemyBattleCheckers[selectedEnemy].IsDead);
                        DestroyEnemyIndicator();

                        if (!firstEnemyAttackDone)
                        {
                            firstEnemyAttackDone = true;
                            StartCoroutine(FirstEnemyAttacks());
                        }
                        else
                            StartCoroutine(EnemyTurn());
                    }
                }
                else
                {
                    battleState = BattleState2.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
            }
            else if (battleState == BattleState2.ENEMYBUSY)
            {

                yield return new WaitForSeconds(2f);
                instantiatedEnemies[enemyToAttack].GetComponent<SkeletonAnimation>().AnimationName = "idle";

                int attackedHero;

                do
                {
                    attackedHero = UnityEngine.Random.Range(0, instantiatedHeroes.Length);
                } while (heroBattleCheckers[attackedHero].IsDead);

                for (int i = 0; i < instantiatedHeroes.Length; i++)
                {

                    if (heroBattleCheckers[i].IsDead == false)
                    {
                        if (instantiatedHeroes[i].GetComponent<Hero>().CurrentHP < instantiatedHeroes[attackedHero].GetComponent<Hero>().CurrentHP &&
                        instantiatedHeroes[i].GetComponent<Hero>().MaxHP != instantiatedHeroes[i].GetComponent<Hero>().CurrentHP)
                            attackedHero = i;
                    }
                }

                bool isCrit = false;
                int damage = instantiatedEnemies[enemyToAttack].GetComponent<Enemy>().getDamageWithCrit(ref isCrit);
                bool isPlayerDead = instantiatedHeroes[attackedHero].GetComponent<Hero>().TakeDamage(damage);
                instantiatedHeroHealthBars[attackedHero].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedHeroes[attackedHero].GetComponent<Hero>().CurrentHP);
                instantiatedHeroes[attackedHero].GetComponent<CharAnimation>().HitAnim();
                DamagePopUp.Create(instantiatedHeroes[attackedHero].position, damage, isCrit);

                yield return new WaitForSeconds(1f);

                //if dead you lose, if not enemy Turn
                if (isPlayerDead)
                {
                    bool allHeroesAreDead = true;
                    heroBattleCheckers[attackedHero].IsDead = true;

                    heroButtons[attackedHero].SetActive(false);
                    Destroy(instantiatedHeroes[attackedHero].gameObject);
                    Destroy(instantiatedHeroHealthBars[attackedHero].gameObject);
                    DestroyHeroIndicator();

                    foreach (var hero in heroBattleCheckers)
                    {
                        if (!hero.IsDead)
                            allHeroesAreDead = false;
                    }

                    if (allHeroesAreDead)
                    {
                        battleState = BattleState2.LOST;
                        DeathHeroAnimation();
                        StartCoroutine(EndBattle());
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.8f);
                        StartCoroutine(PlayerTurn());
                    }
                }
                else
                {
                    yield return new WaitForSeconds(0.8f);
                    StartCoroutine(PlayerTurn());
                }
            }
        }


        private IEnumerator BossAbility(int bossFight)
        {

            switch (bossFight)
            {
                case 1:
                    //FireGolem
                    //Golems Presence makes the area hot and players take damage each turn while rushing to bring down his high health points
                    for (int i = 0; i < heroPrefabs.Length && i < playerVehicle.PartySize; i++)
                    {
                        if (!heroBattleCheckers[i].IsDead)
                        {
                            instantiatedHeroes[i].GetComponent<Hero>().TakeDamage(30);
                            instantiatedHeroHealthBars[i].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedHeroes[i].GetComponent<Hero>().CurrentHP);
                            DamagePopUp.Create(instantiatedHeroes[i].position, 30, false);
                            instantiatedHeroes[i].GetComponent<CharAnimation>().HitAnim();
                        }
                    }
                    break;

                case 2:
                    //Dragon
                    //Dragon attacks each turn with regular attack and every 3 turns does a fire breath attack that deals a lot of damage to all heroes

                    

                    if (specialCounter == 0 || instantiatedEnemies[enemyToAttack].GetComponent<Enemy>().CurrentHP <= 660)
                    {
                        instantiatedEnemies[enemyToAttack].GetComponent<AudioPlayer>().PlayAbility();
                        isAbility = true;
                        instantiatedEnemies[enemyToAttack].GetComponent<SkeletonAnimation>().AnimationName = "skill";

                        yield return new WaitForSeconds(5f);

                        instantiatedEnemies[enemyToAttack].GetComponent<SkeletonAnimation>().AnimationName = "idle";

                        for (int i = 0; i < heroPrefabs.Length && i < playerVehicle.PartySize; i++)
                        {
                            if (!heroBattleCheckers[i].IsDead)
                            {
                                if(instantiatedEnemies[enemyToAttack].GetComponent<Enemy>().CurrentHP <= 660)
                                {
                                    instantiatedHeroes[i].GetComponent<Hero>().TakeDamage(600);
                                    DamagePopUp.Create(instantiatedHeroes[i].position, 600, true);
                                }
                                else
                                {
                                    instantiatedHeroes[i].GetComponent<Hero>().TakeDamage(100);
                                    DamagePopUp.Create(instantiatedHeroes[i].position, 100, false);
                                }

                                instantiatedHeroHealthBars[i].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedHeroes[i].GetComponent<Hero>().CurrentHP);
                                instantiatedHeroes[i].GetComponent<CharAnimation>().HitAnim();


                                if(instantiatedHeroes[i].GetComponent<Hero>().CurrentHP <= 0)
                                {
                                    instantiatedHeroes[i].GetComponent<BattleChecker>().IsDead = true;
                                    Destroy(instantiatedHeroes[i].gameObject);
                                    Destroy(instantiatedHeroHealthBars[i].gameObject);
                                    heroButtons[i].SetActive(false);
                                }
                            }

                        }

                        allHeroesDeadFromAbility = true;
                        DestroyHeroIndicator();

                        foreach (var hero in heroBattleCheckers)
                        {
                            if (!hero.IsDead)
                                allHeroesDeadFromAbility = false;
                        }

                        if (allHeroesDeadFromAbility)
                        {
                            battleState = BattleState2.LOST;
                            DeathHeroAnimation();
                            StartCoroutine(EndBattle());
                        }
                        specialCounter = 3;
                    }
                    else 
                        specialCounter--;

                    break;
                case 3:
                    //Demon
                    //Demon focused on defeating as many heroes as possible attacks every turn, every 3rd turn he stops to mark a hero
                    //after 2 turns the demon uses his special which instantly defeats a hero unless there was a warrior that used block


                    if (specialCounter == 0)
                    {
                        instantiatedEnemies[enemyToAttack].GetComponent<AudioPlayer>().PlayAbility();
                        isAbility = true;
                        instantiatedEnemies[enemyToAttack].GetComponent<SkeletonAnimation>().AnimationName = "skill";

                        yield return new WaitForSeconds(5f);

                        instantiatedEnemies[enemyToAttack].GetComponent<SkeletonAnimation>().AnimationName = "idle";

                        int attackedHero;
                        do
                        {
                            attackedHero = UnityEngine.Random.Range(0, instantiatedHeroes.Length);
                        } while (heroBattleCheckers[attackedHero].IsDead);

                        instantiatedHeroes[attackedHero].GetComponent<Hero>().TakeDamage(10000);
                        instantiatedHeroHealthBars[attackedHero].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedHeroes[attackedHero].GetComponent<Hero>().CurrentHP);
                        DamagePopUp.Create(instantiatedHeroes[attackedHero].position, 10000, false);
                        instantiatedHeroes[attackedHero].GetComponent<CharAnimation>().HitAnim();

                        if (instantiatedHeroes[attackedHero].GetComponent<Hero>().CurrentHP <= 0)
                        {
                            instantiatedHeroes[attackedHero].GetComponent<BattleChecker>().IsDead = true;
                            Destroy(instantiatedHeroes[attackedHero].gameObject);
                            Destroy(instantiatedHeroHealthBars[attackedHero].gameObject);
                        }

                        specialCounter = 3;
                    }
                    else
                        specialCounter--;

                    break;
                default:
                    Debug.Log("wasnt boss 1 2 or 3");
                    break;
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
                instantiatedHeroes[selectedHero].GetComponent<AudioPlayer>().PlayAttack();
                instantiatedHeroes[selectedHero].GetComponent<CharAnimation>().AttackAnim();
                StartCoroutine(CalculateDamage());
            }
        }

        private void DotDamageToEnemies()
        {
            bool isEnemyDeadDOT;
            int casterDOTDamage = HeroClassDB.getHero(3).Damage / 3;

            //if enemy turn
            if (battleState == BattleState2.PLAYERTURN)
            {
                //go through list of enemies
                for (int i = 0; i < instantiatedEnemies.Length; i++)
                {
                    //check if enemy is dead
                    if (!enemyBattleCheckers[i].IsDead)
                    {
                        //check if enemy has a DOT Counter
                        if (enemyBattleCheckers[i].DOTCounter > 0)
                        {
                            //deal damage from Spellcaster
                            isEnemyDeadDOT = instantiatedEnemies[i].GetComponent<Enemy>().TakeDamage(casterDOTDamage);
                            DamagePopUp.Create(instantiatedEnemies[i].position, casterDOTDamage, false);


                            //update enemy health
                            instantiatedEnemyHealthBars[i].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedEnemies[i].GetComponent<Enemy>().CurrentHP);

                            //decrement counter
                            enemyBattleCheckers[i].DOTCounter -= 1;

                            //check if all dead
                            if (isEnemyDeadDOT)
                            {
                                enemyBattleCheckers[i].IsDead = true;
                                Destroy(instantiatedEnemies[i].gameObject);
                                Destroy(instantiatedEnemyHealthBars[i].gameObject);
                                enemyButtons[i].SetActive(false);
                                DestroyEnemyIndicator();

                                bool allEnemiesAreDead = true;

                                foreach (var enemy in enemyBattleCheckers)
                                {
                                    if (!enemy.IsDead)
                                        allEnemiesAreDead = false;
                                }

                                if (allEnemiesAreDead)
                                {

                                    battleState = BattleState2.WON;
                                    DeathEnemyAnimation();
                                    DestroyAllHealthBars();
                                    StartCoroutine(EndBattle());
                                }
                                else
                                {
                                    do
                                    {
                                        selectedEnemy = UnityEngine.Random.Range(0, instantiatedEnemies.Length);
                                    } while (enemyBattleCheckers[selectedEnemy].IsDead);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OnAbilityButton()
        {
            if (battleState != BattleState2.PLAYERTURN)
                return;
            else
            {
                battleState = BattleState2.PLAYERBUSY;



                string name = instantiatedHeroes[selectedHero].name;
                switch (name)
                {
                    case "Warrior(Clone)":
                        shouldDefend = true;
                        DestroyHeroIndicator();
                        DestroyEnemyIndicator();
                        heroBattleCheckers[selectedHero].UsedTurn = true;
                        heroButtons[selectedHero].SetActive(false);
                        battleState = BattleState2.ENEMYTURN;
                        StartCoroutine(EnemyTurn());
                        break;
                    case "Mage(Clone)":
                        shouldHeal = true;
                        dialogueText.text = "Choose a hero to heal";
                        DestroyHeroIndicator();
                        DestroyEnemyIndicator();


                        for (int i = 0; i < heroBattleCheckers.Length; i++)
                        {
                            if (!heroBattleCheckers[i].IsDead)
                            {
                                heroButtons[i].SetActive(true);
                            }
                        }

                        break;
                    case "Ranger(Clone)":
                        instantiatedHeroes[selectedHero].GetComponent<CharAnimation>().HealAnim();
                        instantiatedHeroes[selectedHero].GetComponent<AudioPlayer>().PlayAbility();

                        int enemiesAlive = 0;

                        foreach (var item in enemyBattleCheckers)
                        {
                            if (!item.IsDead)
                                enemiesAlive++;
                        }

                        if (enemiesAlive > 1)
                            shouldAttackTwo = true;

                        StartCoroutine(CalculateDamage());
                        break;

                    case "Spearman(Clone)":
                        instantiatedHeroes[selectedHero].GetComponent<CharAnimation>().HealAnim();
                        instantiatedHeroes[selectedHero].GetComponent<AudioPlayer>().PlayAbility();

                        int allEnemiesAlive = 0;

                        foreach (var item in enemyBattleCheckers)
                        {
                            if (!item.IsDead)
                                allEnemiesAlive++;
                        }

                        if (allEnemiesAlive > 2)
                            shouldAttackThree = true;
                        else if (allEnemiesAlive > 1)
                            shouldAttackTwo = true;

                        StartCoroutine(CalculateDamage());
                        break;

                    case "Wizard(Clone)":
                        instantiatedHeroes[selectedHero].GetComponent<CharAnimation>().HealAnim();
                        instantiatedHeroes[selectedHero].GetComponent<AudioPlayer>().PlayAbility();

                        enemyBattleCheckers[selectedEnemy].DOTCounter = 2;

                        bool isEnemyDead = instantiatedEnemies[selectedEnemy].GetComponent<Enemy>().TakeDamage(HeroClassDB.getHero(3).Damage / 3);
                        DamagePopUp.Create(instantiatedEnemies[selectedEnemy].position, HeroClassDB.getHero(3).Damage / 3, false);

                        instantiatedEnemyHealthBars[selectedEnemy].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedEnemies[selectedEnemy].GetComponent<Enemy>().CurrentHP);

                        if (isEnemyDead)
                        {

                            enemyBattleCheckers[selectedEnemy].IsDead = true;
                            Destroy(instantiatedEnemies[selectedEnemy].gameObject);
                            Destroy(instantiatedEnemyHealthBars[selectedEnemy].gameObject);
                            enemyButtons[selectedEnemy].SetActive(false);

                            bool allEnemiesAreDead = true;

                            foreach (var enemy in enemyBattleCheckers)
                            {
                                if (!enemy.IsDead)
                                    allEnemiesAreDead = false;
                            }

                            if (allEnemiesAreDead)
                            {
                                battleState = BattleState2.WON;
                                DeathEnemyAnimation();
                                DestroyAllHealthBars();
                                StartCoroutine(EndBattle());
                            }
                            else
                            {
                                do
                                {
                                    selectedEnemy = UnityEngine.Random.Range(0, instantiatedEnemies.Length);
                                } while (enemyBattleCheckers[selectedEnemy].IsDead);
                                battleState = BattleState2.ENEMYTURN;
                                StartCoroutine(EnemyTurn());

                            }

                        }
                        else
                        {
                            battleState = BattleState2.ENEMYTURN;
                            StartCoroutine(EnemyTurn());
                        }

                        DestroyHeroIndicator();
                        DestroyEnemyIndicator();
                        heroBattleCheckers[selectedHero].UsedTurn = true;
                        heroButtons[selectedHero].SetActive(false);
                        break;

                    default:
                        break;
                }
            }
        }

        private IEnumerator HealSelectedHero()
        {
            if (battleState == BattleState2.PLAYERBUSY)
            {
                instantiatedHeroes[selectedHero].GetComponent<CharAnimation>().HealAnim();
                instantiatedHeroes[selectedHero].GetComponent<AudioPlayer>().PlayAbility();

                instantiatedHeroes[selectedHeroToHeal].GetComponent<Hero>().Heal(instantiatedHeroes[selectedHeroToHeal].GetComponent<Hero>().MaxHP / 2);
                instantiatedHeroHealthBars[selectedHeroToHeal].GetChild(0).GetComponent<BattleHUD>().SetHP(instantiatedHeroes[selectedHeroToHeal].GetComponent<Hero>().CurrentHP);
                dialogueText.text = "They feel stronger!";
                yield return new WaitForSeconds(0.8f);
                battleState = BattleState2.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }

        IEnumerator EndBattle()
        {
            DestroyHeroIndicator();
            DestroyEnemyIndicator();

            yield return new WaitForSeconds(1.0f);

            foreach (var item in battleElements)
            {
                item.SetActive(false);
            }

            twoForOneButton.SetActive(false);
            blockButton.SetActive(false);
            healButton.SetActive(false);
            wizardAbilityButton.SetActive(false);
            spearmanAbilityButton.SetActive(false);

            if (battleState == BattleState2.WON)
            {
                
                dialogueText.text = "You won the battle!";

                if (wave < 3)
                {
                    StartCoroutine(StartNewBattle());
                }
                else
                {
                    StartCoroutine(ViewVictoryPopUp());
                }

            }
            else if (battleState == BattleState2.LOST)
            {
                defeatMusic.Play();
                defeatPopUp.SetActive(true);
                dialogueText.text = "You were defeated.";
            }
        }

        IEnumerator ViewVictoryPopUp()
        {
            yield return new WaitForSeconds(1f);
            victoryMusic.Play();
            victoryPopUp.SetActive(true);
        }

        private IEnumerator StartNewBattle()
        {
            selectedEnemy = 0;
            EnemyHolderBS.position = new Vector3(10.63f, 0.08f, 0);
            wave++;

            yield return new WaitForSeconds(1f);

            Parallex.ShouldMove = true;
            MoveHeroAimation();

            yield return new WaitForSeconds(2f);
            
            SpawnEnemy();

            dialogueText.text = "An enemy approaches!!";

            MoveEnemiesInC = StartCoroutine(MoveEnemiesIn());

            yield return new WaitForSeconds(2.5f);

            foreach (var item in battleElements)
            {
                item.SetActive(true);
            }

            InstantiateHeroHealthBars();
            InstantiateEnemyHealthBars();
            SpawnHeroButtons();
            SpawnEnemyButtons();


            yield return new WaitForSeconds(1f);
            SetActiveChar(playersCharActions[0]);
            StartCoroutine(PlayerTurn());
        }



        public IEnumerator MoveHeroVehicleIn(float countdownValue = 5f)
        {

            while (countdownValue > 0)
            {
                try
                {
                    if (instantiatedHeroVehicle.position != HeroVehicleBS.position)
                    {
                        Vector3 newPos = Vector3.MoveTowards(instantiatedHeroVehicle.transform.position, HeroVehicleBS.position, 0.22f); 
                        instantiatedHeroVehicle.transform.position = newPos;
                    }
                    else
                        countdownValue = 0;
                }
                catch (Exception) { }

                yield return new WaitForSeconds(.022f);

                
                    countdownValue -= 0.03f;
            }

            Parallex.ShouldMove = true;
        }


        private void MoveHeroAimation()
        {
            instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "run";
        }

        
        private void StopHeroAimation()
        {
            instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
        }

        private void DeathHeroAnimation()
        {
            instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().loop = false;
            instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "death";
        }


        private void StopEnemyAnimation()
        {
            foreach (var enemy in instantiatedEnemies)
            {
                enemy.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                
            }
        }

        private void DeathEnemyAnimation()
        {
            
        }

        public IEnumerator MoveEnemiesOut(float countdownValue = 5f)
        {

            Vector3 pos = new Vector3(-15f, 0.059f, -10.25f);
            while (countdownValue > 0)
            {
                try
                {
                    if (EnemyHolderBS.position != pos)
                    {
                        Vector3 newPos = Vector3.MoveTowards(EnemyHolderBS.transform.position, pos, 0.05f);
                        EnemyHolderBS.transform.position = newPos;
                    }
                }
                catch (Exception)
                {
                    StopCoroutine(MoveEnemiesOutC);
                }

                yield return new WaitForSeconds(.02f);
                countdownValue -= 0.02f;
            }
        }

        public IEnumerator MoveEnemiesIn(float countdownValue = 5f)
        {
            Vector3 pos;
            if(wave == 1 || wave == 3)
                pos = new Vector3(6.4f, 0.08f, 0f);
            else
                pos = new Vector3(4.8f, 0.08f, 0f);
            while (countdownValue > 0)
            {
                try
                {
                    if (EnemyHolderBS.position != pos)
                    {
                        Vector3 newPos = Vector3.MoveTowards(EnemyHolderBS.transform.position, pos, 0.07f);
                        EnemyHolderBS.transform.position = newPos;
                    }
                    else
                    {
                        countdownValue = 0;
                    }
                }
                catch (Exception) { }

                yield return new WaitForSeconds(.022f);
                countdownValue -= 0.02f;
            }
            StopHeroAimation();
            StopEnemyAnimation();
            Parallex.ShouldMove = false;
            StopCoroutine(MoveEnemiesInC);
            
        }

        public IEnumerator MoveHeroVehicleOut(float countdownValue = 5f)
        {
            Vector3 pos = new Vector3(10.5f, 0.059f, 0f);
            while (countdownValue > 0)
            {
                try
                {
                    if (instantiatedHeroVehicle.position != pos)
                    {
                        Vector3 newPos = Vector3.MoveTowards(instantiatedHeroVehicle.transform.position, pos, 7f * Time.deltaTime * Screen.width / Screen.height);
                        instantiatedHeroVehicle.transform.position = newPos;
                    }
                }
                catch (Exception) { }

                yield return new WaitForSeconds(.01f);

                countdownValue -= 0.03f;
            }

            levelLoader.StartTransition();
            yield return new WaitForSeconds(0.5f);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Town");
        }

        private void InstantiateHeroHealthBars()
        {
            for (int i = 0; i < heroes.Length; i++)
            {
                if (!heroBattleCheckers[i].IsDead)
                {
                    heroHealthBarPrefabs[i] = (GameObject)Resources.Load("HealthBar", typeof(GameObject));
                    instantiatedHeroHealthBars[i] = Instantiate(heroHealthBarPrefabs[i].transform, heroBSList[i].position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                    instantiatedHeroHealthBars[i].GetChild(0).GetComponent<BattleHUD>().SetHUDUnitHPHero(instantiatedHeroes[i].GetComponent<Hero>());
                }
            }
        }

        private void InstantiateEnemyHealthBars()
        {
            for (int i = 0; i < instantiatedEnemies.Length; i++)
            {
                enemyHealthBarPrefabs[i] = (GameObject)Resources.Load("HealthBar", typeof(GameObject));
                if (instantiatedEnemies[i].TryGetComponent(out DragonBoss eb) || instantiatedEnemies[i].TryGetComponent(out DemonBoss db) || instantiatedEnemies[i].TryGetComponent(out FireGolemBoss fb))
                {
                    instantiatedEnemyHealthBars[i] = Instantiate(enemyHealthBarPrefabs[i].transform, enemyBSList[i].position + new Vector3(-0.5f, 5f, 0), Quaternion.identity);
                    instantiatedEnemyHealthBars[i].localScale += new Vector3(0.0075f, 0.0065f, 0f);
                }
                else if (instantiatedEnemies[i].TryGetComponent(out EnemyMid em))
                {

                    instantiatedEnemyHealthBars[i] = Instantiate(enemyHealthBarPrefabs[i].transform, enemyBSList[i].position + new Vector3(0.0f, 3.3f, 0), Quaternion.identity);
                    instantiatedEnemyHealthBars[i].localScale += new Vector3(0.0015f, 0.001f, 0f);

                }
                else 
                {
                    instantiatedEnemyHealthBars[i] = Instantiate(enemyHealthBarPrefabs[i].transform, enemyBSList[i].position + new Vector3(0.0f, 2f, 0), Quaternion.identity);
                    instantiatedEnemyHealthBars[i].localScale += new Vector3(0.0015f, 0.001f, 0f);
                }
                instantiatedEnemyHealthBars[i].GetChild(0).GetComponent<BattleHUD>().SetHUDUnitHPEnemy(instantiatedEnemies[i].GetComponent<Enemy>());
                instantiatedEnemyHealthBars[i].SetParent(EnemyHolderBS);
                
            }
        }

        private void InstantiateHeroIndicator()
        {
            if (instantiatedHeroIndicator)
            {
                DestroyHeroIndicator();
            }

            heroIndicator = (GameObject)Resources.Load("GreenArrow", typeof(GameObject));
            instantiatedHeroIndicator = Instantiate(heroIndicator.transform, instantiatedHeroes[selectedHero].transform.position + new Vector3(0, 2f, 0), Quaternion.Euler(0, 0, 0));
        }

        private void DestroyHeroIndicator()
        {
            try
            {
                Destroy(instantiatedHeroIndicator.gameObject);

            }
            catch (Exception)
            {

                
            }
        }

        private void InstantiateEnemyIndicator()
        {
            if (instantiatedEnemyIndicator)
            {
                DestroyEnemyIndicator();
            }

            enemyIndicator = (GameObject)Resources.Load("RedArrow", typeof(GameObject));
            if (instantiatedEnemies[selectedEnemy].TryGetComponent(out DragonBoss eb) || instantiatedEnemies[selectedEnemy].TryGetComponent(out DemonBoss db) || instantiatedEnemies[selectedEnemy].TryGetComponent(out FireGolemBoss fb))
            {
                //DONT INSTANTIATE INDICATOR NOT NECESSARY
              //  instantiatedEnemyIndicator = Instantiate(enemyIndicator.transform, instantiatedEnemies[selectedEnemy].transform.position + new Vector3(0f, 5.5f, 0), Quaternion.Euler(0, 0, 0));
            }
            else if (instantiatedEnemies[selectedEnemy].TryGetComponent(out EnemyMid em))
            {

                instantiatedEnemyIndicator = Instantiate(enemyIndicator.transform, instantiatedEnemies[selectedEnemy].transform.position + new Vector3(0f, 3.8f, 0), Quaternion.Euler(0, 0, 0));
            }
            else
            {
                instantiatedEnemyIndicator = Instantiate(enemyIndicator.transform, instantiatedEnemies[selectedEnemy].transform.position + new Vector3(0f, 2.5f, 0), Quaternion.Euler(0, 0, 0));
            }
         }

        private void DestroyEnemyIndicator()
        {
            try
            {
                Destroy(instantiatedEnemyIndicator.gameObject);
            }
            catch (Exception) { }

        }

        private void DestroyAllHealthBars()
        {
            for (int i = 0; i < heroBattleCheckers.Length; i++)
            {
                if (!heroBattleCheckers[i].IsDead)
                {
                    Destroy(instantiatedHeroHealthBars[i].gameObject);
                }
            }
        }

        private void SpawnHeroButtons()
        {
            for (int i = 0; i < heroBattleCheckers.Length; i++)
            {
                if (!heroBattleCheckers[i].IsDead)
                    heroButtons[i].SetActive(true);

                heroButtons[i].transform.position = heroBSList[i].transform.position + new Vector3(0, 0.6f, 0);
            }

        }

        private void SpawnEnemyButtons()
        {
            for (int i = 0; i < instantiatedEnemies.Length; i++)
            {
                enemyButtons[i].SetActive(true);
                enemyButtons[i].transform.position = enemyBSList[i].transform.position + new Vector3(0, 1.2f, 0);
                enemyButtons[i].transform.localScale = new Vector3(3.316683f, 6.803726f, 8.284678f);
                enemyButtons[i].transform.localScale += new Vector3(4, 12, 0);
            }
        }

        private void DisplayAbilityButton()
        {
            switch (instantiatedHeroes[selectedHero].name)
            {
                case "Warrior(Clone)":
                    blockButton.SetActive(true);
                    healButton.SetActive(false);
                    twoForOneButton.SetActive(false);
                    spearmanAbilityButton.SetActive(false);
                    wizardAbilityButton.SetActive(false);
                    break;
                case "Mage(Clone)":
                    blockButton.SetActive(false);
                    healButton.SetActive(true);
                    twoForOneButton.SetActive(false);
                    spearmanAbilityButton.SetActive(false);
                    wizardAbilityButton.SetActive(false);
                    break;
                case "Ranger(Clone)":
                    blockButton.SetActive(false);
                    healButton.SetActive(false);
                    twoForOneButton.SetActive(true);
                    spearmanAbilityButton.SetActive(false);
                    wizardAbilityButton.SetActive(false);
                    break;
                case "Spearman(Clone)":
                    blockButton.SetActive(false);
                    healButton.SetActive(false);
                    twoForOneButton.SetActive(false);
                    spearmanAbilityButton.SetActive(true);
                    wizardAbilityButton.SetActive(false);
                    break;
                case "Wizard(Clone)":
                    blockButton.SetActive(false);
                    healButton.SetActive(false);
                    twoForOneButton.SetActive(true);
                    spearmanAbilityButton.SetActive(false);
                    wizardAbilityButton.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        

        public void AcceptDefeat()
        {
            StartCoroutine(LoadLevel("CharSelect"));
        }

        IEnumerator LoadLevel(string sceneName)
        {
            levelLoader.StartTransition();
            yield return new WaitForSeconds(0.5f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
