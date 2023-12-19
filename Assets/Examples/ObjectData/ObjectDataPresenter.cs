using AIR.Fluxity;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.ObjectData
{
    public class ObjectDataPresenter : MonoBehaviour
    {
        [SerializeField] private Text uObjectStateText;
        [SerializeField] private Text uObjectCountText;

        private FeatureObserver<ObjectDataState> _objectDataState = new();

        public void Start()
        {
            _objectDataState.OnStateChanged += Display;
        }

        public void OnDestroy()
        {
            _objectDataState.OnStateChanged -= Display;
        }

        private void Display(ObjectDataState state)
        {
            uObjectStateText.text = string.Join(System.Environment.NewLine, state.Guids);
            uObjectCountText.text = $"Count: {state.Guids.Count}";
        }
    }
}