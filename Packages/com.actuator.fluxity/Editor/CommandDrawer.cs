using UnityEditor;

namespace AIR.Fluxity.Editor
{
    internal class CommandDrawer
    {
        private readonly ICommand _target;
        private readonly ObjectWalker _walker;

        public CommandDrawer(ICommand target)
        {
            _target = target;
            _walker = new ObjectWalker();
        }

        public void Draw()
        {
            _walker.Prepare();
            EditorGUI.BeginDisabledGroup(true);
            _walker.DrawObject(_target, 0);
            EditorGUI.EndDisabledGroup();
        }
    }
}