﻿using System;

namespace MemBus
{
    /// <summary>
    /// Helps you in separating a possibly complex Rx-based Observable query 
    /// to its own class
    /// </summary>
    public class RxEnabledObservable<M> : IObservable<M>
    {
        private readonly IBus _bus;

        public RxEnabledObservable(IBus bus)
        {
            _bus = bus;
        }

        public IDisposable Subscribe(IObserver<M> observer)
        {
            if (observer == null) throw new ArgumentNullException("observer");
            var o = constructObservable(new MessageObservable<M>(_bus));
            return o.Subscribe(observer);
        }

        protected virtual IObservable<M> constructObservable(IObservable<M> startingPoint)
        {
            return startingPoint;
        }
    }
}