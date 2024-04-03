using EcologyRPG.Utility.Interactions;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "New Dialogue Choices")]
    public class DialogueChoices : Interaction
    {
        [SerializeField] private List<DialogueChoice> options;

        public IReadOnlyList<DialogueChoice> Options => options;
    }
}
