using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands

{
    public class CharacterSelect : MonoBehaviour
    {


         //add warrior class to party database
        public void addWarrior()
        {
            HeroPartyDB.addHero(new Warrior((Warrior)HeroClassDB.getHero(0)));            
            Debug.Log("Added Warrior");
            HeroPartyDB.SaveParty();
            
        }

        //add mage class to party database
        public void addMage()
        {
            HeroPartyDB.addHero(new Mage((Mage)HeroClassDB.getHero(1))); 
            Debug.Log("Added Mage");
            HeroPartyDB.SaveParty();        
        }

        // add ranger class to party database
        public void addRanger()
        {
            HeroPartyDB.addHero(new Ranger((Ranger)HeroClassDB.getHero(2))); 
            HeroPartyDB.SaveParty();
            Debug.Log("Added Ranger");
        }
        

        
    }
}
