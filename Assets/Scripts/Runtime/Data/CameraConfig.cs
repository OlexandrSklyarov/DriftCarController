using UnityEngine;

namespace SA.Game
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "SO/Camera/CameraConfig")]
    public sealed class CameraConfig : ScriptableObject
    {
        [field: SerializeField, Min(1f)] public float FollowSpeed {get; private set;} = 10f;
        [field: SerializeField, Min(1f)] public float LookSpeed {get; private set;} = 10f;
        [field: SerializeField] public Vector3 Offset {get; private set;}
    }
}