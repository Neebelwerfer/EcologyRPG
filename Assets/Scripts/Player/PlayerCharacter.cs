using Character;
using Items;
using log4net.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerCharacter : BaseCharacter
    {
        public PlayerSettings playerSettings;

        PlayerMovement playerMovement;
        PlayerResourceManager playerResourceManager;
        public PlayerAbilitiesHandler playerAbilitiesHandler;

        public Inventory Inventory { get; private set; }

        public override Vector3 Forward => playerMovement.transform.forward;
        public override Vector3 Position => playerMovement.transform.position;

        readonly List<PlayerModule> modules = new();

        public override void Start()
        {
            base.Start();

            playerMovement = new PlayerMovement();
            modules.Add(playerMovement);

            playerAbilitiesHandler = new PlayerAbilitiesHandler();
            modules.Add(playerAbilitiesHandler);

            playerResourceManager = new PlayerResourceManager();
            modules.Add(playerResourceManager);

            PlayerLevelHandler playerLevelHandler = new PlayerLevelHandler();
            modules.Add(playerLevelHandler);

            Inventory = new Inventory(this, playerSettings.StartingItems);

            foreach (PlayerModule module in modules)
            {
                module.Initialize(this);
            }

            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }

        public virtual void LevelUp()
        {
            level++;
            foreach (var mod in levelMods)
            {
                mod.Value = level;
            }
        }

        public override void Update()
        {
            base.Update();
            foreach (PlayerModule module in modules)
            {
                module.Update();
            }
        }

        void FixedUpdate()
        {
            foreach (PlayerModule module in modules)
            {
                module.FixedUpdate();
            }
        }

        void LateUpdate()
        {
            foreach (PlayerModule module in modules)
            {
                module.LateUpdate();
            }
        }

        private void OnDestroy()
        {
            foreach (PlayerModule module in modules)
            {
                module.OnDestroy();
            }
        }
    }

}
