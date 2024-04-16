using System;

namespace EcologyRPG.Core.Items
{
    [Serializable]
    public class BasicGenerationRules : GenerationRules
    {
        public int minDropAmount;
        public int maxDropAmount;
    }
}