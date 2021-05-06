using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sands
{
    public class EnemyFlying : Enemy
    {
        public EnemyFlying(int id, int damage, int critChance, int maxHealth, int currentHealth) : base(id, damage, critChance, maxHealth, currentHealth) { }

    }
}