using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "New Dialogue Choices")]

public class DialogueChoices : Interaction
{
    [SerializeField] private List<DialogueChoice> options;

    public IReadOnlyList<DialogueChoice> Options => options;
}
