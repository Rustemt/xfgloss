﻿//-----------------------------------------------------------------------
// <copyright file="EventUtilities.cs" company="MyToolkit">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/MyToolkit/MyToolkit/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

// FYI, only included here because it is referenced by the WeakEvent class

using System;
using System.Reflection;

namespace XFGloss
{
    /// <summary>Provides methods for event management. </summary>
    class EventUtilities
    {
        /// <summary>Registers an event on the given target object. </summary>
        /// <param name="target">The target object. </param>
        /// <param name="eventName">The event name. </param>
        /// <param name="callback">The callback. </param>
        /// <returns>The registration token to deregister the event. </returns>
        public static object RegisterEvent(object target, string eventName, Action<object, object> callback)
        {
            var callbackMethodInfo = callback.GetMethodInfo();
            var eventInfo = target.GetType().GetRuntimeEvent(eventName);
            var callbackDelegate = callbackMethodInfo.CreateDelegate(eventInfo.EventHandlerType, callback.Target);
            return eventInfo.AddMethod.Invoke(target, new object[] { callbackDelegate });
        }

        /// <summary>Registers a static event on the given target object. </summary>
        /// <param name="type">The target type. </param>
        /// <param name="eventName">The event name. </param>
        /// <param name="callback">The callback. </param>
        /// <returns>The registration token to deregister the event. </returns>
        public static object RegisterStaticEvent(Type type, string eventName, Action<object, object> callback)
        {
            var callbackMethodInfo = callback.GetMethodInfo();
            var eventInfo = type.GetRuntimeEvent(eventName);
            var callbackDelegate = callbackMethodInfo.CreateDelegate(eventInfo.EventHandlerType, callback.Target);
            return eventInfo.AddMethod.Invoke(null, new object[] { callbackDelegate });
        }

        /// <summary>Deregisters an event from the target object. </summary>
        /// <param name="target">The target object. </param>
        /// <param name="eventName">The event name. </param>
        /// <param name="token">The registration token. </param>
        public static void DeregisterEvent(object target, string eventName, object token)
        {
            var eventInfo = target.GetType().GetRuntimeEvent(eventName);
            eventInfo.RemoveMethod.Invoke(target, new object[] { token });
        }

        /// <summary>Deregisters a static event from the target type. </summary>
        /// <param name="type">The target type. </param>
        /// <param name="eventName">The event name. </param>
        /// <param name="token">The registration token. </param>
        public static void DeregisterStaticEvent(Type type, string eventName, object token)
        {
            var eventInfo = type.GetRuntimeEvent(eventName);
            eventInfo.RemoveMethod.Invoke(null, new object[] { token });
        }
    }
}