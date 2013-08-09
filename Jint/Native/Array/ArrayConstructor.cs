﻿using System;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Runtime.Descriptors;
using Jint.Runtime.Descriptors.Specialized;
using Jint.Runtime.Interop;

namespace Jint.Native.Array
{
    public class ArrayConstructor : FunctionInstance
    {
        private readonly Engine _engine;

        public ArrayConstructor(Engine engine) :  base(engine, new ObjectInstance(engine.RootFunction), null, null)
        {
            _engine = engine;

            // the constructor is the function constructor of an object
            this.Prototype.DefineOwnProperty("constructor", new DataDescriptor(this) { Writable = true, Enumerable = false, Configurable = false }, false);
            this.Prototype.DefineOwnProperty("prototype", new DataDescriptor(this.Prototype) { Writable = true, Enumerable = false, Configurable = false }, false);

            // Array method
            this.Prototype.DefineOwnProperty("push", new DataDescriptor(new BuiltInPropertyWrapper(engine, (Action<ArrayInstance, object>)Push, engine.RootFunction)), false);
        }

        public override dynamic Call(object thisObject, dynamic[] arguments)
        {
            return Construct(arguments);
        }

        public virtual ObjectInstance Construct(dynamic[] arguments)
        {
            var instance = new ArrayInstance(Prototype);

            instance.DefineOwnProperty("length", new DataDescriptor((double)0), false);

            foreach (var arg in arguments)
            {
                instance.Push(arg);
            }

            return instance;
        }

        private static void Push(ArrayInstance thisObject, object o)
        {
            thisObject.Push(o);
            thisObject.Set("length", (double)thisObject.Get("length") + 1);
        }
    }
}
