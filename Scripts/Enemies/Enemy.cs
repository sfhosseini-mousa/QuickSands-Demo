using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public class Enemy : MonoBehaviour
    {
        private int id;
        private int damage;
        private int critChance;
        private int maxHP;
        private int currentHP;

        public Enemy(int id, int damage, int critChance, int maxHP, int currentHP) {
            this.id = id;
            this.damage = damage;
            this.critChance = critChance;
            this.maxHP = maxHP;
            this.currentHP = currentHP;
        }

        /////////// GETTERS AND SETTERS //////////

        public int Damage
        {
            get
            {
                return damage;
            }
            set
            {
                damage = value;
            }
        }

        public int CritDamage
            {
                get
                {
                    return critChance;
                }
                set
                {
                    critChance = value;
                }
        }


        public int MaxHP
        {
            get
            {
                return maxHP;
            }
            set
            {
                maxHP = value;
            }
        }

        public int CurrentHP
        {
            get
            {
                return  currentHP;
            }
            set
            {
                currentHP = value;
            }
        }
        
    }
}