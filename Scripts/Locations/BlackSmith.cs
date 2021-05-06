using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

namespace Sands
{
    public class BlackSmith : MonoBehaviour
    {
        private GameObject[] heroPrefabs = new GameObject[5];
        private GameObject[] upgradePrefabs = new GameObject[5];
        private Transform[] instantiatedHeroes = new Transform[5];
        [SerializeField] private Transform[] heroStations = new Transform[5];
        [SerializeField] private Transform currentHeroStation;
        [SerializeField] private Transform upgradeHeroStation;
        [SerializeField] private GameObject[] heroButtons = new GameObject[5];
        private Transform instantiatedCurrentHero;
        private Transform instantiatedUpgradeHero;
        private int selectedIndex;
        private int price;
        [SerializeField] private Text money;
        [SerializeField] private Text currentHeroHP;
        [SerializeField] private Text currentHeroAttack;
        [SerializeField] private Text upgradeHeroHP;
        [SerializeField] private Text upgradeHeroAttack;
        [SerializeField] private Text upgradeHeroPrice;
        [SerializeField] private GameObject[] upgradeStats = new GameObject[7];
        [SerializeField] private Text errorText;
        void Start()
        {
            foreach (var item in heroButtons)
            {
                item.SetActive(false);
            }

            InstantiateHeroes();
            money.text = System.Convert.ToString(PlayerInventory.Money);
        }

        public void InstantiateHeroes()
        {
            for (int i = 0; i < heroPrefabs.Length && i < HeroPartyDB.getHeroList().Count; i++)
            {
                heroButtons[i].SetActive(true);
                heroPrefabs[i] = (GameObject)Resources.Load(HeroPartyDB.getHero(i).GetType().Name, typeof(GameObject));
                upgradePrefabs[i] = (GameObject)Resources.Load(HeroPartyDB.getHero(i).GetType().Name, typeof(GameObject));
                HeroPartyDB.getHero(i).setSkin(heroPrefabs[i]);
                instantiatedHeroes[i] = Instantiate(heroPrefabs[i].transform, heroStations[i].position, Quaternion.identity);
                instantiatedHeroes[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
            InstantiateCurrentHero(0);
            InstantiateUpgradeHero(0);
            SetUpgradeHeroStats(0);
            SetCurrentHeroStats(0);
        }


        public void ReInstantiateHeroes()
        {
            for (int i = 0; i < heroPrefabs.Length && i < HeroPartyDB.getHeroList().Count; i++)
            {
                Destroy(instantiatedHeroes[i].gameObject);
            }

            for (int i = 0; i < heroPrefabs.Length && i < HeroPartyDB.getHeroList().Count; i++)
            {
                heroButtons[i].SetActive(true);
                heroPrefabs[i] = (GameObject)Resources.Load(HeroPartyDB.getHero(i).GetType().Name, typeof(GameObject));
                upgradePrefabs[i] = (GameObject)Resources.Load(HeroPartyDB.getHero(i).GetType().Name, typeof(GameObject));
                HeroPartyDB.getHero(i).setSkin(heroPrefabs[i]);
                instantiatedHeroes[i] = Instantiate(heroPrefabs[i].transform, heroStations[i].position, Quaternion.identity);
                instantiatedHeroes[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
           
        }



        public void HeroOnClick()
        {
            errorText.text = "";
            Destroy(instantiatedCurrentHero.gameObject);
            if (instantiatedUpgradeHero != null)
                Destroy(instantiatedUpgradeHero.gameObject);

            selectedIndex = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 1;
            HeroPartyDB.getHero(selectedIndex).setSkin(heroPrefabs[selectedIndex]);
            InstantiateCurrentHero(selectedIndex);
            InstantiateUpgradeHero(selectedIndex);
            SetUpgradeHeroStats(selectedIndex);
            SetCurrentHeroStats(selectedIndex);
        }

        public void InstantiateCurrentHero(int index)
        {
            HeroPartyDB.getHero(index).setSkin(heroPrefabs[index]);
            instantiatedCurrentHero = Instantiate(heroPrefabs[index].transform, currentHeroStation.position, Quaternion.identity);
            instantiatedCurrentHero.transform.localScale = new Vector3(2, 2, 2);
        }

        public void InstantiateUpgradeHero(int index)
        {
            bool instantiate = true;
            var skeletonMecanim = upgradePrefabs[index].GetComponent<SkeletonMecanim>();

            if(skeletonMecanim.initialSkinName == "p6")
                instantiate = false;

            switch (skeletonMecanim.initialSkinName)
            {
                case "p2":
                    skeletonMecanim.initialSkinName = "p3";
                    price = 1500;
                    break;
                case "p3":
                    skeletonMecanim.initialSkinName = "p4";
                    price = 3000;
                    break;
                case "p4":
                    skeletonMecanim.initialSkinName = "p5";
                    price = 5000;
                    break;
                case "p5":
                    skeletonMecanim.initialSkinName = "p6";
                    price = 7000;
                    break;
                case "p6":
                    skeletonMecanim.initialSkinName = "p6";
                    break;
                default:
                    break;
            }

            
            if (instantiate)
            {
                foreach (var item in upgradeStats)
                {
                    item.SetActive(true);
                }

                instantiatedUpgradeHero = Instantiate(upgradePrefabs[index].transform, upgradeHeroStation.position, Quaternion.identity);
                instantiatedUpgradeHero.transform.localScale = new Vector3(2, 2, 2);
            }
            else{
                foreach (var item in upgradeStats)
                {
                    item.SetActive(false);
                }
            }
        }


        public void SetCurrentHeroStats(int index)
        {
            int health = 0;
            int damage = 0;
            if (HeroPartyDB.getHero(index).SkinTire < 5)
            {
                if (instantiatedCurrentHero.gameObject.name == "Warrior(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire - 1).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire -1 ).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedCurrentHero.gameObject.name == "Mage(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire + 4).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire + 4).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedCurrentHero.gameObject.name == "Ranger(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire + 9).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire + 9).Damage + HeroPartyDB.getHero(index).Damage;
                }
            }
            else{
                if (instantiatedCurrentHero.gameObject.name == "Warrior(Clone)")
                {
                    health += ArmorDatabase.getArmor(4).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(4).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedCurrentHero.gameObject.name == "Mage(Clone)")
                {
                    health += ArmorDatabase.getArmor(9).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(9).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedCurrentHero.gameObject.name == "Ranger(Clone)")
                {
                    health += ArmorDatabase.getArmor(14).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(14).Damage + HeroPartyDB.getHero(index).Damage;
                }
            }

            currentHeroHP.text = System.Convert.ToString(health);
            currentHeroAttack.text = System.Convert.ToString(damage);
        }

        public void SetUpgradeHeroStats(int index)
        {
            int health = 0;
            int damage = 0;
            if (HeroPartyDB.getHero(index).SkinTire < 5 && instantiatedUpgradeHero != null)
            {
                if (instantiatedUpgradeHero.gameObject.name == "Warrior(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedUpgradeHero.gameObject.name == "Mage(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire + 5).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire + 5).Damage + HeroPartyDB.getHero(index).Damage;
                }
                else if (instantiatedUpgradeHero.gameObject.name == "Ranger(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire + 10).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire + 10).Damage + HeroPartyDB.getHero(index).Damage;
                }
            }
            upgradeHeroHP.text = System.Convert.ToString(health);

            upgradeHeroAttack.text = System.Convert.ToString(damage);

            upgradeHeroPrice.text = System.Convert.ToString(price);
        }




        public void upgradeBtnOnClick()
        {
            if (PlayerInventory.Money >= price && HeroPartyDB.getHero(selectedIndex).SkinTire < 5)
            {
                PlayerInventory.Money -= price;
                money.text = System.Convert.ToString(PlayerInventory.Money);
                PlayerInventory.SavePlayerInventory();

                Destroy(instantiatedCurrentHero.gameObject);
                if (instantiatedUpgradeHero != null) 
                Destroy(instantiatedUpgradeHero.gameObject);

                HeroPartyDB.getHero(selectedIndex).SkinTire++;
                HeroPartyDB.getHero(selectedIndex).setSkin(heroPrefabs[selectedIndex]);
                InstantiateCurrentHero(selectedIndex);
                InstantiateUpgradeHero(selectedIndex);
                ReInstantiateHeroes();

                HeroPartyDB.SaveParty();
                SetCurrentHeroStats(selectedIndex);
                SetUpgradeHeroStats(selectedIndex);

                
            }

            else if(HeroPartyDB.getHero(selectedIndex).SkinTire == 5){
                errorText.text = "Already Maxed Out";
            }
            else{
                errorText.text = "Not Enough Gold";
            }
        }

    }
}