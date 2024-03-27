namespace EcologyRPG.Utility.Interactions
{
    public interface IInteractable
    {
        Interaction Interaction { get; }
        void Interact();
    }
}