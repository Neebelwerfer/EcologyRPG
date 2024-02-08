using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : Character
{
    public InputActionReference Movement;
    public InputActionReference Sprint;

    PlayerMovement playerMovement;

    public override void Start()
    {
        base.Start();
        playerMovement = new PlayerMovement();
        playerMovement.Initialize(this);
    }

    void Update()
    {
        playerMovement.Update();
    }
}
