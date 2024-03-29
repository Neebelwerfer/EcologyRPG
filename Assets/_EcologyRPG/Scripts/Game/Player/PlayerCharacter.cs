
using EcologyRPG.Core.Character;
using EcologyRPG.Core.Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcologyRPG.Game.Player
{
    public class PlayerCharacter : BaseCharacter
    {
        public PlayerSettings playerSettings;

        PlayerMovement playerMovement;
        PlayerResourceManager playerResourceManager;
        public PlayerAbilitiesHandler playerAbilitiesHandler;

        public Inventory Inventory { get; private set; }

        readonly List<PlayerModule> modules = new();

        public PlayerCharacter(PlayerSettings playerSettings) : base()
        {
            this.playerSettings = playerSettings;

            playerMovement = new PlayerMovement();
            modules.Add(playerMovement);
            playerMovement.Initialize(this);

            playerResourceManager = new PlayerResourceManager();
            modules.Add(playerResourceManager);

            PlayerLevelHandler playerLevelHandler = new PlayerLevelHandler();
            modules.Add(playerLevelHandler);

            Inventory = new Inventory(this, playerSettings.StartingItems);

            playerAbilitiesHandler = new PlayerAbilitiesHandler();
            modules.Add(playerAbilitiesHandler);

            foreach (PlayerModule module in modules)
            {
                module.Initialize(this);
            }

            faction = Faction.player;
            Tags = playerSettings.Tags;
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }


        public override void Die()
        {
            base.Die();
            PlayerManager.Instance.PlayerDead();
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
            if (IsPaused) return;
            base.Update();
            foreach (PlayerModule module in modules)
            {
                module.Update();
            }
        }

        public void FixedUpdate()
        {
            if (IsPaused) return;
            foreach (PlayerModule module in modules)
            {
                module.FixedUpdate();
            }
        }

        public void LateUpdate()
        {
            if (IsPaused) return;
            foreach (PlayerModule module in modules)
            {
                module.LateUpdate();
            }
        }
    }

}