using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sands {
    public static class HeroClassDB {

        public static List<Hero> heroes = new List<Hero>();

        static HeroClassDB() 
        {
            heroes = new List<Hero>() {
                //adding all 3 types off heroes to sample class
                // int id, int damage, int critChance, int maxHP, int currentHP, int capacity, skinTier
                new Warrior(1, 100, 10, 500, 500, 100, 1, false),
                new Mage( 2, 250, 15, 250, 250, 60, 1, false),
                new Ranger(3, 100, 33, 350, 350, 75, 1, false),
                new Wizard(4, 150, 10, 300, 300, 70, 1, false),
                new Spearman(5, 80, 20, 400, 400, 90, 1, false),
                
                new EnemyWarrior(6, 40, 10, 250, 250, 100, 1, false),
                new EnemyMage(7, 70, 15, 100, 100, 60, 1, false),
                new EnemyRanger(8, 40, 33, 150, 150, 75, 1, false),
                new EnemyWizard(9, 40, 33, 150, 150, 75, 1, false),
                new EnemySpearman(10, 40, 33, 150, 150, 75, 1, false)
            }; 
        }

        //get database
        public static List<Hero> getHeroList() {
            return heroes;
        }

        //get Hero at position
        public static Hero getHero(int position) {
          return heroes[position];
        }

        //clear
        public static void clearHeroList() {
            heroes.Clear();
        }
    }
}