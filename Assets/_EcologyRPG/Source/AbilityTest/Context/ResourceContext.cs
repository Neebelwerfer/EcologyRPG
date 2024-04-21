using EcologyRPG.Core.Character;

namespace EcologyRPG.AbilityScripting
{
    public class ResourceContext
    {
        readonly Resource Resource;
        public ResourceContext(Resource resource)
        {
            Resource = resource;
        }

        public bool HaveEnough(float value)
        {
            return Resource.CurrentValue >= value;
        }

        public void SetCurrent(float value)
        {
            Resource.CurrentValue = value;
        }

        public void Consume(float value)
        {
            Resource.SetCurrentValue(Resource.CurrentValue - value);
        }
            
        public float GetCurrent()
        {
            return Resource.CurrentValue;
        }

        public float GetMax()
        {
            return Resource.MaxValue;
        }
    }
}