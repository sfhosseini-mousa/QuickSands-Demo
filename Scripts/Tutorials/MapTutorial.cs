using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands
{
    public class MapTutorial : MonoBehaviour
    {
        Button[] buttons;

        GameObject instantiatedHeroPopUp;

        [SerializeField] GameObject[] tutorialPositions;
        [SerializeField] HeroPopUpCreator heroPopUpCreator;

        ObjectFader objectFader = new ObjectFader();
        ButtonDeactivator buttonDeactivator = new ButtonDeactivator();

        [SerializeField] GameObject westrayButton;

        [SerializeField] HandPointerCreator handPointerCreator;
        [SerializeField] Transform[] HandPositions;

        void Start()
        {
            buttons = GameObject.FindObjectsOfType<Button>();
            if (TutorialManager.FifthTimeGoingFromMapToTradeGoods)
            {
                LocationDB.getLocation(Player.CurrentLocation.Id - 1).TradePrices[0] = 10; 
                LocationDB.getLocation(1).TradePrices[0] = 30;
                TapFirstTownTutorial();
            }
            else if (TutorialManager.TutorialNotDone)
            {
                TutorialManager.SixthTimeGoingFromTradeGoodsToMap = false;
                TutorialManager.TutorialNotDone = false;
                TapFirstTownToGoTutorial();
            }
        }

        void TapFirstTownTutorial()
        {
            heroPopUpCreator.CreateHeroPopUp(ref instantiatedHeroPopUp, "You can only travel to the Towns next to you.\nChoose Westray.", tutorialPositions[0].transform, 2);

            buttonDeactivator.DeactivateAllButtonsExcept("Westray", buttons);

            handPointerCreator.InstantiateHandPointer(HandPositions[0]);

            westrayButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(ExitBlacksmith());
                handPointerCreator.DestroyHandPointer();
            });
        }

        IEnumerator ExitBlacksmith()
        {
            StartCoroutine(objectFader.DisappearCanvasGroup(instantiatedHeroPopUp));

            yield return new WaitForSeconds(.5f);
            handPointerCreator.InstantiateHandPointer(HandPositions[1]);
            instantiatedHeroPopUp.GetComponentInChildren<Text>().text = "You can see the items that are profitable if you trade them with the next town.\nCloth looks good!\nTap on Trade Goods.";
            StartCoroutine(objectFader.ApearCanvasGroup(instantiatedHeroPopUp));
            buttonDeactivator.DeactivateAllButtonsExcept("Trade Goods Button", buttons);
        }

        void TapFirstTownToGoTutorial()
        {
            heroPopUpCreator.CreateHeroPopUp(ref instantiatedHeroPopUp, "It is time to start the journey.\nTap on Westray to select it.", tutorialPositions[0].transform, 2);

            buttonDeactivator.DeactivateAllButtonsExcept("Westray", buttons);

            handPointerCreator.InstantiateHandPointer(HandPositions[0]);

            westrayButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(Go());
                handPointerCreator.DestroyHandPointer();
            });
        }

        IEnumerator Go()
        {
            StartCoroutine(objectFader.DisappearCanvasGroup(instantiatedHeroPopUp));

            yield return new WaitForSeconds(.5f);
            handPointerCreator.InstantiateHandPointer(HandPositions[2]);
            instantiatedHeroPopUp.GetComponentInChildren<Text>().text = "Wish us good luck and tap on Go.\nHave Fun!";
            StartCoroutine(objectFader.ApearCanvasGroup(instantiatedHeroPopUp));
            buttonDeactivator.DeactivateAllButtonsExcept("Go Button", buttons);
        }
    }
}
