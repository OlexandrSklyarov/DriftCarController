using UnityEngine;

namespace SA.Game
{
    public class Hud : MonoBehaviour
    {
        [field: SerializeField] public DashbordMonitor SpeedDisplay {get; private set;}
        [field: SerializeField] public DashbordMonitor RPMDisplay {get; private set;}
    }
}
