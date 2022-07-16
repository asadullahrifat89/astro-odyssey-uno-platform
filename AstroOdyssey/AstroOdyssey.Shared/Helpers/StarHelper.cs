using System;

namespace AstroOdyssey
{
    public class StarHelper
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;

        private readonly Random random = new Random();

        #endregion

        #region Ctor

        public StarHelper(GameEnvironment gameEnvironment)
        {
            this.gameEnvironment = gameEnvironment;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates a random star.
        /// </summary>
        public void GenerateStar(double starSpeed)
        {
            var newStar = new Star();

            newStar.SetAttributes(speed: starSpeed, scale: gameEnvironment.GetGameObjectScale());

            var top = 0 - newStar.Height;
            var left = random.Next(10, (int)gameEnvironment.Width - 10);

            newStar.AddToGameEnvironment(top: top, left: left, gameEnvironment: gameEnvironment);
        }

        #endregion
    }

    public class MeteorHelper
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;

        private readonly Random random = new Random();

        #endregion

        public MeteorHelper(GameEnvironment gameEnvironment)
        {
            this.gameEnvironment = gameEnvironment;
        }

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

        #endregion
    }
}
