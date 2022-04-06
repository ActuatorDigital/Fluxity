﻿using System;
using System.Threading.Tasks;

namespace AIR.Fluxity.Tests.DummyTypes
{
    internal class DummyDelegateEffect : Effect<DummyCommand>
    {
        private Action _customAction;

        public DummyDelegateEffect(Action customAction, IDispatcher dispatcher)
            : base(dispatcher)
        {
            _customAction = customAction;
        }

        public override Task DoEffect(DummyCommand command)
        {
            _customAction?.Invoke();
            return Task.CompletedTask;
        }
    }
}