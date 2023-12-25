using UnityEngine;

namespace SA.Game
{
    [CreateAssetMenu(fileName = "WindowConfig", menuName = "SO/WindowConfig")]
    public sealed class WindowConfig : ScriptableObject
    {
        [field: SerializeField] public BaseUIWindow[] WindowPrefabs {get; private set;} 
    }
}