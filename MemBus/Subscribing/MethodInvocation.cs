using System;
using MemBus.Support;

namespace MemBus.Subscribing
{
    /// <summary>
    /// Basic <see cref="ISubscription"/> implementation that wraps a void(Arg a) method
    /// as a subscription
    /// </summary>
    public class MethodInvocation<T> : ISubscription, IKnowsSubscribedInstance
    {
        private readonly Action<T> _action;

        /// <summary>
        /// ctor for any delegate. Can fail with <see cref="InvalidCastException"/>
        /// </summary>
        public MethodInvocation(Delegate action) : this((Action<T>)action)
        {
        }

        /// <summary>
        /// ctor for <see cref="Action{T}"/>
        /// </summary>
        public MethodInvocation(Action<T> action)
        {
            _action = action;
        }

        /// <summary>
        /// <see cref="ISubscription.Push"/>
        /// </summary>
        /// <param name="message"></param>
        public void Push(object message)
        {
            _action((T)message);
        }

        /// <summary>
        /// <see cref="ISubscription.Handles"/>
        /// </summary>
        public bool Handles(Type messageType)
        {
            return messageType.CanBeCastTo<T>();
        }

        object IKnowsSubscribedInstance.Instance
        {
            get
            {
                return _action.ExtractTarget();
            }
        }
    }
}