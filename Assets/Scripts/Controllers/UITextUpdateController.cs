using UnityEngine;
using UnityEngine.UI;

namespace Controllers {
    public class UITextUpdateController : MonoBehaviour
    {
        [SerializeField] private Text textUI;

        public void UpdateText(string text)
        {
            textUI.text = text;
        }
    }
}