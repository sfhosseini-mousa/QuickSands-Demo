using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneChange : MonoBehaviour
{
  public void NewGame()
  {
    File.Delete(Application.persistentDataPath + "/player.savefile");
    Sands.SaveSystem.Pdata = new Sands.PlayerData();
    Sands.SaveSystem.LoadAll();
    Sands.SaveSystem.SaveAll();
    SceneManager.LoadScene("CharSelect");
  }

  public void NewGame2()
    {
      if(!File.Exists(Application.persistentDataPath + "/player.savefile"))
          SceneManager.LoadScene("CharSelect");
    }

  public void warning()
    {
        SceneManager.LoadScene("SaveWarning");
    }

    public void QuitGame() {
        Sands.SaveSystem.SaveAll();
        Application.Quit();
    }

    public void characterSelected() {

        SceneManager.LoadScene("Town");
    }

    public void BlackSmithSelected()
    {

        SceneManager.LoadScene("BlackSmith");
    }

    public void TradeGoodsSelected()
    {

        SceneManager.LoadScene("TradeGoods");
    }

    public void InnSelected()
    {

        SceneManager.LoadScene("Inn");
    }

    public void QuestBoardSelected()
    {

        SceneManager.LoadScene("QuestBoard");
    }

    public void ShipShopSelected()
    {

        SceneManager.LoadScene("ShipShop");
    }

    public void InventorySelected()
    {

        SceneManager.LoadScene("InvManager");
    }

    public void MapSelected()
    {
        SceneManager.LoadScene("Map");
    }

    public void MapCloseSelected()
    {
        SceneManager.LoadScene("Town");
    }

    public void ReturnToTownSelected()
    {
        SceneManager.LoadScene("Town");
    }

    public void MainMenuSelected()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
