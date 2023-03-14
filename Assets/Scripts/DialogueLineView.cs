using TMPro;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueLineView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _line;

        public void SetText(DialogueLine line)
        {
            _line.text = $"{line.Text}";
        }
    }
}

