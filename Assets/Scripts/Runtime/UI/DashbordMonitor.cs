using TMPro;
using UnityEngine;

namespace SA.Game
{
    public class DashbordMonitor : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _value;
        [SerializeField] private RectTransform _arrow;
        [SerializeField] private GameObject _valueDisplay;
        [SerializeField] private int _maxValue = 200;
        [SerializeField] private float _startAngle = 44f;
        [SerializeField] private float _endAngle = -224;
        [SerializeField] private bool _isShowValueDidsplay;

        private float _curAngle;
        private float angleVelocity;

        private void Awake() 
        {
            _valueDisplay.SetActive(_isShowValueDidsplay);    
            _curAngle = _startAngle;
        }

        public void SetValue(float value)
        {
            DisplayValue((int)value);            
            RotateArrow(value);
        }

        private void DisplayValue(int value)
        {
            _value.text = $"{value}";
        }

        private void RotateArrow(float value)
        {
            value = Mathf.Clamp(value, 0f, _maxValue);
            var normValue = Mathf.Clamp01(value / _maxValue);
            var range = _startAngle - _endAngle;
            var angle = _startAngle - range * normValue;_curAngle = Mathf.SmoothDampAngle(_curAngle, angle, ref angleVelocity, 0.1f);
            _arrow.eulerAngles = new Vector3(0f, 0f, angle);
        }
    }
}
