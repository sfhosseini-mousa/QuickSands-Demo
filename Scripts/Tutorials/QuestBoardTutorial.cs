using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands
{
    public class QuestBoardTutorial : MonoBehaviour
    {
        Button[] buttons;

        GameObject instantiatedHeroPopUp;

        [SerializeField] GameObject[] tutorialPositions;
        [SerializeField] HeroPopUpCreator heroPopUpCreator;

        ButtonDeactivator buttonDeactivator = new ButtonDeactivator();
        ObjectFader objectFader = new ObjectFader();

        [SerializeField] GameObject acceptButton;

        [SerializeField] HandPointerCreator handPointerCreator;
        [SerializeField] Transform[] HandPositions;

        void Start()
        {
            buttons = GameObject.FindObjectsOfType<Button>();
            buttonDeactivator.DeactivateAllButtonsExcept("", buttons);

            if (TutorialManager.FourthTimeEnteringFromQuestBoard)
                StartCoroutine(AcceptFirstQuestTutorial());
        }

        IEnumerator AcceptFirstQuestTutorial()
        {
            heroPopUpCreator.CreateHeroPopUp(ref instantiatedHeroPopUp, "Looks like we found someone!\nSelect their note.", tutorialPositions[0].transform, 2);
            instantiatedHeroPopUp.GetComponentInChildren<Text>().gameObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            instantiatedHeroPopUp.GetComponentInChildren<Text>().gameObject.transform.localPosition = new Vector3(-17f, -4f, 0f);


            yield return new WaitForSeconds(0.01f);
            buttons = GameObject.FindObjectsOfType<Button>();
            buttonDeactivator.DeactivateAllButtonsExcept("QuestHolder", buttons);

            handPointerCreator.InstantiateHandPointer(HandPositions[0]);
            Button quest = null;
            foreach (var item in buttons)
            {
                if (item.name == "QuestHolder")
                    quest = item;
            }
            quest.onClick.AddListener(() =>
            {
                StartCoroutine(AcceptQuest());
                handPointerCreator.DestroyHandPointer();
            });
        }

        IEnumerator AcceptQuest()
        {
            StartCoroutine(objectFader.DisappearCanvasGroup(instantiatedHeroPopUp));

            buttons = GameObject.FindObjectsOfType<Button>();
            buttonDeactivator.DeactivateAllButtonsExcept("", buttons);


            yield return new WaitForSeconds(.5f);
            handPointerCreator.InstantiateHandPointer(HandPositions[1]);
            instantiatedHeroPopUp.GetComponentInChildren<Text>().text = "Now accept the quest.";
            StartCoroutine(objectFader.ApearCanvasGroup(instantiatedHeroPopUp));
            buttonDeactivator.DeactivateAllButtonsExcept("Accept Btn", buttons);

            acceptButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(ExitQuestBoard());
                handPointerCreator.DestroyHandPointer();
            });
        }

        IEnumerator ExitQuestBoard()
        {
            StartCoroutine(objectFader.DisappearCanvasGroup(instantiatedHeroPopUp));

            yield return new WaitForSeconds(.5f);
            handPointerCreator.InstantiateHandPointer(HandPositions[2]);
            instantiatedHeroPopUp.GetComponentInChildren<Text>().text = "You can always find interesting quests here.\nPress back to exit.";
            StartCoroutine(objectFader.ApearCanvasGroup(instantiatedHeroPopUp));
            buttonDeactivator.DeactivateAllButtonsExcept("Back", buttons);
        }
    }
}
