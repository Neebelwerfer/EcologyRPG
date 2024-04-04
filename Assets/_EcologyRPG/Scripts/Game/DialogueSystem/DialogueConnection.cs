
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.Dialogue
{
    [System.Serializable]
    public class DialogueConnection
    {
        [SerializeField] private string letsTalk;
        [SerializeField] private string questTalk;
        [SerializeField] private DialogueChoices choices;
        [SerializeField] private DialogueQuest questDialogue;
        //A Quest version of Dialogue

        public string LetsTalk => letsTalk;
        public string QuestTalk => questTalk;
        public DialogueChoices Choices => choices;
        public DialogueQuest DialogueQuest => questDialogue;
    }
}
