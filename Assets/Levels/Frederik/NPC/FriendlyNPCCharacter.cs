using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FriendlyNPCCharacter : BaseCharacter, IInteractable
{
    [SerializeField] private ScriptableObject interaction; // might want to make a middle class for NPC interactions so this is not just a ScriptableObject
    public ScriptableObject Interaction => interaction;


    public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
        base.Update();

        }
        private void FixedUpdate()
        {

        }

        private void LateUpdate()
        {
            
        }
    public void Interact()
    {
        // this needs multiple setups, dialogue and dialogue with choices interaction, quest interaction, repeatable delivery interaction
    }
}
