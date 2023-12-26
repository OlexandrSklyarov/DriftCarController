
namespace SA.Game
{
    public struct CarEngineComponent
    {
        public ICarEngine EngineRef;
        public float RealSpeed;
        public float RPM;
        internal float SpeedFactor;
    }
}