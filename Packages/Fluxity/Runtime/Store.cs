using System;
using System.Collections.Generic;
using System.Linq;

namespace AIR.Fluxity
{
    public sealed class Store : IStore
    {
        private readonly Dictionary<Type, IFeature> _features = new();
        private readonly Dictionary<Type, IActionSet> _actionSetPre = new();
        private readonly Dictionary<Type, IActionSet> _actionSetPost = new();

        public IReadOnlyCollection<IFeature> GetAllFeatures()
        {
            return _features.Values;
        }

        public void AddFeature<TState>(IFeature<TState> feature)
            where TState : struct
        {
            _features[feature.GetStateType] = feature;
        }

        public void RegisterReducer<TState, TCommand>(IReducer<TState, TCommand> reducer)
            where TState : struct
            where TCommand : ICommand
        {
            _features[typeof(TState)].Register(reducer);
        }

        public void AddPreAction<TCommand>(Action<TCommand> action)
            where TCommand : ICommand
        {
            var type = typeof(TCommand);
            if (!_actionSetPre.ContainsKey(type))
            {
                _actionSetPre[type] = new ActionSet<TCommand>();
            }
            var actionSet = (ActionSet<TCommand>)_actionSetPre[type];
            actionSet.AddAction(action);
        }

        public void AddPostAction<TCommand>(Action<TCommand> action)
            where TCommand : ICommand
        {
            var type = typeof(TCommand);
            if (!_actionSetPost.ContainsKey(type))
            {
                _actionSetPost[type] = new ActionSet<TCommand>();
            }
            var actionSet = (ActionSet<TCommand>)_actionSetPost[type];
            actionSet.AddAction(action);
        }

        public void ProcessCommand<TCommand>(TCommand command)
          where TCommand : ICommand
        {
            if(_actionSetPre.TryGetValue(command.GetType(), out var preActionSet))
                preActionSet.Invoke(command);

            foreach (var feature in _features.Values)
            {
                feature.ProcessReducers(command);
            }

            if (_actionSetPost.TryGetValue(command.GetType(), out var postActionSet))
                postActionSet.Invoke(command);
        }

        public IFeatureObservable<TState> GetFeatureObservable<TState>()
            where TState : struct
        {
            return (IFeatureObservable<TState>)_features[typeof(TState)];
        }

        internal interface IActionSet 
        {
            void Invoke(ICommand command);
        }

        internal interface IActionSet<TCommand> : IActionSet
            where TCommand : ICommand
        {
            void Invoke(TCommand command);
        }

        internal sealed class ActionSet<TCommand> : IActionSet<TCommand>
            where TCommand : ICommand
        {
            private readonly List<Action<TCommand>> _actions = new();
            public void AddAction(Action<TCommand> action)
            {
                _actions.Add(action);
            }

            public void Invoke(ICommand command)
            {
                Invoke((TCommand)command);
            }

            public void Invoke(TCommand command)
            {
                foreach (var action in _actions)
                {
                    action(command);
                }
            }
        }
    }
}