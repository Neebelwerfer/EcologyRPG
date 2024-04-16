using EcologyRPG.GameSystems;
using EcologyRPG.GameSystems.Interactables;
using UnityEngine;

namespace EcologyRPG.GameSystems.Interactables
{
    [System.Serializable]

    [CreateAssetMenu(menuName = "Interactables/Quest Progessor")]
    public class QuestProgressor : Interaction
    {
        [SerializeField] private string questFlag;
        [SerializeField] private int progressStage;
        public string QuestFlag => questFlag;
        public int ProgessStage => progressStage;

        public override void Interact()
        {
            Game.Flags.SetFlag(QuestFlag, ProgessStage);
        }
    }
}
