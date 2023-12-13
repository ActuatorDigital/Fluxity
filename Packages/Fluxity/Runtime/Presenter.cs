using System;
using System.Collections.Generic;
using AIR.Flume;

namespace AIR.Fluxity
{
    public abstract class Presenter : DependentBehaviour, IPresenter
    {
        private readonly List<IDisposable> _bindings = new();

        public virtual void Start()
        {
            CreateBindings();
            SetUp();
            Display();
        }

        public abstract void CreateBindings();

        public abstract void Display();

        public FeatureBinding<TState> Bind<TState>()
            where TState : struct
        {
            var newBinding = new FeatureBinding<TState>();
            newBinding.OnStateChanged += x => Display();
            _bindings.Add(newBinding);
            return newBinding;
        }

        protected virtual void SetUp() { }

        public virtual void OnDestroy()
        {
            foreach (var item in _bindings)
            {
                item.Dispose();
            }
        }
    }
}