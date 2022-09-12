namespace AstroOdyssey
{
    public interface IMeteorFactory 
    {
        void SetGameEnvironment(GameEnvironment gameEnvironment);

        void SpawnMeteor(GameLevel gameLevel);

        void GenerateMeteor(GameLevel gameLevel);

        void DestroyMeteor(Meteor meteor);

        void UpdateMeteor(Meteor meteor, out bool destroyed);

        void LevelUp();
    }
}
