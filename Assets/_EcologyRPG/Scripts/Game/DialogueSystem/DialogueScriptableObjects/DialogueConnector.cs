using EcologyRPG.Utility.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Game.Dialogue
{
    [CreateAssetMenu(fileName = "New Connection", menuName = "Dialogue/New Dialogue Connection")]
    public class DialogueConnector : Interaction
    {
        [SerializeField] private DialogueConnection dialogueConnection;
        public DialogueConnection DialogueConnection => dialogueConnection;
    }
}
