using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sands
{
    public class BlackSmithTutorial : MonoBehaviour
    {
        Button[] buttons;

        GameObject instantiatedHeroPopUp;

        [SerializeField] GameObject[] tutorialPositions;
        [SerializeField] HeroPopUpCreator heroPopUpCreator;

        ButtonDeactivator buttonDeactivator = new ButtonDeactivator();

        [SerializeField] GameObject upgradeButton;

        [SerializeField] HandPointerCreator handPointerCreator;
        [SerializeField] Transform[] HandPositions;

        void Start()
        {
            buttons = GameObject.FindObjectsOfType<Button>();
            if (TutorialManager.ThirdTimeEnteringFromBlackSmith)
                UpgradeFirstHeroTutorial();
        }

        void UpgradeFirstHeroTutorial()
        {
            heroPopUpCreator.CreateHeroPopUp(ref instantiatedHeroPopUp, "Tap the Upgrade button to give me new Armor and Weapon.", tutorialPositions[0].transform, 1);

            buttonDeactivator.DeactivateAllButtonsExcept("Upgrade", buttons);

            handPointerCreator.InstantiateHandPointer(HandPositions[0]);

            upgradeButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(ExitBlacksmith());
                handPointerCreator.DestroyHandPointer();
            });
        }

        IEnumerator ExitBlacksmith()
        {
            Destroy(instantiatedHeroPopUp);
            heroPopUpCreator.CreateHeroPopUp(ref instantiatedHeroPopUp, "I feel stronger already.\nYou know the drill.\nPress the back button.", tutorialPositions[0].transform, 2);
            instantiatedHeroPopUp.GetComponentInChildren<SkeletonMecanim>().skeleton.A = 1f;

            yield return new WaitForSeconds(1f);
            handPointerCreator.InstantiateHandPointer(HandPositions[1]);
            buttonDeactivator.DeactivateAllButtonsExcept("Back", buttons);
        }
    }
}
