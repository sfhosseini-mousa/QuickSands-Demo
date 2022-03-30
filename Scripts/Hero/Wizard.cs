using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace Sands
{
    public class Wizard : Hero
    {
        public Wizard(int id, int damage, int critChance, int maxHP, int currentHP, int capacity, int skinTire, bool isQuestHero) : base(id, damage, critChance, maxHP, currentHP, capacity, skinTire, isQuestHero) { }
        void Awake()
        {
            this.Id = HeroClassDB.heroes[3].Id;
            this.Damage = HeroClassDB.heroes[3].Damage;
            this.CritChance = HeroClassDB.heroes[3].CritChance;
            this.MaxHP = HeroClassDB.heroes[3].MaxHP;
            this.CurrentHP = HeroClassDB.heroes[3].CurrentHP;
            this.Capacity = HeroClassDB.heroes[3].Capacity;
            this.IsQuestHero = HeroClassDB.heroes[3].IsQuestHero;
        }

        //memento copy constructor
        public Wizard(WizardMemento wizardMemento) : base(wizardMemento) { }

        //copy constructor
        public Wizard(Wizard wizard) : base(wizard) { }

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
                    skeletonMecanim.initialSkinName = "p2";
                    break;
                case 2:
                    skeletonMecanim.initialSkinName = "p3";
                    break;
                case 3:
                    skeletonMecanim.initialSkinName = "p4";
                    break;
                case 4:
                    skeletonMecanim.initialSkinName = "p5";
                    break;
                case 5:
                    skeletonMecanim.initialSkinName = "p6";
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
