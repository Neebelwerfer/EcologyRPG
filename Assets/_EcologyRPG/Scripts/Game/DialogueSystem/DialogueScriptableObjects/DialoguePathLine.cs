using EcologyRPG.Utility.Interactions;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Game.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "New Dialogue Path Line")]
    public class DialoguePathLine : Interaction
    {
        [SerializeField] private List<Dialogue> dialogues;
        public IReadOnlyList<Dialogue> Dialogues => dialogues;

    }
}