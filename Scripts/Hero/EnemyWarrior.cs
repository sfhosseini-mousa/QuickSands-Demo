﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

//Script sets the values of the unit and its animation
namespace Sands
{
    public class EnemyWarrior : Hero
    {
        public EnemyWarrior(int damage, int critChance, int maxHP, int currentHP, int capacity, int skinTire) : base(damage, critChance, maxHP, currentHP, capacity, skinTire) { }
        void Awake()
        {

            this.Damage = HeroClassDB.heroes[3].Damage;
            this.CritChance = HeroClassDB.heroes[3].CritChance;
            this.MaxHP = HeroClassDB.heroes[3].MaxHP;
            this.CurrentHP = HeroClassDB.heroes[3].CurrentHP;
            this.Capacity = HeroClassDB.heroes[3].Capacity;
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

        ////create a new skin class variable
                ////assign a skin to it
                ////add it's other attachments to the set
                //Skin enemySkin = new Skin("enemySkin");
                //Skin shadow = skeleton.skeleton.Data.FindSkin("e5");
                //enemySkin.AddAttachments(shadow);

                ////set the skin to the skeleton model
                ////attach everything
                //skeleton.skeleton.SetSkin(enemySkin);
                //skeleton.skeleton.SetSlotsToSetupPose();

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