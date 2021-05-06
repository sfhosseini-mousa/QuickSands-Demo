using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands
{
    public class BattleHUD : MonoBehaviour
    {

        public Slider hpSlider;
        public Gradient gradient;
        public Image fill;

        //Update the playerHUD to be the health of a solo hero
        public void SetHUDUnitHPHero(Hero hero)
        {
            hpSlider.maxValue = hero.MaxHP;
            hpSlider.value = hero.CurrentHP;

            Debug.Log(hpSlider.maxValue + "Mage Health");
            

            fill.color = gradient.Evaluate(1f);
        }

        //Update the enemyHUD to be the health of a solo enemy
        public void SetHUDUnitHPEnemy(Enemy enemy)
        {
            hpSlider.maxValue = enemy.MaxHP;
            hpSlider.value = enemy.CurrentHP;

            fill.color = gradient.Evaluate(1f);
        }


        //Update the playerHUD to be the health of Vehicle SumHP
         public void SetHUDVehicleHP(Vehicle vehicle)
        {
            hpSlider.maxValue = vehicle.SumHP;
            hpSlider.value = vehicle.CurrentHP;

            Debug.Log(hpSlider.maxValue + "Vehicle Health");

            fill.color = gradient.Evaluate(1f);
        }

        public void SetHP(int hp)
        {
            hpSlider.value = hp;

            fill.color = gradient.Evaluate(hpSlider.normalizedValue);
        }

    }

}
