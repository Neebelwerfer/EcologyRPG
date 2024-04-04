using EcologyRPG.Utility.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRGP.Game
{
    [System.Serializable]
    public class QuestProgressor : Interaction
    {
        [SerializeField] private string questFlag;
        [SerializeField] private int progressStage;
        public string QuestFlag => questFlag;
        public int ProgessStage => progressStage;

    }
}
