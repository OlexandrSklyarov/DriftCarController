using UnityEngine;

namespace SA.Game
{
    public sealed class CarView : MonoBehaviour, ICarEngine
    {       
        [field: SerializeField] public Rigidbody RB {get; private set;}
        [field: SerializeField] public CarConfig Config {get; private set;}
        [field: Space, SerializeField] public WheelData[] Wheels {get; private set;}

        [Space, SerializeField] private Transform _centerOfMass;

        private void Awake()
        {
            RB.centerOfMass = _centerOfMass.localPosition;
        }

        #region Data
        [System.Serializable]
        public struct WheelData
        {
            public WheelCollider Wheel;
            public Transform TransformRef;
            public bool IsFront;
        }
        #endregion
    }
}
