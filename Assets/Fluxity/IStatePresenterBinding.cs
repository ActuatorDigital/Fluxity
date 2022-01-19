using System;

namespace AIR.Fluxity
{
    public interface IStatePresenterBinding<TState> : IDisposable
        where TState : struct
    {
        TState CurrentState { get; }

        void Inject(IFeature<TState> feature);
    }
}