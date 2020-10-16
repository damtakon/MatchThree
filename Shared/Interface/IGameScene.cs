namespace Shared.Interface
{
    public interface IGameScene : IUpdateDrawable
    {
        /// <summary>
        /// Load needed content for current scene
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Unload content needed only this scene
        /// </summary>
        void UnloadContent();
    }
}