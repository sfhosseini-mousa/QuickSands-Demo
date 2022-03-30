using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands
{
    public class TradeGoodsTutorial : MonoBehaviour
    {
        Button[] buttons;

        GameObject instantiatedHeroPopUp;

        [SerializeField] GameObject[] tutorialPositions;
        [SerializeField] HeroPopUpCreator heroPopUpCreator;

        ButtonDeactivator buttonDeactivator = new ButtonDeactivator();
        ObjectFader objectFader = new ObjectFader();

        [SerializeField] GameObject buyButton;

        [SerializeField] HandPointerCreator handPointerCreator;
        [SerializeField] Transform[] HandPositions;
        [SerializeField] Slider countSlider;

        void Start()
        {
            buttons = GameObject.FindObjectsOfType<Button>();
            

            if (TutorialManager.SixthTimeGoingFromTradeGoodsToMap)
            {
                buttonDeactivator.DeactivateAllButtonsExcept("", buttons);
                StartCoroutine(BuyFirstItemTutorial());
                TutorialManager.FifthTimeGoingFromMapToTradeGoods = false;
            }
        }

        IEnumerator BuyFirstItemTutorial()
        {
            heroPopUpCreator.CreateHeroPopUp(ref instantiatedHeroPopUp, "Select Cloth to choose the quantity.", tutorialPositions[0].transform, 2);
            instantiatedHeroPopUp.GetComponentInChildren<Text>().gameObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            instantiatedHeroPopUp.GetComponentInChildren<Text>().gameObject.transform.localPosition = new Vector3(0f, -16f, 0f);


            yield return new WaitForSeconds(0.01f);
            buttons = GameObject.FindObjectsOfType<Button>();
            buttonDeactivator.DeactivateAllButtonsExcept("ItemHolder", buttons);

            handPointerCreator.InstantiateHandPointer(HandPositions[0]);
            Button quest = null;
            foreach (var item in buttons)
            {
                if (item.name == "ItemHolder")
                    quest = item;
            }
            quest.onClick.AddListener(() =>
            {
                StartCoroutine(BuyCloth());
                handPointerCreator.DestroyHandPointer();
            });
        }

        IEnumerator BuyCloth()
        {
            StartCoroutine(objectFader.DisappearCanvasGroup(instantiatedHeroPopUp));

            buttons = GameObject.FindObjectsOfType<Button>();
            buttonDeactivator.DeactivateAllButtonsExcept("", buttons);


            yield return new WaitForSeconds(.5f);
            handPointerCreator.InstantiateHandPointer(HandPositions[1]);
            instantiatedHeroPopUp.GetComponentInChildren<Text>().text = "Drag the slider all the way to the right and press Buy.";
            StartCoroutine(objectFader.ApearCanvasGroup(instantiatedHeroPopUp));

            buyButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(ExitQuestBoard());
                handPointerCreator.DestroyHandPointer();
            });
        }

        public void SliderHitMax()
        {
            if (TutorialManager.SixthTimeGoingFromTradeGoodsToMap)
            {
                if ((int)(countSlider.value) == LocationDB.getLocation(Player.CurrentLocation.Id - 1).ItemStock[0])
                {
                    buyButton.GetComponent<Button>().interactable = true;
                }
                else
                {
                    buyButton.GetComponent<Button>().interactable = false;
                }
            }
        }

        IEnumerator ExitQuestBoard()
        {
            StartCoroutine(objectFader.DisappearCanvasGroup(instantiatedHeroPopUp));

            yield return new WaitForSeconds(.5f);
            handPointerCreator.InstantiateHandPointer(HandPositions[2]);
            instantiatedHeroPopUp.GetComponentInChildren<Text>().text = "Now we should go back.\nTap on the map.";
            StartCoroutine(objectFader.ApearCanvasGroup(instantiatedHeroPopUp));

            buttons = GameObject.FindObjectsOfType<Button>();
            buttonDeactivator.DeactivateAllButtonsExcept("Map", buttons);
        }
    }
}
