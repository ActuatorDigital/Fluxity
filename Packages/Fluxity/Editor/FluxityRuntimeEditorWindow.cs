using AIR.Flume;
using UnityEditor;

namespace AIR.Fluxity.Editor
{
    internal class FluxityRuntimeEditorWindow : EditorWindow
    {
        internal class FluxityHandle : Dependent
        {
            public IStore Store { get; private set; }
            public IDispatcher Dispatcher { get; private set; }

            public void Inject(
                IStore store,
                IDispatcher dispatcher)
            {
                Store = store;
                Dispatcher = dispatcher;
            }
        }

        protected FluxityHandle _fluxityHandle;

        public virtual void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        public virtual void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        protected virtual void Refresh()
        {
            _fluxityHandle = null;
        }

        protected virtual IStore GetStore()
        {
            if (_fluxityHandle == null)
            {
                if (EditorApplication.isPlaying)
                {
                    _fluxityHandle = new FluxityHandle();
                }
            }

            return _fluxityHandle?.Store;
        }

        protected virtual IDispatcher GetDispatcher()
        {
            if (_fluxityHandle == null)
            {
                if (EditorApplication.isPlaying)
                {
                    _fluxityHandle = new FluxityHandle();
                }
            }

            return _fluxityHandle?.Dispatcher;
        }

        protected virtual void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            Refresh();
            Repaint();
        }
    }
}