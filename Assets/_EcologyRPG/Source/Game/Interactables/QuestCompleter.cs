using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.Interactables

{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Interactables/Quest Completer")]
    // this class is exclusively for the last state of a quest, it rewards the player with loot
    public class QuestCompleter : QuestProgressor
    {
        [SerializeField] private LootSpawner spawner; //Quest reward
        public LootSpawner Spawner => spawner;
        public override void Interact()
        {
            base.Interact();
            Spawner.Interact();
        }
    }
}
