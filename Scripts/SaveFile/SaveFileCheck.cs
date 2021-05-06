using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveFileCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(File.Exists(Application.persistentDataPath + "/player.savefile"))
            GameObject.FindGameObjectWithTag("continueBtn").SetActive(true);
        
        else
            GameObject.FindGameObjectWithTag("continueBtn").SetActive(false);
  }

    public void saveWarningTest()
    {

      if (!File.Exists(Application.persistentDataPath + "/player.savefile"))
      {
      
          GameObject.FindGameObjectWithTag("SaveWarning").SetActive(false);


      }
    }


}
