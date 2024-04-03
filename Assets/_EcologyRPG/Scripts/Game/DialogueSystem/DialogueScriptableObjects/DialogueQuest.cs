using EcologyRPG.Utility.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Game.Dialogue
{
    [CreateAssetMenu(fileName = "New Quest Dialogue", menuName = "Dialogue/New Quest Dialogue")]
    public class DialogueQuest : Interaction
    {
        [SerializeField] private List<DialogueQuestInfo> quests;

        public IReadOnlyList<DialogueQuestInfo> Quests => quests;

    }
}