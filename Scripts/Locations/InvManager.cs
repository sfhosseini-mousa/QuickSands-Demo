
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;


namespace Sands
{
    public class InvManager : MonoBehaviour
    {
        private GameObject[] heroPrefabs = new GameObject[5];

        private Transform instantiatedCurrentHero;
        private Vehicle playerVehicle;
        private Transform[] instantiatedHeroes = new Transform[5];
        private Transform instantiatedHeroVehicle;
        [SerializeField] private Transform HeroVehicleBS;


        [SerializeField] private Transform[] heroStations = new Transform[5];
        [SerializeField] private GameObject[] heroButtons = new GameObject[5];
        [SerializeField] private Transform currentHeroStation;
        [SerializeField] private Text currentHeroHP;
        [SerializeField] private Text currentHeroAttack;
        private int selectedIndex;
        private GameObject heroVehiclePrefab;
        [SerializeField] private GameObject fireButton;
        [SerializeField] private GameObject firePopUp;
        [SerializeField] private Text vehicleName;
        [SerializeField] private Text heroLevel;
        [SerializeField] private Text partyHealth;
        [SerializeField] private Text partyDamage;
        private int pHealth = 0;
        private int pDamage = 0;
        int health = 0;
        int damage = 0;

        // Start is called before the first frame update
        void Start()
        {
            if (Player.HasVehicle)
                vehicleName.text = Player.CurrentVehicle.Name;
            else
                vehicleName.gameObject.SetActive(false);

            foreach (var item in heroButtons)
            {
                item.SetActive(false);
            }

            //IF PLAYER HAS VEHICLE
            if (SaveSystem.Pdata.HasVehicle)
            {

                Player.LoadPlayer();
                playerVehicle = Player.CurrentVehicle;

                switch (playerVehicle.Name)
                {
                    case "Scout":
                        heroVehiclePrefab = (GameObject)Resources.Load("Scout2Vehicle", typeof(GameObject));
                        instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                        instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                        instantiatedHeroVehicle.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                        heroStations[0].transform.position = new Vector3(3.75f, 9.4f, 0f);
                        heroStations[1].transform.position = new Vector3(-1.53f, 9.4f, 0f);

                        heroButtons[0].transform.position = new Vector3(3.75f, 12.4f, 0f);
                        heroButtons[1].transform.position = new Vector3(-1.53f, 12.4f, 0f);



                        break;
                    case "Warthog":
                        heroVehiclePrefab = (GameObject)Resources.Load("Warthog3Vehicle", typeof(GameObject));
                        instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                        instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                        instantiatedHeroVehicle.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                        heroStations[0].transform.position = new Vector3(1.89f, 9.59f, 0f);
                        heroStations[1].transform.position = new Vector3(-3.5f, 9.59f, 0f);
                        heroStations[2].transform.position = new Vector3(8.08f, 12.81f, 0f);

                        heroButtons[0].transform.position = new Vector3(1.89f, 12.59f, 0f);
                        heroButtons[1].transform.position = new Vector3(-3.5f, 12.59f, 0f);
                        heroButtons[2].transform.position = new Vector3(8.08f, 15.81f, 0f);
                        break;
                    case "Goliath":
                        heroVehiclePrefab = (GameObject)Resources.Load("Goliath4Vehicle", typeof(GameObject));
                        instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                        instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                        instantiatedHeroVehicle.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                        heroStations[0].transform.position = new Vector3(8.5f, 12.42f, 0f);
                        heroStations[1].transform.position = new Vector3(3.83f, 12.42f, 0f);
                        heroStations[2].transform.position = new Vector3(-0.5f, 14.07f, 0f);
                        heroStations[3].transform.position = new Vector3(-4.890064f, 14.07f, 0f);

                        heroButtons[0].transform.position = new Vector3(8.6f, 15.42f, 0f);
                        heroButtons[1].transform.position = new Vector3(3.83f, 15.42f, 0f);
                        heroButtons[2].transform.position = new Vector3(-0.5f, 17.07f, 0f);
                        heroButtons[3].transform.position = new Vector3(-4.890064f, 17.07f, 0f);
                        break;
                    case "Leviathan":
                        heroVehiclePrefab = (GameObject)Resources.Load("Leviathan5Vehicle", typeof(GameObject));
                        instantiatedHeroVehicle = Instantiate(heroVehiclePrefab.transform, HeroVehicleBS.position, Quaternion.identity);
                        instantiatedHeroVehicle.GetComponent<SkeletonAnimation>().AnimationName = "idle";
                        instantiatedHeroVehicle.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                        heroStations[0].transform.position = new Vector3(2.5f, 13.24f, 0f);
                        heroStations[1].transform.position = new Vector3(7.08f, 9.03f, 0f);
                        heroStations[2].transform.position = new Vector3(12.46f, 9.03f, 0f);
                        heroStations[3].transform.position = new Vector3(-1.2f, 13.24f, 0f);
                        heroStations[4].transform.position = new Vector3(-5.6f, 13.24f, 0f);

                        heroButtons[0].transform.position = new Vector3(2.5f, 16.24f, 0f);
                        heroButtons[1].transform.position = new Vector3(7.08f, 12.3f, 0f);
                        heroButtons[2].transform.position = new Vector3(12.46f, 12.3f, 0f);
                        heroButtons[3].transform.position = new Vector3(-1.6f, 16.24f, 0f);
                        heroButtons[4].transform.position = new Vector3(-5.6f, 16.24f, 0f);
                        break;

                    default:
                        break;
                }

                for (int i = 0; i < HeroPartyDB.getHeroList().Count; i++)
                {
                    heroButtons[i].SetActive(true);
                }

                if (HeroPartyDB.getHeroList().Count == 1)
                    fireButton.GetComponent<Selectable>().interactable = false;

                InstantiateHeroes();

            }
            else {

                fireButton.GetComponent<Selectable>().interactable = false;

                heroStations[0].transform.position = new Vector3(5f, 2.76f, 0f);
                heroButtons[0].transform.position = new Vector3(5f, 5.76f, 0f);
                heroPrefabs = new GameObject[1];


                heroPrefabs[0] = (GameObject)Resources.Load(HeroPartyDB.getHero(0).GetType().Name, typeof(GameObject));
                HeroPartyDB.getHero(0).setSkin(heroPrefabs[0]);
                instantiatedHeroes = new Transform[1];

                instantiatedHeroes[0] = Instantiate(heroPrefabs[0].transform, heroStations[0].position, Quaternion.identity);
                instantiatedHeroes[0].transform.localScale = new Vector3(5.0f, 5.0f, 0.0f);

                InstantiateCurrentHero(0);
                SetCurrentHeroStats(0);
            }

            partyStats();
        }

        public void InstantiateHeroes()
        {
            for (int i = 0; i < heroPrefabs.Length && i < HeroPartyDB.getHeroList().Count; i++)
            {
                heroButtons[i].SetActive(true);
                heroPrefabs[i] = (GameObject)Resources.Load(HeroPartyDB.getHero(i).GetType().Name, typeof(GameObject));

                HeroPartyDB.getHero(i).setSkin(heroPrefabs[i]);
                instantiatedHeroes[i] = Instantiate(heroPrefabs[i].transform, heroStations[i].position, Quaternion.identity);
                instantiatedHeroes[i].transform.localScale = new Vector3(5.0f, 5.0f, 0.0f);
            }
            InstantiateCurrentHero(0);


            SetCurrentHeroStats(0);
        }

        public void InstantiateCurrentHero(int index)
        {
            HeroPartyDB.getHero(index).setSkin(heroPrefabs[index]);
            instantiatedCurrentHero = Instantiate(heroPrefabs[index].transform, currentHeroStation.position, Quaternion.identity);
            instantiatedCurrentHero.transform.localScale = new Vector3(7.0f, 7.0f, 0.0f);
            heroLevel.text = System.Convert.ToString(HeroPartyDB.getHero(index).SkinTire);
        }


        public void ReInstantiateHeroes()
        {
            foreach (var item in heroButtons)
            {
                item.SetActive(false);
            }

            for (int i = 0; i < HeroPartyDB.getHeroList().Count; i++)
            {
                heroButtons[i].SetActive(true);
            }

            if (HeroPartyDB.getHeroList().Count == 1)
                fireButton.GetComponent<Selectable>().interactable = false;

            for (int i = 0; i < HeroPartyDB.getHeroList().Count + 1; i++)
            {
                Destroy(instantiatedHeroes[i].gameObject);
            }

            for (int i = 0; i < heroPrefabs.Length && i < HeroPartyDB.getHeroList().Count; i++)
            {
                heroButtons[i].SetActive(true);
                heroPrefabs[i] = (GameObject)Resources.Load(HeroPartyDB.getHero(i).GetType().Name, typeof(GameObject));

                HeroPartyDB.getHero(i).setSkin(heroPrefabs[i]);
                instantiatedHeroes[i] = Instantiate(heroPrefabs[i].transform, heroStations[i].position, Quaternion.identity);
                instantiatedHeroes[i].transform.localScale = new Vector3(5.0f, 5.0f, 0.0f);
            }
            Destroy(instantiatedCurrentHero.gameObject);
            InstantiateCurrentHero(0);


            SetCurrentHeroStats(0);

        }



        public void HeroOnClick()
        {

            Destroy(instantiatedCurrentHero.gameObject);


            selectedIndex = System.Convert.ToInt32(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name) - 1;

            InstantiateCurrentHero(selectedIndex);


            SetCurrentHeroStats(selectedIndex);
        }

        public void SetCurrentHeroStats(int index)
        {
            health = 0;
            damage = 0;

            if (HeroPartyDB.getHero(index).SkinTire < 5)
            {
                if (instantiatedCurrentHero.gameObject.name == "Warrior(Clone)")
                {
                    health += ArmorDatabase.getArmor(HeroPartyDB.getHero(index).SkinTire - 1).Health + HeroPartyDB.getHero(index).MaxHP;
                    damage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(index).SkinTire).Damage + HeroPartyDB.getHero(index).Damage;
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
            else
            {
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


        public void partyStats()
        {
            pHealth = 0;
            pDamage = 0;

            if (Player.HasVehicle)
            {
                for (int i = 0; i < HeroPartyDB.getHeroList().Count; i++)
                {
                    if (HeroPartyDB.getHero(i).SkinTire < 5)
                    {
                        if (instantiatedCurrentHero.gameObject.name == "Warrior(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(HeroPartyDB.getHero(i).SkinTire - 1).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(i).SkinTire).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                        else if (instantiatedCurrentHero.gameObject.name == "Mage(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(HeroPartyDB.getHero(i).SkinTire + 4).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(i).SkinTire + 4).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                        else if (instantiatedCurrentHero.gameObject.name == "Ranger(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(HeroPartyDB.getHero(i).SkinTire + 9).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(i).SkinTire + 9).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                    }
                    else
                    {
                        if (instantiatedCurrentHero.gameObject.name == "Warrior(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(4).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(4).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                        else if (instantiatedCurrentHero.gameObject.name == "Mage(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(9).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(9).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                        else if (instantiatedCurrentHero.gameObject.name == "Ranger(Clone)")
                        {
                            pHealth += ArmorDatabase.getArmor(14).Health + HeroPartyDB.getHero(i).MaxHP;
                            pDamage += WeaponDatabase.getWeapon(14).Damage + HeroPartyDB.getHero(i).Damage;
                        }
                    }
                }
            }
            else
            {
                pHealth = health;
                pDamage = damage;
                //if (HeroPartyDB.getHero(0).SkinTire < 5)
                //{
                //    if (instantiatedCurrentHero.gameObject.name == "Warrior(Clone)")
                //    {
                //        pHealth += ArmorDatabase.getArmor(HeroPartyDB.getHero(0).SkinTire - 1).Health + HeroPartyDB.getHero(0).MaxHP;
                //        pDamage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(0).SkinTire).Damage + HeroPartyDB.getHero(0).Damage;
                //    }
                //    else if (instantiatedCurrentHero.gameObject.name == "Mage(Clone)")
                //    {
                //        pHealth += ArmorDatabase.getArmor(HeroPartyDB.getHero(0).SkinTire + 4).Health + HeroPartyDB.getHero(0).MaxHP;
                //        pDamage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(0).SkinTire + 4).Damage + HeroPartyDB.getHero(0).Damage;
                //    }
                //    else if (instantiatedCurrentHero.gameObject.name == "Ranger(Clone)")
                //    {
                //        pHealth += ArmorDatabase.getArmor(HeroPartyDB.getHero(0).SkinTire + 9).Health + HeroPartyDB.getHero(0).MaxHP;
                //        pDamage += WeaponDatabase.getWeapon(HeroPartyDB.getHero(0).SkinTire + 9).Damage + HeroPartyDB.getHero(0).Damage;
                //    }
                //}
                //else
                //{
                //    if (instantiatedCurrentHero.gameObject.name == "Warrior(Clone)")
                //    {
                //        pHealth += ArmorDatabase.getArmor(4).Health + HeroPartyDB.getHero(0).MaxHP;
                //        pDamage += WeaponDatabase.getWeapon(4).Damage + HeroPartyDB.getHero(0).Damage;
                //    }
                //    else if (instantiatedCurrentHero.gameObject.name == "Mage(Clone)")
                //    {
                //        pHealth += ArmorDatabase.getArmor(9).Health + HeroPartyDB.getHero(0).MaxHP;
                //        pDamage += WeaponDatabase.getWeapon(9).Damage + HeroPartyDB.getHero(0).Damage;
                //    }
                //    else if (instantiatedCurrentHero.gameObject.name == "Ranger(Clone)")
                //    {
                //        pHealth += ArmorDatabase.getArmor(14).Health + HeroPartyDB.getHero(0).MaxHP;
                //        pDamage += WeaponDatabase.getWeapon(14).Damage + HeroPartyDB.getHero(0).Damage;
                //    }
                //}
            }
            if (Player.HasVehicle) 
            pHealth += Player.CurrentVehicle.VehicleHP;


            partyDamage.text = pDamage.ToString();
            partyHealth.text = pHealth.ToString();
        }

        public void FireOnClick() {
            firePopUp.SetActive(true);
        }

        public void YesOnClick()
        {
            HeroPartyDB.getHeroList().RemoveAt(selectedIndex);
            HeroPartyDB.SaveParty();
            heroButtons[selectedIndex].SetActive(false);
            ReInstantiateHeroes();
            partyStats();

        }
    }
}
