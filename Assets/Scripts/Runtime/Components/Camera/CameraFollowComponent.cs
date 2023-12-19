using UnityEngine;

namespace SA.Game
{
    public struct CameraFollowComponent
    {
        public CameraConfig Config;

        public Camera CameraRef { get; internal set; }
    }
}