using System.Threading.Tasks;
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

        public override Task DoEffect(ChangeCountCommand command)
        {
            Debug.Log($"Count changed by {command.Delta}.");
            return Task.CompletedTask;
        }
    }
}