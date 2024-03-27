namespace EcologyRPG.Game.Player
{
    public abstract class PlayerModule
    {
        public abstract void Initialize(PlayerCharacter player);

        public virtual void Update()
        {

        }
        public virtual void FixedUpdate()
        {

        }

        public virtual void LateUpdate()
        {

        }

        public abstract void OnDestroy();
    }
}