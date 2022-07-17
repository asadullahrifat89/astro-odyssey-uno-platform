namespace AstroOdyssey
{
    public class EnemyProjectileHelper 
    {
        #region Fields

        private readonly GameEnvironment gameEnvironment;
        private readonly string baseUrl;

        #endregion

        #region Ctor

        public EnemyProjectileHelper(GameEnvironment gameEnvironment, string baseUrl)
        {
            this.gameEnvironment = gameEnvironment;
            this.baseUrl = baseUrl;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates a projectile.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="destroyed"></param>
        public void UpdateProjectile(EnemyProjectile projectile, out bool destroyed)
        {
            destroyed = false;

            // move projectile down                
            projectile.MoveY();

            // remove projectile if outside game canvas
            if (projectile.GetY() > gameEnvironment.Height)
            {
                gameEnvironment.AddDestroyableGameObject(projectile);
                destroyed = true;
            }
        }

        #endregion
    }
}
