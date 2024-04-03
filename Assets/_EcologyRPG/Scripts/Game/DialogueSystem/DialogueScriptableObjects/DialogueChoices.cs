using EcologyRPG.Utility.Interactions;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Game.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue Choices", menuName = "Dialogue/New Dialogue Choices")]
    public class DialogueChoices : Interaction
    {
        [SerializeField] private List<DialogueChoice> options;

        public IReadOnlyList<DialogueChoice> Options => options;
    }
}
