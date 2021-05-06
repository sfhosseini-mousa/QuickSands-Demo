using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public class Nest : Location
    {
        private List<Enemy> enemies;
        private List<Enemy> wave1;
        private List<Enemy> wave2;
        private List<Enemy> wave3;
        private int multiplier;
        private bool activeStatus = false;
        public Nest(){}
        
        public Nest(int id, string name, double lattitude, double longtitude, int territory, int[] nearbyTowns, int multiplier, bool activeStatus) : base(id, name, lattitude, longtitude, territory, nearbyTowns)
        {   
            enemies = EnemyClassDB.getEnemyList();
            
            foreach (Enemy enemy in enemies)
            {
                enemy.Damage *= multiplier;
                enemy.MaxHP *= multiplier;
                enemy.CurrentHP *= multiplier;
            }

            wave1 = new List<Enemy>(){
                enemies[0],
                enemies[0],
                enemies[1]
            };
            wave2 = new List<Enemy>(){
                enemies[0],
                enemies[0],
                enemies[0],
                enemies[1],
                enemies[1]
            };
             wave3 = new List<Enemy>(){
                enemies[2]
            };
        }

        public Nest(NestMemento nest) : base(nest){
            this.enemies = nest.Enemies;
            this.wave1 = nest.Wave1;
            this.wave2 = nest.Wave2;
            this.wave3 = nest.Wave3;
            this.multiplier = nest.Multiplier;
            this.activeStatus = nest.ActiveStatus;
        }

        public int Multiplier
        {
            get{ return multiplier; }
            set{ multiplier = value; }
        }
        public bool ActiveStatus
        {
            get{ return activeStatus; }
            set{ activeStatus = value; }
        }
        public List<Enemy> Enemies
        {
            get{ return enemies; }
            set{ enemies = value; }
        }
        public List<Enemy> Wave1
        {
            get{ return wave1; }
            set{ wave1 = value; }
        }
        public List<Enemy> Wave2
        {
            get{ return wave2; }
            set{ wave2 = value; }
        }
        public List<Enemy> Wave3
        {
            get{ return wave3; }
            set{ wave3 = value; }
        }
    }
}