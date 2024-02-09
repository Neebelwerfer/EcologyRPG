using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCharacter : BaseCharacter
    {
        public PlayerSettings playerSettings;

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

        void FixedUpdate()
        {
            playerMovement.FixedUpdate();
        }

        void LateUpdate()
        {
            playerMovement.LateUpdate();
        }
    }

}
