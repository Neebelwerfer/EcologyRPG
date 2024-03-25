using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using Utility;
using DialogueWindows;

public class Interactor : MonoBehaviour, IInteractable
{
    [SerializeField] private Interaction interaction;
    [SerializeField] private DialogueWindow dialogueWindow;

    public Interaction Interaction => interaction;


    private void Update()
    {
        if (dialogueWindow != null) { dialogueWindow = FindObjectOfType<DialogueWindow>(); }
        //On use initialise interaction, call Interact()
    }

    public void Interact()
    {
        if (interaction is DialoguePathLine path)
        {
            dialogueWindow.Open(path);
        }
        else if (interaction is DialogueChoices choices)
        {
            dialogueWindow.Open(choices);
        }
    }
}
