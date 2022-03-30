using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands
{
    public class ShipShopTutorial : MonoBehaviour
    {
        Button[] buttons;

        GameObject instantiatedHeroPopUp;

        [SerializeField] GameObject[] tutorialPositions;
        [SerializeField] HeroPopUpCreator heroPopUpCreator;

        ButtonDeactivator buttonDeactivator = new ButtonDeactivator();
        ObjectFader objectFader = new ObjectFader();

        [SerializeField] GameObject buyButton;
        [SerializeField] GameObject upgradeButton;

        [SerializeField] HandPointerCreator handPointerCreator;
        [SerializeField] Transform[] HandPositions;

        void Start()
        {
            buttons = GameObject.FindObjectsOfType<Button>();

            if (TutorialManager.SecondTimeEnteringFromShipShop)
                BuyFirstCarTutorial();
        }

        void BuyFirstCarTutorial()
        {
            heroPopUpCreator.CreateHeroPopUp(ref instantiatedHeroPopUp, "With the money that you have, you can buy this vehicle.\nTap Buy to purchase it.", tutorialPositions[0].transform, 1);

            buttonDeactivator.DeactivateAllButtonsExcept("Buy", buttons);

            handPointerCreator.InstantiateHandPointer(HandPositions[0]);

            buyButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(ExitShipShop());
                handPointerCreator.DestroyHandPointer();
            });
        }

        IEnumerator ExitShipShop()
        {
            StartCoroutine(objectFader.DisappearCanvasGroup(instantiatedHeroPopUp));

            yield return new WaitForSeconds(.5f);
            handPointerCreator.InstantiateHandPointer(HandPositions[1]);
            instantiatedHeroPopUp.GetComponentInChildren<Text>().text = "Okay, now let's get out of here.\nPress the back button.";
            StartCoroutine(objectFader.ApearCanvasGroup(instantiatedHeroPopUp));
            buttonDeactivator.DeactivateAllButtonsExcept("Exit Button", buttons);
        }
    }
}
