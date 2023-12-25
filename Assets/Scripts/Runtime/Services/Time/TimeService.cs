
namespace SA.Game 
{
    public sealed class TimeService 
    {
        public float Time;
        public float DeltaTime;
        public float FixedDeltaTime;
        public float UnscaledDeltaTime;
        public float UnscaledTime;

        public void OnUpdate()
        {
            Time = UnityEngine.Time.time;
            UnscaledTime = UnityEngine.Time.unscaledTime;
            DeltaTime = UnityEngine.Time.deltaTime;
            FixedDeltaTime = UnityEngine.Time.fixedDeltaTime;
            UnscaledDeltaTime = UnityEngine.Time.unscaledDeltaTime;
        }
    }
}