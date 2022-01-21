using System;
using System.Collections.Generic;
using AIR.Flume;

namespace AIR.Fluxity
{
    public class Feature<TState> : Dependent, IFeature<TState>
        where TState : struct
    {
        private readonly Dictionary<Type, List<IReducer<TState>>> _reducers = new Dictionary<Type, List<IReducer<TState>>>();

        public event Action<TState> OnStateChanged;

        public TState State { get; private set; }

        public Type GetStateType => typeof(TState);

        public void Inject(IStore store)
            => store.AddFeature(this);

        public void SetState(TState newState)
        {
            State = newState;
            OnStateChanged?.Invoke(State);
        }

        public void Register(IReducer<TState> reducer)
        {
            if (_reducers.TryGetValue(reducer.GetCommandType, out var extantReducers))
            {
                extantReducers.Add(reducer);
                return;
            }

            _reducers.Add(reducer.GetCommandType, new List<IReducer<TState>>() { reducer });
        }

        public void ProcessReducers<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            if (_reducers.TryGetValue(typeof(TCommand), out var extantReducers))
            {
                var state = State;
                foreach (var reducer in extantReducers)
                {
                    state = reducer.Reduce(state, command);
                }

                SetState(state);
            }
        }

        public void Register(IReducer reducer)
            => Register((IReducer<TState>)reducer);
    }
}