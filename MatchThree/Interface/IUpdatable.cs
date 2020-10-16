﻿using Microsoft.Xna.Framework;

namespace MatchThree.Interface
{
    public interface IUpdatable
    {

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void Update(GameTime gameTime);
    }
}