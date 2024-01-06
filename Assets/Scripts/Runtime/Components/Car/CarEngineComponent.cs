
namespace SA.Game
{
    public struct CarEngineComponent
    {
        public ICarEngine EngineRef;
        public float TotalPower;
        public float RealSpeed;
        public float RPM;
        public float EngineRPM;
        public float SpeedFactor;
        public int GearIndex;
    }
}