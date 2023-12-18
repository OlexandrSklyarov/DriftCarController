
namespace SA.Game
{
    public class SharedData
    {
        public SceneData SceneData;
        public MainConfig MainConfig;
        public IInputService Input;

        public TimeService TimeService { get; internal set; }
    }
}