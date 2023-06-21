using UnityEditor;
using System.Reflection;

namespace AIR.Fluxity.Editor
{
    internal sealed class FeatureStateDrawer
    {
        private readonly IFeature _targetFeature;
        private readonly PropertyInfo _stateGetter;
        private readonly ObjectWalker _walker;

        public FeatureStateDrawer(IFeature targetFeat, string name)
        {
            Name = name;
            _targetFeature = targetFeat;
            _stateGetter = _targetFeature.GetType().GetProperty("State");
            _walker = new ObjectWalker();
        }

        public string Name { get; internal set; }

        public void Draw()
        {
            var actualState = _stateGetter.GetValue(_targetFeature);
            _walker.Prepare();
            EditorGUI.BeginDisabledGroup(true);
            _walker.DrawObject(actualState, 0);
            EditorGUI.EndDisabledGroup();
        }
    }
}