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

        private int rotatedMeteorSpawnCounter = 10;
        private int rotatedMeteorSpawnLimit = 10;

        private int overPoweredMeteorSpawnCounter = 15;
        private int overPoweredMeteorSpawnLimit = 15;

        private int meteorCounter;
        private int meteorSpawnLimit = 55;
        private double meteorSpeed = 1.5;

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
        /// Levels up meteors.
        /// </summary>
        public void LevelUp()
        {
            meteorSpawnLimit -= 2;
            meteorSpeed += 1;
        }

        /// <summary>
        /// Spawns a meteor.
        /// </summary>
        public void SpawnMeteor(GameLevel gameLevel)
        {
            if ((int)gameLevel > 0)
            {
                // each frame progress decreases this counter
                meteorCounter -= 1;

                // when counter reaches zero, create a meteor
                if (meteorCounter < 0)
                {
                    GenerateMeteor(gameLevel);
                    meteorCounter = meteorSpawnLimit;
                }
            }
        }

        /// <summary>
        /// Generates a random meteor.
        /// </summary>
        public void GenerateMeteor(GameLevel gameLevel)
        {
            var newMeteor = new Meteor();

            newMeteor.SetAttributes(speed: meteorSpeed + random.NextDouble(), scale: gameEnvironment.GetGameObjectScale());

            double left = 0;
            double top = 0;

            // generate large but slower and stronger enemies after level 3
            if (gameLevel > GameLevel.Level_3 && overPoweredMeteorSpawnCounter <= 0)
            {
                overPoweredMeteorSpawnCounter = overPoweredMeteorSpawnLimit;

                newMeteor.OverPower();

                overPoweredMeteorSpawnLimit = random.Next(10, 20);
            }

            // generate side ways flying meteors after level 2
            if (gameLevel > GameLevel.Level_2 && rotatedMeteorSpawnCounter <= 0)
            {
                newMeteor.XDirection = (XDirection)random.Next(1, Enum.GetNames<XDirection>().Length);
                rotatedMeteorSpawnCounter = rotatedMeteorSpawnLimit;

                switch (newMeteor.XDirection)
                {
                    case XDirection.LEFT:
                        newMeteor.Rotation = (newMeteor.Rotation + 1) * -1;
                        left = gameEnvironment.Width;
                        break;
                    case XDirection.RIGHT:
                        newMeteor.Rotation = newMeteor.Rotation + 1;
                        left = 0 - newMeteor.Width + 10;
                        break;
                    default:
                        break;
                }

                // side ways meteors fly at a higher speed
                newMeteor.Speed++;
#if DEBUG
                Console.WriteLine("Meteor XDirection: " + newMeteor.XDirection + ", " + "X: " + left + " " + "Y: " + top);
#endif
                top = random.Next(0, (int)gameEnvironment.Height / 3);
                newMeteor.Rotate();

                // randomize next x flying meteor pop up
                rotatedMeteorSpawnLimit = random.Next(5, 15);
            }
            else
            {
                left = random.Next(10, (int)gameEnvironment.Width - 70);
                top = 0 - newMeteor.Height;
            }

            newMeteor.AddToGameEnvironment(top: 0 - newMeteor.Height, left: random.Next(10, (int)gameEnvironment.Width - 100), gameEnvironment: gameEnvironment);

            rotatedMeteorSpawnCounter--;
            overPoweredMeteorSpawnCounter--;
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
            meteor.MoveX();

            // if the object is marked for lazy destruction then no need to perform collisions
            if (meteor.MarkedForFadedRemoval)
                return;

            // if meteor or meteor object has gone beyond game view
            destroyed = gameEnvironment.CheckAndAddDestroyableGameObject(meteor);
        }

        #endregion
    }
}
