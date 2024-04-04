using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.Dialogue
{
    [System.Serializable]
    public class DialogueQuestInfo
    {
        [SerializeField] private string questText;
        [SerializeField] private DialoguePathLine infoPath;
        [SerializeField] private DialoguePathLine completionPath;

        [SerializeField] private string questFlag;
        [SerializeField] private int completionValue;
        // TODO: something regarding connecting this to the flag system

        public string QuestText => questText;
        public string QuestFlag => questFlag;
        public int CompletionValue => completionValue;
        public DialoguePathLine InfoPath => infoPath;
        public DialoguePathLine CompletionPath => completionPath;
    }
}
