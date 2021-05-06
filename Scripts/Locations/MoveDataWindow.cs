

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class MoveDataWindow : MonoBehaviour
{
    [SerializeField] private RectTransform TradeTipWindow;
    [SerializeField] private RectTransform MapWindow;
    [SerializeField] private Camera cam;
    private GameObject tradeTip;

    Vector3 position;

    private void Start(){

        StartCoroutine(Deactivate());
    }


    private void Update()
    {
      
    }

    IEnumerator Deactivate(){
        tradeTip = GameObject.FindGameObjectWithTag("tradeTip");
        yield return new WaitForSeconds(0.2f);
        tradeTip.SetActive(false);
    }

   

}

