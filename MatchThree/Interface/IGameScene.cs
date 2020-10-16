namespace MatchThree.Interface
{
    public interface IGameScene : IUpdatable, IDrawable
    {
        bool IsLoaded { get; }

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