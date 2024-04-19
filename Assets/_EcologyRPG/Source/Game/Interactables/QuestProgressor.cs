using EcologyRPG.GameSystems;
using EcologyRPG.GameSystems.Interactables;
using UnityEngine;

namespace EcologyRPG.GameSystems.Interactables
{
    [System.Serializable]

    [CreateAssetMenu(menuName = "Interactables/Quest Progessor")]
    public class QuestProgressor : Interaction
    {
        [SerializeField] private string questFlag; //quest flag to be changed
        [SerializeField] private int progressStage; // value to change too
        public string QuestFlag => questFlag;
        public int ProgessStage => progressStage;

        public override void Interact()
        {
            Game.Flags.SetFlag(QuestFlag, ProgessStage);
        }
    }
}
