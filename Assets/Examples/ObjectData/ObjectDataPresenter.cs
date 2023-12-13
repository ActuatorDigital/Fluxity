using AIR.Fluxity;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.ObjectData
{
    public class ObjectDataPresenter : Presenter
    {
        [SerializeField] private Text uObjectStateText;
        [SerializeField] private Text uObjectCountText;

        private IFeaturePresenterBinding<ObjectDataState> _objectDataStateBinding;

        public override void CreateBindings()
        {
            _objectDataStateBinding = Bind<ObjectDataState>();
        }

        public override void Display()
        {
            uObjectStateText.text = string.Join(System.Environment.NewLine, _objectDataStateBinding.CurrentState.Guids);
            uObjectCountText.text = $"Count: {_objectDataStateBinding.CurrentState.Guids.Count}";
        }

        protected override void SetUp()
        {
        }
    }
}