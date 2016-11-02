using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace MemBus.Support
{
    public static class RespondExtension
    {
        public static bool TryInvoke(this object target, Action<dynamic> selector)
        {
            var dynExists = new DynamicExists(target);
            selector(dynExists);
            if (dynExists.OperationExists)
                selector(target);
            return dynExists.OperationExists;
        }

        public static bool RespondsTo(this object target, Func<dynamic,object> selector)
        {
            var dynExists = new DynamicExists(target);
            selector(dynExists);
            return dynExists.OperationExists;
        }

        [Api]
        public static bool RespondsTo(this object target, Action<dynamic> selector)
        {
            var dynExists = new DynamicExists(target);
            selector(dynExists);
            return dynExists.OperationExists;
        }

        private class DynamicExists : DynamicObject
        {
            private static readonly Dictionary<string,bool> Cache = new Dictionary<string, bool>();
            private static readonly ReaderWriterLockSlim RwLock = new ReaderWriterLockSlim();
            private readonly object _instance;
            private bool _workingWithEvent;

            public DynamicExists(object instance)
            {
                _instance = instance;
            }

            public bool OperationExists { get; private set; }

            public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
            {
                var possibleMembers = from mi in FindByName(binder.Name)
                    let parms = mi.GetParameters()
                    where parms.Length == args.Length
                    let correlates = parms.Zip(args, 
                      (pi, o) => (o == null && pi.ParameterType.GetTypeInfo().IsClass) || 
                                 ( o != null && o.GetType().CanBeCastTo(pi.ParameterType)))
                                 .All(truth => truth)
                    where correlates
                    select mi;

                OperationExists = possibleMembers.Count() == 1; //Expecting 1 or none as method overloading rules would be violated
                result = null;

                return true; // We always "find" a member
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                if (!_workingWithEvent) //TryGetMember is called before set when working with an event
                    WorkWithSetter(binder, value);
                return true; // We always "find" a member
            }


            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var runtimeEvent = _instance.GetType().GetRuntimeEvent(binder.Name);
                if (runtimeEvent != null)
                {
                    OperationExists = true;
                    result = runtimeEvent;
                    _workingWithEvent = true;
                }
                else
                {
                    OperationExists = _instance.GetType().GetRuntimeProperty(binder.Name) != null;
                    result = 1; //The int will support a possible += operator of an event that was searched but not found.
                }
                return true; // We always "find" a member
            }

            private void WorkWithSetter(SetMemberBinder binder, object value)
            {
                try
                {
                    var key = _instance.GetType().FullName + binder.Name + (value != null ? value.GetType().FullName : "null");
                    RwLock.EnterUpgradeableReadLock();
                    if (!Cache.ContainsKey(key))
                    {
                        try
                        {
                            var exists = (from pi in _instance.GetType().GetRuntimeProperties()
                                            where pi.Name == binder.Name && pi.SetMethod != null && pi.SetMethod.IsPublic &&
                                                (
                                                    (pi.PropertyType.GetTypeInfo().IsClass && value == null) ||
                                                    (value != null && value.GetType().CanBeCastTo(pi.PropertyType))
                                                ) select pi).Count() == 1;
                            RwLock.EnterWriteLock();
                            Cache[key] = exists;
                        }
                        finally
                        {
                            RwLock.ExitWriteLock();
                        }
                    }
                    OperationExists = Cache[key];
                }
                finally
                {
                    RwLock.ExitUpgradeableReadLock();
                }                
            }

            private IEnumerable<MethodInfo> FindByName(string  name)
            {
                return _instance.GetType().GetRuntimeMethods().Where(m => m.Name == name);
            }
        }
    }
}