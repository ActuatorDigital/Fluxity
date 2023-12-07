using System;

namespace AIR.Fluxity
{
    public interface IFeaturePresenterBinding<TState> : IDisposable
        where TState : struct
    {
        TState CurrentState { get; }

        void Inject(IStore store);
    }
}