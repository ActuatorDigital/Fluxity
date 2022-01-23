using System;
using System.Collections.Generic;
using AIR.Flume;

namespace AIR.Fluxity
{
    public abstract class Presenter : DependentBehaviour, IPresenter
    {
        private readonly List<IDisposable> _bindings = new List<IDisposable>();

        public virtual void Start()
        {
            CreateBindings();
            Display();
        }

        public abstract void Display();

        public abstract void CreateBindings();

        public IFeaturePresenterBinding<TState> Bind<TState>()
            where TState : struct
        {
            var newBinding = new FeaturePresenterBinding<TState>(this);
            _bindings.Add(newBinding);
            return newBinding;
        }

        public virtual void OnDestroy()
        {
            foreach (var item in _bindings)
            {
                item.Dispose();
            }
        }
    }
}