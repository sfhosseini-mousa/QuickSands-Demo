using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sands{
    public static class InnHeroes
    {
        private static List<Hero> innHeroesList;
        static InnHeroes(){
            innHeroesList = new List<Hero>();
        }

        public static void GenerateHeroes(){
            innHeroesList = new List<Hero>();
            int len = UnityEngine.Random.Range(1, 4);
            for (int i = 0; i < len; i++)
            {
                int randomHero = UnityEngine.Random.Range(0, 3);
                if(randomHero == 0)
                    InnHeroesList.Add(new Warrior((Warrior)HeroClassDB.getHero(randomHero)));
                else if(randomHero == 1)
                    InnHeroesList.Add(new Mage((Mage)HeroClassDB.getHero(randomHero)));
                else if(randomHero == 2)
                    InnHeroesList.Add(new Ranger((Ranger)HeroClassDB.getHero(randomHero)));

                InnHeroesList[i].SkinTire = UnityEngine.Random.Range(1, 6);

                if (len == 1)
                    InnHeroesList[i].SkinTire = 1;
            }
        }

        public static List<Hero> InnHeroesList{
            get{
                return innHeroesList;
            }
            set{
                innHeroesList = value;
            }
        }
    }
}