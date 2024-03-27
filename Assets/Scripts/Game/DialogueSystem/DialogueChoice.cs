using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    [SerializeField] private string choiceText;
    [SerializeField] private DialoguePathLine choicePath;

    public string ChoiceText => choiceText;
    public DialoguePathLine ChoicePath => choicePath;
}

