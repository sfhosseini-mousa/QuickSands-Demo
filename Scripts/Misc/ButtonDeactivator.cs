using UnityEngine.UI;

namespace Sands
{
    class ButtonDeactivator
    {
        public void DeactivateAllButtonsExcept(string name, Button[] buttons)
        {
            foreach (var button in buttons)
            {
                if (button.gameObject.name != name)
                    button.interactable = false;
                else
                    button.interactable = true;
            }
        }

        public void ActivateAllButtons(Button[] buttons)
        {
            foreach (var item in buttons)
            {
                item.GetComponent<Button>().interactable = true;
            }
        }

    }
}
