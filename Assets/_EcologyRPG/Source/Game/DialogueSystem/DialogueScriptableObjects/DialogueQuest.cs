using EcologyRPG.GameSystems.Interactables;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.Dialogue
{
    [CreateAssetMenu(fileName = "New Quest Dialogue", menuName = "Dialogue/New Quest Dialogue")]
    public class DialogueQuest : Interaction
    {
        [SerializeField] private List<DialogueQuestInfo> quests;

        public IReadOnlyList<DialogueQuestInfo> Quests => quests;

        public override void Interact()
        {
        }
    }
}