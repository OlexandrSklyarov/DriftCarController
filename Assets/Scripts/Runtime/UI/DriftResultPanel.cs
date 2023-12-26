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

        private void Awake() 
        {
            EnableDriftResultPanel(false);    
        }

        private void EnableDriftResultPanel(bool isActive)
        {
            _currentDriftResult.SetActive(isActive);
        }

        public void SetDriftResult(float totalPoints, float currentAngle, 
            float driftFactor, float currentDriftPoint, bool isShowDriftResult)
        {
            _totalPoints.text = totalPoints.ToString("###,###,000");
            _currentAngle.text = currentAngle.ToString("###,##0") + "°";
            _currentDriftFactor.text = driftFactor.ToString("###,###,##0.0") + "x"; 
            _currentDriftPoints.text = currentDriftPoint.ToString("###,###,000");

            EnableDriftResultPanel(isShowDriftResult);  
        }
    }
}
