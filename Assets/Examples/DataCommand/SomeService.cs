using UnityEngine;

namespace Examples.DataCommand
{
    public interface ISomeService
    {
        void DoSomething(string v);
    }

    public class SomeService : ISomeService
    {
        public void DoSomething(string v)
        {
            Debug.Log(v);
        }
    }
}