
namespace SA.Game
{
    public struct CarEngineComponent
    {
        public ICarEngine EngineRef;
        public float Speed;

        public float RealSpeed { get; internal set; }
    }
}