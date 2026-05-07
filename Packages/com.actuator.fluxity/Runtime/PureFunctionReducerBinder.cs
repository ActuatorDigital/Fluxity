using System;
using System.Reflection;

namespace AIR.Fluxity
{
    public sealed class PureFunctionReducerBinder<TState, TCommand> : IReducer<TState, TCommand>
        where TState : struct
        where TCommand : ICommand
    {
        private const int REDUCER_ARGUMENT_COUNT = 2;
        private readonly IReducer<TState, TCommand>.ReduceDelegate _function;

        public PureFunctionReducerBinder(MethodInfo pureFunctionMethod)
        {
            ValidateMethod(pureFunctionMethod);

            _function = (IReducer<TState, TCommand>.ReduceDelegate)pureFunctionMethod.CreateDelegate(typeof(IReducer<TState, TCommand>.ReduceDelegate));
        }

        public PureFunctionReducerBinder(IReducer<TState, TCommand>.ReduceDelegate reducerDelegate)
            : this(reducerDelegate.Method)
        {
        }

        public Type CommandType => typeof(TCommand);

        public static void ValidateMethod(MethodInfo pureFunctionMethod)
        {
            if (!pureFunctionMethod.IsStatic)
                throw new PureFunctionReducerException($"Expected static got instance. Invalid method on reducer '{pureFunctionMethod.Name}'.");

            var returnType = pureFunctionMethod.ReturnType;
            if (returnType != typeof(TState))
                throw new PureFunctionReducerException($"Expected '{typeof(TState)}' but got '{returnType}'. Invalid return type on reducer '{pureFunctionMethod.Name}'.");

            var reducerArgs = pureFunctionMethod.GetParameters();
            var reducerArgCount = reducerArgs.Length;
            if (reducerArgCount != REDUCER_ARGUMENT_COUNT)
                throw new PureFunctionReducerException($"Expected '{REDUCER_ARGUMENT_COUNT}' but got '{reducerArgCount}'. Invalid argument count on reducer '{pureFunctionMethod.Name}'.");

            var reducerArg0 = reducerArgs[0].ParameterType;
            if (reducerArg0 != typeof(TState))
                throw new PureFunctionReducerException($"Expected '{typeof(TState)}' but got '{reducerArg0}'. Invalid state arg type on reducer '{pureFunctionMethod.Name}'.");

            var reducerArg1 = reducerArgs[1].ParameterType;
            if (reducerArg1 != typeof(TCommand))
                throw new PureFunctionReducerException($"Expected '{typeof(TCommand)}' but got '{reducerArg1}'. Invalid command arg type on reducer '{pureFunctionMethod.Name}'.");
        }

        public TState Reduce(TState state, TCommand command)
            => _function(state, command);

        public TState Reduce(TState state, ICommand command)
            => Reduce(state, (TCommand)command);

        public MethodInfo ReducerBindingInfo()
            => _function?.Method ?? null;
    }

    [Serializable]
    public class PureFunctionReducerException : Exception
    {
        public PureFunctionReducerException(string message)
            : base(message)
        {
        }
    }
}