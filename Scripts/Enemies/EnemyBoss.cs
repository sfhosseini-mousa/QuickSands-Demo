using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public class EnemyBoss : Enemy
    {


        public EnemyBoss(int id, int damage, int critChance, int maxHealth, int currentHealth) : base(id, damage, critChance, maxHealth, currentHealth) { }

        //what distinguishes bosses? Maybe an ability?
        /*public ability()
        {

        }*/

    }
}