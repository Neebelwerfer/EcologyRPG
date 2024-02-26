using Character;
using Items;
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

        public Inventory Inventory { get; private set; }

        public override void Start()
        {
            base.Start();
            playerMovement = new PlayerMovement();
            playerMovement.Initialize(this);
            playerAbilitiesHandler = new PlayerAbilitiesHandler();
            playerAbilitiesHandler.Initialize(this);
            Inventory = new Inventory(this, playerSettings.StartingItems);
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
