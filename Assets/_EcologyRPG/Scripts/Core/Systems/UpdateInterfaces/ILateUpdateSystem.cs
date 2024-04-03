namespace EcologyRPG.Core.Systems
{
    public interface ILateUpdateSystem : ISystem
    {
        void OnLateUpdate();
    }
}
