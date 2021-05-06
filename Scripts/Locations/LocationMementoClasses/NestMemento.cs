using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    [System.Serializable]
    public class NestMemento : LocationMemento
    {
        private List<Enemy> enemies;
        private List<Enemy> wave1;
        private List<Enemy> wave2;
        private List<Enemy> wave3;
        private int multiplier;
        private bool activeStatus = false;

        public NestMemento(Nest nest) : base(nest){
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