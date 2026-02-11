using AOT;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BananaParty.Input.TVRemote
{
    public static class WebInputBridge
    {
        private static readonly Dictionary<InputKey, Queue<PressEvent>> _pressEventQueues = new();
        private static readonly Dictionary<InputKey, Queue<ReleaseEvent>> _releaseEventQueues = new();

#if UNITY_WEBGL && !UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity InitializeOnLoadMethod")]
        private static void Initialize()
        {
            WebInputBridgeInitialize(OnButtonPress, OnButtonRelease);
        }

        [DllImport("__Internal")]
        private static extern void WebInputBridgeInitialize(Action<int, int> onButtonPressCallback, Action<int, int> onButtonReleaseCallback);

        public static void RegisterButton(WebInputDeviceType webInputDeviceType, int webKeyCode)
        {
            var key = new InputKey(webInputDeviceType, webKeyCode);
            _pressEventQueues[key] = new Queue<PressEvent>();
            _releaseEventQueues[key] = new Queue<ReleaseEvent>();
            WebInputBridgeRegisterButton((int)webInputDeviceType, webKeyCode);
        }

        [DllImport("__Internal")]
        private static extern void WebInputBridgeRegisterButton(int webInputDeviceType, int webKeyCode);

        public static void PollInput()
        {
            WebInputBridgePollInput();
        }

        [DllImport("__Internal")]
        private static extern void WebInputBridgePollInput();

        public static bool HasUnreadPressEvents(WebInputDeviceType webInputDeviceType, int webKeyCode)
        {
            var key = new InputKey(webInputDeviceType, webKeyCode);
            return _pressEventQueues[key].Count > 0;
        }

        public static bool HasUnreadReleaseEvents(WebInputDeviceType webInputDeviceType, int webKeyCode)
        {
            var key = new InputKey(webInputDeviceType, webKeyCode);
            return _releaseEventQueues[key].Count > 0;
        }

        public static PressEvent ReadPressEvents(WebInputDeviceType webInputDeviceType, int webKeyCode)
        {
            var key = new InputKey(webInputDeviceType, webKeyCode);
            return _pressEventQueues[key].Dequeue();
        }

        public static ReleaseEvent ReadReleaseEvents(WebInputDeviceType webInputDeviceType, int webKeyCode)
        {
            var key = new InputKey(webInputDeviceType, webKeyCode);
            return _releaseEventQueues[key].Dequeue();
        }

        [MonoPInvokeCallback(typeof(Action<int, int>))]
        private static void OnButtonPress(int webInputDeviceType, int webKeyCode)
        {
            var key = new InputKey((WebInputDeviceType)webInputDeviceType, webKeyCode);
            _pressEventQueues[key].Enqueue(new PressEvent(Time.realtimeSinceStartup));
        }

        [MonoPInvokeCallback(typeof(Action<int, int>))]
        private static void OnButtonRelease(int webInputDeviceType, int webKeyCode)
        {
            var key = new InputKey((WebInputDeviceType)webInputDeviceType, webKeyCode);
            _releaseEventQueues[key].Enqueue(new ReleaseEvent(Time.realtimeSinceStartup));
        }

        private readonly struct InputKey
        {
            public readonly WebInputDeviceType DeviceType;
            public readonly int KeyCode;

            public InputKey(WebInputDeviceType deviceType, int keyCode)
            {
                DeviceType = deviceType;
                KeyCode = keyCode;
            }
        }
    }
}
