using System;
using TMPro;
using UnityEngine;

namespace SA.Game
{
    [Serializable]
    public class CarDashbordUI
    {
        [field: SerializeField] public DashbordMonitor SpeedDisplay {get; private set;}
        [field: SerializeField] public DashbordMonitor RPMDisplay {get; private set;}
        [SerializeField] private TextMeshProUGUI _gearValue;

        public void SetGearValue(int value) => _gearValue.text = $"[{value}]";
    }
}