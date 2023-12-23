
namespace SA.Game
{
    public struct CarEngineComponent
    {
        public ICarEngine EngineRef;
        public float RealSpeed;
        public float SpeedOnKmh;
        public float SpeedOnMph;
        public float RPM;
        public float CurrentTorque;
        public float Clutch;
        public float WheelRPM;
        public float NextChangeGearTime;
        public int GearIndex;
        public int NextGearIndex;
    }
}