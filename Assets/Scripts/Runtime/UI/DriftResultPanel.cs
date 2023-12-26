using TMPro;
using UnityEngine;

namespace SA.Game
{
    public class DriftResultPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _totalPoints;
        [SerializeField] private TextMeshProUGUI _currentAngle;
        [SerializeField] private TextMeshProUGUI _currentDriftPoints;
        [SerializeField] private TextMeshProUGUI _currentDriftFactor;
        [SerializeField] private GameObject _currentDriftResult;
        [SerializeField] private Gradient _pointsValueGradient;

        private void Awake() 
        {
            EnableDriftResultPanel(false);    
        }

        private void EnableDriftResultPanel(bool isActive)
        {
            _currentDriftResult.SetActive(isActive);
        }

        public void SetDriftResult(float totalPoints, float currentAngle, 
            float driftFactor, float driftFactorNorm, float currentDriftPoint, bool isShowDriftResult)
        {
            _totalPoints.text = totalPoints.ToString("###,###,000");
            _currentAngle.text = currentAngle.ToString("###,##0") + "°";
            _currentDriftFactor.text = driftFactor.ToString("###,###,##0.0") + "x"; 
            _currentDriftPoints.text = currentDriftPoint.ToString("###,###,000");

            var color = _pointsValueGradient.Evaluate(driftFactorNorm); 
            _currentDriftFactor.color = color;
            _currentDriftPoints.color = color;

            EnableDriftResultPanel(isShowDriftResult);  
        }
    }
}
