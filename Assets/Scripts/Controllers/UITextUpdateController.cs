using UnityEngine;
using UnityEngine.UI;

namespace Controllers {
    // This class is on the Text components of the plate UI and the chopping board UI
    public class UITextUpdateController : MonoBehaviour
    {
        [SerializeField] private Text textUI;

        public void UpdateText(string text)
        {
            textUI.text = text;
        }
    }
}