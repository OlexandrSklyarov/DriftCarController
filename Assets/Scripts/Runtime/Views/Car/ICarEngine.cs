using UnityEngine;
using static SA.Game.CarView;

namespace SA.Game
{
    public interface ICarEngine
    {
        public Rigidbody RB {get;}
        public CarConfig Config {get;}
        public WheelData[] Wheels {get;}   
        public int WheelDriveCount {get;}   
    }
}