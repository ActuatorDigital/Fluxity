using System.Reflection;

namespace AIR.Fluxity
{
    public interface IEffect<TCommand> : IEffect
        where TCommand : ICommand
    {
        public delegate void EffectDelegate(TCommand command, IDispatcher dispatcher);

        void DoEffect(TCommand command, IDispatcher dispatcher);
    }

    public interface IEffect
    {
        void DoEffect(ICommand command, IDispatcher dispatcher);
        MethodInfo EffectBindingInfo();
    }
}