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
        PlayerAbilitiesHandler playerAbilitiesHandler;

        public override void Start()
        {
            base.Start();
            playerMovement = new PlayerMovement();
            playerMovement.Initialize(this);
            playerAbilitiesHandler = new PlayerAbilitiesHandler();
            playerAbilitiesHandler.Initialize(this);
        }

        public override void Update()
        {
            base.Update();
            playerMovement.Update();
            playerAbilitiesHandler.Update();
        }

        void FixedUpdate()
        {
            playerMovement.FixedUpdate();
        }

        void LateUpdate()
        {
            playerMovement.LateUpdate();
        }

        private void OnDestroy()
        {
            playerMovement.OnDestroy();
            playerAbilitiesHandler.OnDestroy();
        }
    }

}
