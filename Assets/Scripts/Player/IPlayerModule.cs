public interface IPlayerModule
{
    public void Initialize(PlayerCharacter player);

    public void Update();
    public void FixedUpdate();
    public void LateUpdate();
}