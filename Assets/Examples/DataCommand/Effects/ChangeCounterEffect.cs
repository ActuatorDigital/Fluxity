using AIR.Fluxity;
using UnityEngine;

namespace Examples.DataCommand
{
    public class ChangeCounterEffect : Effect<ChangeCountCommand>
    {
        public ChangeCounterEffect(IDispatcher dispatcher)
            : base(dispatcher)
        {
        }

        public override void DoEffect(ChangeCountCommand command)
        {
            Debug.Log($"Count changed by {command.Delta}.");
        }
    }
}