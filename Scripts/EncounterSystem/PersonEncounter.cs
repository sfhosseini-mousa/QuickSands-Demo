using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System;
using UnityEngine.Rendering;

namespace Sands
{
    public class PersonEncounter : MonoBehaviour
    {
        //persons
        private Transform instantiatedPerson;
        [SerializeField] private Transform personStation;
        //popups
        [SerializeField] private GameObject WarriorPopUp;
        [SerializeField] private GameObject PersonEncounterFin;
        [SerializeField] private GameObject ResolutionRobText;
        [SerializeField] private GameObject ResolutionAidPopup;

        //warrior descriptions
        [SerializeField] private GameObject warriorEncounterText;
        [SerializeField] private GameObject WarriorResolutionAidText;
        [SerializeField] private GameObject WarriorResolutionLeaveText;

        //mage descriptions
        [SerializeField] private GameObject mageEncounterText;
        [SerializeField] private GameObject mageResolutionAidText;
        [SerializeField] private GameObject mageResolutionLeaveText;
        //ranger descriptions
        [SerializeField] private GameObject rangerEncounterText;
        [SerializeField] private GameObject rangerResolutionBuyText;
        [SerializeField] private GameObject rangerResolutionLeaveText;

        //other
        [SerializeField] private GameObject wizardEncounterText;
        [SerializeField] private GameObject spearmanEncounterText;
        //buttons
        [SerializeField] private GameObject warriorRobButt;
        [SerializeField] private GameObject warriorAidButt;
        [SerializeField] private GameObject warriorIgnoreButt;
        [SerializeField] private GameObject rangerRobButt;
        [SerializeField] private GameObject rangerBuyButt;
        [SerializeField] private GameObject rangerIgnoreButt;
        [SerializeField] private GameObject mageLieButt;
        [SerializeField] private GameObject mageTruthButt;
        [SerializeField] private GameObject mageRobbButt;
        
        //inventory stuff
        [SerializeField] Text goldAmount;
        [SerializeField] Text strangerFaction;
        [SerializeField] Text strangerFactionRob;
        [SerializeField] Text strangerFactionResolution;
        GameObject itemHolder1, itemHolder2, itemHolder3;
        [SerializeField] GameObject itemHolder1Pos, itemHolder2Pos, itemHolder3Pos;


        Tradeable item1;
        Tradeable item2;
        Tradeable item3;
        int randomID = 0;
        int randomID2 = 0;
        int randomAmount = 0;
        int randomAmount2 = 0;
        System.Random itemRandy = new System.Random();
        int factionID = 0;
        private GameObject personPrefab;
        private Coroutine MovePersonInC = null;
        private Coroutine MovePersonOutC = null;
        int randy;
        [SerializeField] private GameObject Gauge;

        public void Begin()
        {
             randy = UnityEngine.Random.Range(1, 4);

            Parallex.ShouldMove = true;
            MoveHeroAimation();
            SpawnPerson();
            StartCoroutine(PersonMovement());
            MoveEnemyAnimation();
        }


        private void SpawnPerson()
        {
            System.Console.WriteLine(randy);
            Parallex.ShouldMove = false;
            StopHeroAnimation();
            //instantiate a warrior mage or ranger
            if (randy < 2)
            {
                //make characters move and instantiate pop-up with relavent encounter information/choices
                Hero warrior = new Warrior((Warrior)HeroClassDB.getHero(0));
                personPrefab = (GameObject)Resources.Load(warrior.GetType().Name, typeof(GameObject));
            }
            else if (randy < 3)
            {
                //make characters move and instantiate pop-up with relavent encounter information/choices
                Hero mage = new Mage((Mage)HeroClassDB.getHero(1));
                personPrefab = (GameObject)Resources.Load(mage.GetType().Name, typeof(GameObject));
            }
            else if (randy < 4)
            {
                //make characters move and instantiate pop-up with relavent encounter information/choices
                Hero ranger = new Ranger((Ranger)HeroClassDB.getHero(2));
                personPrefab = (GameObject)Resources.Load(ranger.GetType().Name, typeof(GameObject));
            }
            else if (randy < 5)
            {
                //make characters move and instantiate pop-up with relavent encounter information/choices
                Hero wizard = new Wizard((Wizard)HeroClassDB.getHero(3));
                personPrefab = (GameObject)Resources.Load(wizard.GetType().Name, typeof(GameObject));
            }
            else if (randy < 6)
            {
                //make characters move and instantiate pop-up with relavent encounter information/choices
                Hero spearman = new Spearman((Spearman)HeroClassDB.getHero(3));
                personPrefab = (GameObject)Resources.Load(spearman.GetType().Name, typeof(GameObject));
            }
            personPrefab.GetComponent<Hero>().SkinTire = 1;
            personPrefab.GetComponent<Hero>().setSkin(personPrefab);
            personPrefab.GetComponent<SortingGroup>().sortingOrder = 9;

            instantiatedPerson = Instantiate(personPrefab.transform, personStation.position, Quaternion.Euler(0, 180, 0));
        }

        private void activatePopup()
        {
            if (randy < 2)
            {
                WarriorPopUp.SetActive(true);
                warriorEncounterText.SetActive(true);
                factionSetter();
                if (PlayerInventory.Money >= 1000)
                    warriorAidButt.SetActive(true);
                warriorIgnoreButt.SetActive(true);
                warriorRobButt.SetActive(true);
            }
            else if (randy < 3)
            {
                WarriorPopUp.SetActive(true);
                mageEncounterText.SetActive(true);
                factionSetter();
                mageLieButt.SetActive(true);
                mageRobbButt.SetActive(true);
                mageTruthButt.SetActive(true);
            }
            else if (randy < 4)
            {
                WarriorPopUp.SetActive(true);
                rangerEncounterText.SetActive(true);
                factionSetter();
                rangerRobButt.SetActive(true);
                if (PlayerInventory.Money >= 3000)
                    rangerBuyButt.SetActive(true);
                rangerIgnoreButt.SetActive(true);
            }
            else if (randy < 5)
            {
                WarriorPopUp.SetActive(true);
                wizardEncounterText.SetActive(true);
            }
            else if (randy < 6)
            {
                WarriorPopUp.SetActive(true);
                spearmanEncounterText.SetActive(true);
            }
        }

        //generate loot from robbing him, subtract reputation of faction in nearby town
        public void robHimButton()
        {
            WarriorPopUp.SetActive(false);

            int num = UnityEngine.Random.Range(10, 21);
            int faction = Player.LocationToTravelTo.Territory - 1;
            Player.FactionReputation[faction] -= (ushort)num;
           
            PersonEncounterFin.SetActive(true);
          //  PersonEncounterFin.GetComponent<LootPopUp>().GenerateLootScreen();

            generateLoot();
            ResolutionRobText.SetActive(true);
            strangerFactionRob.text = factionName(factionID) + " - " + num;
         //   inventoryOneAmount.text = item1.ItemName;
         //   inventoryTwoAmount.text = item2.ItemName;
          //  itemOneAmount.text = randomAmount.ToString();
          //  itemTwoAmount.text = randomAmount2.ToString();
            EncounterSystem.DidRob = true;

        }

        //lose loot from aiding him, gain reputation with faction in nearby town
        public void aidWarriorButton()
        {
            WarriorPopUp.SetActive(false);

            int num = UnityEngine.Random.Range(10, 21);
            int faction = Player.LocationToTravelTo.Territory - 1;
            Player.FactionReputation[faction] += (ushort)num;

            ResolutionAidPopup.SetActive(true);
            WarriorResolutionAidText.SetActive(true);
            strangerFactionResolution.text = factionName(factionID) + " + " + num;
            if (PlayerInventory.Money >= 1000)
            {
                PlayerInventory.Money -= 1000;
                goldAmount.text = "-1000";

            }

           // aidResItemName.text = "Rations - 3";
       

        }

        //A fucking loot box system
        public void aidRangerButton()
        {     
            WarriorPopUp.SetActive(false);
            int num = UnityEngine.Random.Range(10, 21);
            int faction = Player.LocationToTravelTo.Territory - 1;
            Player.FactionReputation[faction] += (ushort)num;

            int diamondChance = UnityEngine.Random.Range(1, 21);

            if (diamondChance <= 5)
                lootBox(1);
            else if (diamondChance <= 10)
                lootBox(3);
            else if (diamondChance <= 15)
                lootBox(5);
            else if (diamondChance <= 19)
                lootBox(7);
            else
                lootBox(9);

            ResolutionAidPopup.SetActive(true);
            rangerResolutionBuyText.SetActive(true);
            strangerFactionResolution.text = factionName(factionID) + " + " + num;
            PlayerInventory.Money -= 1000;
            goldAmount.text = " -3000";
        }

        public void tellMageButton()
        {
            WarriorPopUp.SetActive(false);
            int num = UnityEngine.Random.Range(10, 21);
            int faction = Player.LocationToTravelTo.Territory - 1;
            Player.FactionReputation[faction] += (ushort)num;
            ResolutionAidPopup.SetActive(true);
            mageResolutionAidText.SetActive(true);
            strangerFactionResolution.text = factionName(factionID) + " + " + num;

            PlayerInventory.Money += 2000;
            goldAmount.text = " + 2000";
        }

        //nothing happens
        public void leaveHimButton()
        {
            EncounterSystem.DidRob = true;
            WarriorPopUp.SetActive(false);
            moveOn();

        }


        public void lootBox(int id)
        {
            int capacity = 0;
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

            item3 = TradeableDatabase.getTradeable(id);

            if (carrying + item3.Weight * 5 <= capacity)
            {
                GameObject itemHolder = (GameObject)Resources.Load("ItemHolder", typeof(GameObject));
                //put loot from loot screen in player inventory
                PlayerInventory.AddToInventory(id + 1, 5);

                itemHolder3 = Instantiate(itemHolder, itemHolder3Pos.transform);

                itemHolder3.GetComponent<ItemHolder>().itemNameText.text = item3.ItemName;
                itemHolder3.GetComponent<ItemHolder>().priceText.text = "";
                itemHolder3.GetComponent<ItemHolder>().countText.text = "5";
                itemHolder3.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item3.ItemName);

                itemHolder3Pos.transform.localScale = new Vector3(0.75f, 0.75f, 1f);

                //    aidResItemName.text = item1.ItemName;
                //   aidResItemAmount.text = " + 5";

                carrying = 0;
                foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
                    carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;              
            }
            else
                PlayerInventory.Money += 1000;
        }



        public void generateLoot()
        {
            int capacity = 0;
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

            int partySize = 1;

            do
            {
                if (Player.HasVehicle)
                {
                    partySize = Player.CurrentVehicle.PartySize;
                }
                if (partySize == 1)
                {
                    randomID = itemRandy.Next(1, 3);
                    randomID2 = itemRandy.Next(1, 3);


                }
                if (partySize == 2)
                {
                    randomID = itemRandy.Next(1, 5);
                    randomID2 = itemRandy.Next(1, 5);


                }
                if (partySize == 3)
                {
                    randomID = itemRandy.Next(1, 7);
                    randomID2 = itemRandy.Next(1, 7);


                }
                if (partySize == 4 || partySize == 5)
                {
                    randomID = itemRandy.Next(1, 11);
                    randomID2 = itemRandy.Next(1, 11);


                }
            } while (randomID == randomID2);



            randomAmount = itemRandy.Next(1, 5);
            randomAmount2 = itemRandy.Next(1, 5);


            //loot items and amount
            item1 = TradeableDatabase.getTradeable(randomID - 1);
            item2 = TradeableDatabase.getTradeable(randomID2 - 1);

            GameObject itemHolder = (GameObject)Resources.Load("ItemHolder", typeof(GameObject));

            itemHolder1 = Instantiate(itemHolder, itemHolder1Pos.transform);
            itemHolder2 = Instantiate(itemHolder, itemHolder2Pos.transform);

            itemHolder1.GetComponent<ItemHolder>().itemNameText.text = item1.ItemName;
            itemHolder1.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder1.GetComponent<ItemHolder>().countText.text = randomAmount.ToString();
            itemHolder1.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item1.ItemName);

            itemHolder2.GetComponent<ItemHolder>().itemNameText.text = item2.ItemName;
            itemHolder2.GetComponent<ItemHolder>().priceText.text = "";
            itemHolder2.GetComponent<ItemHolder>().countText.text = randomAmount2.ToString();
            itemHolder2.GetComponent<ItemHolder>().iconImage.sprite = Resources.Load<Sprite>(item2.ItemName);

            itemHolder1Pos.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            itemHolder2Pos.transform.localScale = new Vector3(0.75f, 0.75f, 1f);

            if (carrying + item1.Weight*randomAmount + item2.Weight*randomAmount <= capacity)
            {
                //put loot from loot screen in player inventory
                    PlayerInventory.AddToInventory(randomID, randomAmount);
                    PlayerInventory.AddToInventory(randomID2, randomAmount);
                    carrying = 0;
                    foreach (var inventoryTradeable in PlayerInventory.TradeableInventory)
                    {
                        carrying += inventoryTradeable.OwnedTradeable.Weight * inventoryTradeable.Count;
                    }

                
            }
        }


       
        public string factionName(int factNum)
        {
              if (factNum == 1)
               return "Republic of Veden";
            if (factNum == 2)
                return "Fara Empire";
            if (factNum == 3)
                return "The Kaiserreich";
            else
                return "Nowhere";
        }

        public int factionSetter()
        {
            int randomFaction = UnityEngine.Random.Range(1, 3);
            if (randomFaction == 1)
            {
                strangerFaction.text = factionName(Player.LocationToTravelTo.Territory);
                factionColour(Player.LocationToTravelTo.Territory);
                factionID = Player.LocationToTravelTo.Territory;
                return factionID;
            }
            else if (randomFaction == 2)
            {
                strangerFaction.text = factionName(Player.CurrentLocation.Territory);
                factionColour(Player.CurrentLocation.Territory);
                factionID = Player.CurrentLocation.Territory;
                return factionID;
            }
            else
                return factionID;
        }

        public void factionColour(int factNum)
        {
            if (factNum == 1)
            {
                strangerFaction.color = Color.blue;
                strangerFactionRob.color = Color.blue;
                strangerFactionResolution.color = Color.blue;
            }
            if (factNum == 2)
            {
                strangerFaction.color = Color.green;
                strangerFactionRob.color = Color.green;
                strangerFactionResolution.color = Color.green;
            }
            if (factNum == 3)
            {
                strangerFaction.color = Color.red;
                strangerFactionRob.color = Color.red;
                strangerFactionResolution.color = Color.red;

            }
        }

        private IEnumerator PersonMovement()
        {
            MovePersonInC = StartCoroutine(MovePersonIn());
            yield return new WaitForSeconds(0.9f);
            
            activatePopup();

        }

        public IEnumerator MovePersonIn(float countdownValue = 1.5f)
        {
            Vector3 pos = new Vector3(6.56f, 0.08f, 0f);
            while (countdownValue > 0 && instantiatedPerson.transform.position.x > pos.x)
            {
                try
                {
                    if (instantiatedPerson.position != pos)
                    {
                        if (Player.HasVehicle)
                        {
                            Vector3 newPos = Vector3.MoveTowards(instantiatedPerson.transform.position, pos, 0.12f);
                            instantiatedPerson.transform.position = newPos;
                        }
                        else
                        {
                            Vector3 newPos = Vector3.MoveTowards(instantiatedPerson.transform.position, pos, 0.09f);
                            instantiatedPerson.transform.position = newPos;
                        }
                    }
                    StopEnemyAnimation();
                }
                catch (Exception) { }

                yield return new WaitForSeconds(.01f);
                countdownValue -= 0.01f;
            }
            StopCoroutine(MovePersonInC);
            Parallex.ShouldMove = false;
            StopHeroAnimation();

        }

        public IEnumerator MovePersonOut(float countdownValue = 5f)
        {
            MoveEnemyAnimation();
            Vector3 pos = new Vector3(-15f, 0.059f, -10.25f);
            while (countdownValue > 0)
            {
                try
                {
                    if (instantiatedPerson.position != pos)
                    {

                        if (Player.HasVehicle)
                        {

                            Vector3 newPos = Vector3.MoveTowards(instantiatedPerson.transform.position, pos, 0.12f);
                            instantiatedPerson.transform.position = newPos;
                        }
                        else
                        {
                            Vector3 newPos = Vector3.MoveTowards(instantiatedPerson.transform.position, pos, 0.09f);
                            instantiatedPerson.transform.position = newPos;
                        }

                    }
                    else
                    {
                        countdownValue = 0;
                    }
                }
                catch (Exception)
                {
                    StopCoroutine(MovePersonOutC);
                }

                yield return new WaitForSeconds(.01f);
                countdownValue -= 0.01f;
            }
            StopCoroutine(MovePersonOutC);

        }

        private void MoveHeroAimation()
        {
            if (Player.HasVehicle)
                EncounterSystem.InstantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "run";
            else
                EncounterSystem.InstantiatedHeroes[0].GetComponent<Animator>().SetBool("Running", true);
        }



        private void StopHeroAnimation()
        {

            if (Player.HasVehicle)
                EncounterSystem.InstantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
            else
                EncounterSystem.InstantiatedHeroes[0].GetComponent<Animator>().SetBool("Running", false);
        }
        private void MoveEnemyAnimation()
        {
            instantiatedPerson.GetComponent<Animator>().SetBool("Running", true);
        }

        private void StopEnemyAnimation()
        {
            instantiatedPerson.GetComponent<Animator>().SetBool("Running", false);
        }

        private IEnumerator moveOnCoroutine()
        {
            PersonEncounterFin.SetActive(false);
            warriorEncounterText.SetActive(false);
            rangerEncounterText.SetActive(false);
            mageEncounterText.SetActive(false);
            WarriorPopUp.SetActive(false);
            ResolutionAidPopup.SetActive(false);
            mageLieButt.SetActive(false);
            mageRobbButt.SetActive(false);
            mageTruthButt.SetActive(false);
            rangerRobButt.SetActive(false);
            rangerBuyButt.SetActive(false);
            rangerIgnoreButt.SetActive(false);
            rangerResolutionBuyText.SetActive(false);
            mageResolutionAidText.SetActive(false);
            WarriorResolutionAidText.SetActive(false);
            Destroy(itemHolder1);
            Destroy(itemHolder2);
            Destroy(itemHolder3);
            // aidResItemName.text = "";
            goldAmount.text = "";
           // aidResItemAmount.text = "";
            MovePersonOutC = StartCoroutine(MovePersonOut());
            MoveHeroAimation();
            Parallex.ShouldMove = true;
            yield return new WaitForSeconds(5f);
            Destroy((GameObject)instantiatedPerson.gameObject);
            EncounterSystem.ContinueFunction = true;
        }


            public void moveOn()
        {
        
            StartCoroutine(moveOnCoroutine());
            
        }


    }
 }
