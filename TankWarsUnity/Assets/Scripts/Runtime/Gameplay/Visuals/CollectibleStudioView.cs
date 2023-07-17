namespace TankWars.Runtime.Gameplay.Visuals
{
    using TankWars.Runtime.Gameplay.Unlockables; 

    public class CollectibleStudioView : StudioDisplayView
    {
        public void UpdateCollectiblePlaceHolder(KeyChainCollectible keyChainCollectible)
        {
            UpdatePlaceHolderAppearance(keyChainCollectible.Mesh, keyChainCollectible.Material);
        }
    }
}
