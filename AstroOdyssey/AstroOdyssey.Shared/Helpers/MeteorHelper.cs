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

        #endregion
    }
}
