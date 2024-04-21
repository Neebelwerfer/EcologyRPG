namespace EcologyRPG.AbilityTest
{
    [System.Serializable]
    public class AbilityData
    {
        public string Name;
        public string IconPath;
        public uint ID;
        public string Description;
        public float Cooldown;
        public string ScriptPath;

        public AbilityData(string name, uint ID, string description, float cooldown)
        {
            Name = name;
            this.ID = ID;
            Description = description;
            Cooldown = cooldown;
        }
    }
}