using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Examples.StoresWindow
{
    // We are going to make a few States of varrying complexity so you can 
    //  demo them in the Fluxity->Stores EditorWindow.
    public struct ListOfIntsState
    {
        public static ListOfIntsState CreateDefault() => new ListOfIntsState { ints = Enumerable.Range(0, 10).ToList() };

        public List<int> ints;
    }

    public struct SimpleState
    {
        public static SimpleState CreateDefault() => new SimpleState
        {
            valueStr = "Simple State",
            valueF = 3.14f,
            valueI = -1,
            valueV3 = Random.onUnitSphere,
        };

        public float valueF;
        public int valueI;
        public string valueStr;
        public Vector3 valueV3;
    }

    public struct TransformsState
    {
        public static TransformsState CreateDefault() => new TransformsState { transforms = Object.FindObjectsOfType<Transform>() };

        public Transform[] transforms;
    }

    public struct CustomObjectCyclicState
    {
        public class SomeObject
        {
            public string name;
            public SomeObject child;
            public Transform[] transforms;
        }

        public static CustomObjectCyclicState CreateDefault()
        {
            var obj = new SomeObject
            {
                name = "Root",
                transforms = Object.FindObjectsOfType<Transform>(),
            };

            obj.child = new SomeObject
            {
                name = "Child",
                transforms = Object.FindObjectsOfType<Transform>(),
                child = new SomeObject
                {
                    name = "GrandChild",
                    transforms = Object.FindObjectsOfType<Transform>(),
                    child = new SomeObject
                    {
                        name = "Cycle",
                        transforms = Object.FindObjectsOfType<Transform>(),
                        child = obj,
                    },
                }
            };

            return new CustomObjectCyclicState { someObject = obj };
        }

        public SomeObject someObject;
    }
}