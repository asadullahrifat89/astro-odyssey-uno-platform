using System;
using static AstroOdyssey.Constants;

namespace AstroOdyssey
{
    public class MeteorHelper
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;

        private readonly Random random = new Random();

        private readonly string baseUrl;

        #endregion

        #region Ctor

        public MeteorHelper(GameEnvironment gameEnvironment, string baseUrl)
        {
            this.gameEnvironment = gameEnvironment;
            this.baseUrl = baseUrl;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates a random meteor.
        /// </summary>
        public void GenerateMeteor(double meteorSpeed)
        {
            var NewMeteor = new Meteor();

            NewMeteor.SetAttributes(speed: meteorSpeed + random.NextDouble(), scale: gameEnvironment.GetGameObjectScale());
            NewMeteor.AddToGameEnvironment(top: 0 - NewMeteor.Height, left: random.Next(10, (int)gameEnvironment.Width - 100), gameEnvironment: gameEnvironment);
        }

        /// <summary>
        /// Destroys a meteor. Removes from game environment, increases player score, plays sound effect.
        /// </summary>
        /// <param name="meteor"></param>
        public void DestroyMeteor(Meteor meteor)
        {
            meteor.MarkedForFadedRemoval = true;
            App.PlaySound(baseUrl, SoundType.METEOR_DESTRUCTION);
        }

        /// <summary>
        /// Updates an meteor. Moves the meteor inside game environment and removes from it when applicable.
        /// </summary>
        /// <param name="meteor"></param>
        /// <param name="destroyed"></param>
        public void UpdateMeteor(Meteor meteor, out bool destroyed)
        {
            destroyed = false;

            // move meteor down
            meteor.Rotate();
            meteor.MoveY();

            // if the object is marked for lazy destruction then no need to perform collisions
            if (meteor.MarkedForFadedRemoval)
                return;

            // if meteor or meteor object has gone beyond game view
            destroyed = AddDestroyableGameObject(meteor);
        }

        /// <summary>
        /// Removes a game object from game view. 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private bool AddDestroyableGameObject(GameObject gameObject)
        {
            // if game object is out of bounds of game view
            if (gameObject.GetY() > gameEnvironment.Height || gameObject.GetX() > gameEnvironment.Width || gameObject.GetX() + gameObject.Width < 0)
            {
                gameEnvironment.AddDestroyableGameObject(gameObject);

                return true;
            }

            return false;
        }

        #endregion
    }
}
