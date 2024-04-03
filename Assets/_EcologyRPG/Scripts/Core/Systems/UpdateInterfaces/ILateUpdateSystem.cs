namespace EcologyRPG._Core.Systems
{
    public interface ILateUpdateSystem : ISystem
    {
        void OnLateUpdate();
    }
}
