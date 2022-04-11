using AIR.Fluxity;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.Spinner
{
    public class StartSpinButtonView : MonoBehaviour
    {
        [SerializeField] private Text uButtonText;
        [SerializeField] private Button uButton;
        [SerializeField] private float uDegreesPerSecond = 90f;
        private DispatcherHandle _dispatcherHandle;
        public void Start()
        {
            _dispatcherHandle = new DispatcherHandle();
            uButtonText.text = "Start";
            uButton.onClick.AddListener(OnClick);
        }

        public void OnDestroy() => uButton.onClick.RemoveListener(OnClick);
        private void OnClick()
        {
            var command = new StartSpinCommand { DegreesPerSecond = uDegreesPerSecond };
            _dispatcherHandle.Dispatch(command);
        }
    }
}