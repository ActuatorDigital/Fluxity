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

    public struct CommonTypesState
    {
        public static CommonTypesState CreateDefault() => new CommonTypesState
        {
            dateTime = System.DateTime.Now,
            guid = System.Guid.NewGuid(),
            timeSpan = System.TimeSpan.FromMinutes(5),
        };

        public System.Guid guid;
        public System.DateTime dateTime;
        public System.TimeSpan timeSpan;
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
            public Transform[] transforms;
            public System.Guid guid;
            public SomeObject child;
        }

        public static CustomObjectCyclicState CreateDefault()
        {
            var obj = new SomeObject
            {
                name = "Root",
                guid = System.Guid.NewGuid(),
                transforms = Object.FindObjectsOfType<Transform>(),
            };

            obj.child = new SomeObject
            {
                name = "Child",
                guid = System.Guid.NewGuid(),
                transforms = Object.FindObjectsOfType<Transform>(),
                child = new SomeObject
                {
                    name = "GrandChild",
                    guid = System.Guid.NewGuid(),
                    transforms = Object.FindObjectsOfType<Transform>(),
                    child = new SomeObject
                    {
                        name = "Cycle",
                        guid = System.Guid.NewGuid(),
                        transforms = Object.FindObjectsOfType<Transform>(),
                        child = obj,
                    },
                },
            };

            return new CustomObjectCyclicState { someObject = obj };
        }

        public SomeObject someObject;
    }
}