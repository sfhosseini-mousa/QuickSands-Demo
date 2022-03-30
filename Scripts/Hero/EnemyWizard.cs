using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

//Script sets the values of the unit and its animation
namespace Sands
{
    public class EnemyWizard : Hero
    {
        public EnemyWizard(int id, int damage, int critChance, int maxHP, int currentHP, int capacity, int skinTire, bool isQuestHero) : base(id, damage, critChance, maxHP, currentHP, capacity, skinTire, isQuestHero) { }
        void Awake()
        {
            this.Id = HeroClassDB.heroes[8].Id;
            this.Damage = HeroClassDB.heroes[8].Damage;
            this.CritChance = HeroClassDB.heroes[8].CritChance;
            this.MaxHP = HeroClassDB.heroes[8].MaxHP + 200;
            this.CurrentHP = HeroClassDB.heroes[8].CurrentHP + 200;
            this.Capacity = HeroClassDB.heroes[8].Capacity;
            this.IsQuestHero = HeroClassDB.heroes[8].IsQuestHero;
        }

        public override bool TakeDamage(int dmg)
        {
            CurrentHP -= dmg;

            if (CurrentHP <= 0)
            {
                CurrentHP = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Heal(int healAmount)
        {
            CurrentHP += healAmount;
            if (CurrentHP > MaxHP)
                CurrentHP = MaxHP;
        }


        //sets the skin of the recieved prefab of the hero
        public override void setSkin(GameObject prefab)
        {
            var skeletonMecanim = prefab.GetComponent<SkeletonMecanim>();
            switch (SkinTire)
            {
                case 1:
                    skeletonMecanim.initialSkinName = "e2";
                    break;
                case 2:
                    skeletonMecanim.initialSkinName = "e3";
                    break;
                case 3:
                    skeletonMecanim.initialSkinName = "e4";
                    break;
                case 4:
                    skeletonMecanim.initialSkinName = "e5";
                    break;
                case 5:
                    skeletonMecanim.initialSkinName = "e6";
                    break;
                default:
                break;
            }
        }

        public int GetHealth()
        {
            return CurrentHP;
        }

    }
}