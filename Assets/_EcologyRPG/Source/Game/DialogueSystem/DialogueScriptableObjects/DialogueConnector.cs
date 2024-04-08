using EcologyRPG.Utility.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.Dialogue
{
    [CreateAssetMenu(fileName = "New Connection", menuName = "Dialogue/New Dialogue Connection")]
    public class DialogueConnector : Interaction
    {
        [SerializeField] private DialogueConnection dialogueConnection;
        public DialogueConnection DialogueConnection => dialogueConnection;

        public override void Interact()
        {
            DialogueWindow.current.Open(this);
        }
    }
}
