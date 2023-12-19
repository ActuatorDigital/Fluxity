using AIR.Fluxity;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.ObjectData
{
    public class ObjectDataPresenter : Presenter
    {
        [SerializeField] private Text uObjectStateText;
        [SerializeField] private Text uObjectCountText;

        private IFeatureView<ObjectDataState> _objectDataStateBinding;

        public override void CreateBindings()
        {
            _objectDataStateBinding = Bind<ObjectDataState>();
        }

        public override void Display()
        {
            uObjectStateText.text = string.Join(System.Environment.NewLine, _objectDataStateBinding.State.Guids);
            uObjectCountText.text = $"Count: {_objectDataStateBinding.State.Guids.Count}";
        }
    }
}