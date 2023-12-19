using FMOD.Studio;

namespace SA.Game
{
    public static class FMODAudioExtensions
    {
        public static void TryStop(this EventInstance eventInstance)
        {
            eventInstance.getPlaybackState(out PLAYBACK_STATE state);

            if (state.Equals(PLAYBACK_STATE.STARTING))
            {
                eventInstance.stop(STOP_MODE.ALLOWFADEOUT);
            }
        }

        public static void TryPlay(this EventInstance eventInstance)
        {
            eventInstance.getPlaybackState(out PLAYBACK_STATE state);

            if (state.Equals(PLAYBACK_STATE.STOPPED))
            {
                eventInstance.start();
            }
        }
    }
}