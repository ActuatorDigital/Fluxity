using System.Reflection;

namespace AIR.Fluxity
{
    public interface IEffect<TCommand> : IEffect
        where TCommand : ICommand
    {
        public new delegate void EffectDelegate(TCommand command, IDispatcher dispatcher);

        void DoEffect(TCommand command, IDispatcher dispatcher);
    }

    public interface IEffect
    {
        public delegate void EffectDelegate(ICommand command, IDispatcher dispatcher);

        void DoEffect(ICommand command, IDispatcher dispatcher);
        MethodInfo EffectBindingInfo();
    }
}