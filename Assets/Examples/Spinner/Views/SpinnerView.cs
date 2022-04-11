using UnityEngine;

namespace Examples.Spinner
{
    public class SpinnerView : MonoBehaviour
    {
        private bool _isSpinning = false;
        private float _degreesPerSecond = 0;
        public void Update()
        {
            if (!_isSpinning)
                return;

            var rotationDelta = _degreesPerSecond * Time.deltaTime;
            transform.Rotate(0, rotationDelta, 0);
        }

        public void SetSpinRate(float degreesPerSecond) => _degreesPerSecond = degreesPerSecond;
        public void StartSpin() => _isSpinning = true;
        public void StopSpin() => _isSpinning = false;
    }
}