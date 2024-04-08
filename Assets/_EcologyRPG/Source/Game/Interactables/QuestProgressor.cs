using EcologyRPG.GameSystems;
using EcologyRPG.GameSystems.Interactables;
using UnityEngine;

namespace EcologyRGP.GameSystems.Interactables
{
    [System.Serializable]
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
