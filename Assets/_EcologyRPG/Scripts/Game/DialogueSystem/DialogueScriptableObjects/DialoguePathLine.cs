using EcologyRPG.Utility.Interactions;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Game.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue Path", menuName = "Dialogue/New Dialogue Path Line")]
    public class DialoguePathLine : Interaction
    {
        [SerializeField] private List<Dialogue> dialogues;
        public IReadOnlyList<Dialogue> Dialogues => dialogues;

    }
}