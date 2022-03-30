using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class HeroMemento
    {
        private int id;
        private int damage;
        private int critChance;
        private int maxHP;
        private int currentHP;
        private int capacity;
        private int skinTire;
        private bool isQuestHero;

        //Copy comstructor
        public HeroMemento(Hero hero)
        {
            this.id = hero.Id;
            this.damage = hero.Damage;
            this.critChance = hero.CritChance;
            this.maxHP = hero.MaxHP;
            this.currentHP = hero.CurrentHP;
            this.capacity = hero.Capacity;
            this.skinTire = hero.SkinTire;
            this.isQuestHero = hero.IsQuestHero;
        }

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public int Damage {
            get {
                return damage;
            }
            set {
                damage = value;
            }
        }

        public int CritChance {
            get {
                return critChance;
            }
            set {
                critChance = value;
            }
        }

        public int MaxHP {
            get {
                return maxHP;
            }
            set {
                maxHP = value;
            }
        }

        public int CurrentHP {
            get {
                return currentHP;
            }
            set {
                currentHP = value;
            }
        }

        public int Capacity {
            get {
                return capacity;
            }
            set {
                capacity = value;
            }
        }

        public int SkinTire{
            get {
                return skinTire;
            }
            set {
                skinTire = value;
            }
        }

        public bool IsQuestHero
        {
            get
            {
                return isQuestHero;
            }
            set
            {
                isQuestHero = value;
            }
        }
    }
}