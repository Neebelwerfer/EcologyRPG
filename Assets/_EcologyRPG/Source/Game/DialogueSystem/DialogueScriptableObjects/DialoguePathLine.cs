using EcologyRPG.GameSystems.Interactables;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue Path", menuName = "Dialogue/New Dialogue Path Line")]
    public class DialoguePathLine : Interaction
    {
        [SerializeField] private List<Dialogue> dialogues;
        public IReadOnlyList<Dialogue> Dialogues => dialogues;

        public override void Interact()
        {
            DialogueWindow.current.Open(this);
        }
    }
}