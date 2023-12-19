using UnityEngine;

namespace SA.Game
{
    [CreateAssetMenu(fileName = "MainConfig", menuName = "SO/MainConfig")]
    public sealed class MainConfig : ScriptableObject
    {
        [field: SerializeField] public CarView CarViewPrefab {get; private set;} 
        [field: SerializeField] public CameraConfig Camera {get; private set;} 
    }
}