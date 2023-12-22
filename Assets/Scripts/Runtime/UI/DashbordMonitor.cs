using TMPro;
using UnityEngine;

namespace SA.Game
{
    public class DashbordMonitor : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _value;
        [SerializeField] private RectTransform _arrow;
        [SerializeField] private GameObject _valueDisplay;
        [SerializeField] private float _minAngle;
        [SerializeField] private float _maxAngle;
        [SerializeField] private bool _isShowValueDidsplay;

        private void Awake() 
        {
            _valueDisplay.SetActive(_isShowValueDidsplay);    
        }

        public void SetNormalizeValue(float normValue, int value)
        {
            RotateArrow(normValue);
            DisplayValue(value);
        }

        private void DisplayValue(int value)
        {
            _value.text = $"{value}";
        }

        private void RotateArrow(float normValue)
        {
            var angle = Mathf.Lerp(_minAngle, _maxAngle, normValue);
            _arrow.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
