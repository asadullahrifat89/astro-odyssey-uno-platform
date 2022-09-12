namespace AstroOdyssey
{
    public interface IAudioHelper
    {
        void PlaySound(SoundType soundType);
        void StopSound();
        void PauseSound(SoundType soundType);
        void ResumeSound(SoundType soundType);
    }
}
