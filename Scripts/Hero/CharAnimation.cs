using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System;

namespace Sands {
    public class CharAnimation : MonoBehaviour {


        public Animator anim;

        // Start is called before the first frame update
        void Start() {
            anim = GetComponent<Animator>();
        
        }
        
        public void AttackAnim() {
            
            anim.SetTrigger("Attack");
        }

        public void HealAnim() {

            anim.SetTrigger("Skill");
        }

        public void RunningAnim() {

            anim.SetBool("Running", true);
        }
        
        public void IdleAnim() {
            
            anim.SetBool("Running", false);
        }



    }
}