using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands
{
    public class TownTutorial : MonoBehaviour
    {
        

        GameObject heroPrefab;
        GameObject instantiatedHeroPrefab;

        Button[] buttons;

        GameObject instantiatedHeroPopUp;

        [SerializeField] GameObject[] tutorialPositions;
        [SerializeField] HeroPopUpCreator heroPopUpCreator;
        [SerializeField] GameObject nextTutorialButton;

        ButtonDeactivator buttonDeactivator = new ButtonDeactivator();
        ObjectFader objectFader = new ObjectFader();
        [SerializeField] HandPointerCreator handPointerCreator;
        [SerializeField] Transform[] HandPositions;

        void Start()
        {
            buttons = GameObject.FindObjectsOfType<Button>();
            nextTutorialButton.SetActive(false);

            TutorialManager.FirstTimePlaying = Player.IsFirstBattle;

            if(TutorialManager.FirstTimePlaying)
                SetAllFirstTimes();

            Player.IsFirstBattle = false;
            Player.SavePlayer();

            if (TutorialManager.FirstTimeEntering)
            {
                TutorialManager.FirstTimePlaying = false;
                TutorialManager.FirstTimeEntering = false;
                StartCoroutine(FirstTimeEnteringTown());
            }
            else if(TutorialManager.SecondTimeEnteringFromShipShop)
            {
                TutorialManager.SecondTimeEnteringFromShipShop = false;
                SecondTimeEnteringTown();
            }
            else if (TutorialManager.ThirdTimeEnteringFromBlackSmith)
            {
                TutorialManager.ThirdTimeEnteringFromBlackSmith = false;
                ThirdTimeEnteringTown();
            }
            else if(TutorialManager.FourthTimeEnteringFromQuestBoard)
            {
                TutorialManager.FourthTimeEnteringFromQuestBoard = false;
                StartCoroutine(FourthTimeEnteringTown());
            }
        }

        void SetAllFirstTimes()
        {
            TutorialManager.FirstTimeEntering = Player.IsFirstBattle;
            TutorialManager.SecondTimeEnteringFromShipShop = Player.IsFirstBattle;
            TutorialManager.ThirdTimeEnteringFromBlackSmith = Player.IsFirstBattle;
            TutorialManager.FourthTimeEnteringFromQuestBoard = Player.IsFirstBattle;
            TutorialManager.FifthTimeGoingFromMapToTradeGoods = Player.IsFirstBattle;
            TutorialManager.SixthTimeGoingFromTradeGoodsToMap = Player.IsFirstBattle;
            TutorialManager.TutorialNotDone = Player.IsFirstBattle;
        }

        IEnumerator FirstTimeEnteringTown()
        {
            heroPopUpCreator.CreateHeroPopUp(ref instantiatedHeroPopUp, "Hello Stranger! Thank you for choosing me!\nAre you the one who found the champions' spoils?", tutorialPositions[0].transform, 1);

            buttonDeactivator.DeactivateAllButtonsExcept("NextTutorial Button", buttons);

            yield return new WaitForSeconds(1f);
            nextTutorialButton.SetActive(true);
        }

        public void SecondTutorial()
        {
            if(TutorialManager.SecondTimeEnteringFromShipShop)
                StartCoroutine(SecondTutorialCoroutine());
        }

        IEnumerator SecondTutorialCoroutine()
        {
            StartCoroutine(objectFader.DisappearCanvasGroup(instantiatedHeroPopUp));

            yield return new WaitForSeconds(.5f);

            instantiatedHeroPopUp.GetComponentInChildren<Text>().text = "Let's get you started!\n\nTap on the Ship Shop.";
            StartCoroutine(objectFader.ApearCanvasGroup(instantiatedHeroPopUp));
            nextTutorialButton.SetActive(false);
            handPointerCreator.InstantiateHandPointer(HandPositions[0]);
            buttonDeactivator.DeactivateAllButtonsExcept("ShipShop Button", buttons);
        }

        void SecondTimeEnteringTown()
        {
            heroPopUpCreator.CreateHeroPopUp(ref instantiatedHeroPopUp, "It's my turn now.\nI want some new gear.\nTap on Blacksmith.", tutorialPositions[1].transform, 1);
            instantiatedHeroPopUp.GetComponentInChildren<Text>().gameObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            instantiatedHeroPopUp.GetComponentInChildren<Text>().gameObject.transform.localPosition = new Vector3(-17f, -16f, 0f);

            buttonDeactivator.DeactivateAllButtonsExcept("BlackSmith Button", buttons);

            handPointerCreator.InstantiateHandPointer(HandPositions[1]);
        }

        void ThirdTimeEnteringTown()
        {
            heroPopUpCreator.CreateHeroPopUp(ref instantiatedHeroPopUp, "We need a passenger who can help us on the way to next Town.\nTap on the Quest Board to see if we can find someone.", tutorialPositions[0].transform, 2);

            buttonDeactivator.DeactivateAllButtonsExcept("QuestBoard Button", buttons);

            handPointerCreator.InstantiateHandPointer(HandPositions[2]);
        }

        IEnumerator FourthTimeEnteringTown()
        {
            heroPopUpCreator.CreateHeroPopUp(ref instantiatedHeroPopUp, "You can manage your quests in the Party tab", tutorialPositions[0].transform, 2);

            buttonDeactivator.DeactivateAllButtonsExcept("NextTutorial Button", buttons);

            handPointerCreator.InstantiateHandPointer(HandPositions[3]);

            yield return new WaitForSeconds(1f);
            nextTutorialButton.SetActive(true);
        }

        public void FourthTimeEnteringTownNext()
        {
            if (!TutorialManager.SecondTimeEnteringFromShipShop)
                StartCoroutine(FourthTimeEnteringTownNextCo());
        }

        IEnumerator FourthTimeEnteringTownNextCo()
        {
            handPointerCreator.DestroyHandPointer();
            StartCoroutine(objectFader.DisappearCanvasGroup(instantiatedHeroPopUp));

            yield return new WaitForSeconds(.5f);

            instantiatedHeroPopUp.GetComponentInChildren<Text>().text = "Let's see what you can invest your Coins in.\nTap on the Map";
            StartCoroutine(objectFader.ApearCanvasGroup(instantiatedHeroPopUp));
            nextTutorialButton.SetActive(false);
            handPointerCreator.InstantiateHandPointer(HandPositions[4]);
            buttonDeactivator.DeactivateAllButtonsExcept("Map", buttons);
        }
    }
}