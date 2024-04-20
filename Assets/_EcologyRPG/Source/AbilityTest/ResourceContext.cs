using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;

namespace EcologyRPG.AbilityTest
{
    public class ResourceContext
    {
        readonly Resource Resource;
        public ResourceContext(Resource resource)
        {
            Resource = resource;
        }

        public void SetCurrent(float value)
        {
            Resource.CurrentValue = value;
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