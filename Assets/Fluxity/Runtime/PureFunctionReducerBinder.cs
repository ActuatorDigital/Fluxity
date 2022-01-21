using System.Reflection;

namespace AIR.Fluxity
{
    public class PureFunctionReducerBinder<TState, TCommand> : Reducer<TState, TCommand>
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

        public override TState Reduce(TState state, TCommand command)
            => _function(state, command);

        public static void ValidateMethod(MethodInfo pureFunctionMethod)
        {
            if (!pureFunctionMethod.IsStatic)
                throw new ReducerException($"Expected static got instance. Invalid method on reducer '{pureFunctionMethod.Name}'.");

            var returnType = pureFunctionMethod.ReturnType;
            if (returnType != typeof(TState))
                throw new ReducerException($"Expected '{typeof(TState)}' but got '{returnType}'. Invalid return type on reducer '{pureFunctionMethod.Name}'.");

            var reducerArgs = pureFunctionMethod.GetParameters();
            var reducerArgCount = reducerArgs.Length;
            if (reducerArgCount != REDUCER_ARGUMENT_COUNT)
                throw new ReducerException($"Expected '{REDUCER_ARGUMENT_COUNT}' but got '{reducerArgCount}'. Invalid argument count on reducer '{pureFunctionMethod.Name}'.");

            var reducerArg0 = reducerArgs[0].ParameterType;
            if (reducerArg0 != typeof(TState))
                throw new ReducerException($"Expected '{typeof(TState)}' but got '{reducerArg0}'. Invalid state arg type on reducer '{pureFunctionMethod.Name}'.");

            var reducerArg1 = reducerArgs[1].ParameterType;
            if (reducerArg1 != typeof(TCommand))
                throw new ReducerException($"Expected '{typeof(TCommand)}' but got '{reducerArg1}'. Invalid command arg type on reducer '{pureFunctionMethod.Name}'.");
        }
    }
}