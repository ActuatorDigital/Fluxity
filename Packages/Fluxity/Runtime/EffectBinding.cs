using System.Reflection;

namespace AIR.Fluxity
{
    public sealed class EffectBinding<TCommand> : IEffect<TCommand>
        where TCommand : ICommand
    {
        private readonly IEffect<TCommand>.EffectDelegate _effectDel;

        public EffectBinding(IEffect<TCommand>.EffectDelegate effectDef)
        {
            _effectDel = effectDef;
        }

        public void DoEffect(TCommand command, IDispatcher dispatcher)
            => _effectDel?.Invoke(command, dispatcher);

        public void DoEffect(ICommand command, IDispatcher dispatcher)
            => DoEffect((TCommand)command, dispatcher);

        public MethodInfo EffectBindingInfo()
            => _effectDel?.Method ?? null;
    }
}