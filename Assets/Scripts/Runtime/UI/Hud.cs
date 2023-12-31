using UnityEngine;

namespace SA.Game
{
    public class Hud : MonoBehaviour
    {
        [field: SerializeField] public CarDashbordUI CarDashbord {get; private set;}
        [field: Space, SerializeField] public DriftResultPanel DriftPanel {get; private set;}
    }
}
