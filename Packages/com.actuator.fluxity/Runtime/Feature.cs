using System;
using System.Collections.Generic;

namespace AIR.Fluxity
{
    public sealed class Feature<TState> : IFeature<TState>
        where TState : struct
    {
        private readonly Dictionary<Type, List<IReducer<TState>>> _reducers = new();

        public Feature(TState state)
        {
            State = state;
        }

        public event Action<TState> OnStateChanged;

        public TState State { get; private set; }
        public Type GetStateType => typeof(TState);
        public IReadOnlyCollection<Type> GetAllHandledCommandTypes() => _reducers.Keys;
        public IReadOnlyCollection<IReducer> GetAllReducersForCommand(Type commandType) => _reducers[commandType].AsReadOnly();

        public void SetState(TState newState)
        {
            State = newState;
            OnStateChanged?.Invoke(State);
        }

        public void Register(IReducer<TState> reducer)
        {
            if (_reducers.TryGetValue(reducer.CommandType, out var extantReducers))
            {
                extantReducers.Add(reducer);
                return;
            }

            _reducers.Add(reducer.CommandType, new List<IReducer<TState>>() { reducer });
        }

        public void Register(IReducer reducer)
            => Register((IReducer<TState>)reducer);

        public void ProcessReducers(ICommand command)
        {
            if (_reducers.TryGetValue(command.GetType(), out var extantReducers))
            {
                var state = State;
                foreach (var reducer in extantReducers)
                {
                    state = reducer.Reduce(state, command);
                }

                SetState(state);
            }
        }
    }
}