using UnityEngine;

namespace EcologyRPG.Game.Dialogue
{
    [System.Serializable]
    public class DialogueChoice
    {
        [SerializeField] private string choiceText;
        [SerializeField] private DialoguePathLine choicePath;

        public string ChoiceText => choiceText;
        public DialoguePathLine ChoicePath => choicePath;
    }
}