using UnityEngine;

namespace BananaParty.Input.TVRemote
{
    public class TVRemoteButton
    {
        private readonly KeyCode _unityKeyCode;
        private readonly int _webKeyCode;
        private readonly WebInputDeviceType _webInputDeviceType;

        public readonly EventHub<PressEvent> PressEventHub = new();
        public readonly EventHub<ReleaseEvent> ReleaseEventHub = new();

        public bool IsHeld { get; private set; }

        public TVRemoteButton(WebInputDeviceType webInputDeviceType, int webKeyCode, KeyCode unityKeyCode)
        {
            _unityKeyCode = unityKeyCode;
            _webKeyCode = webKeyCode;
            _webInputDeviceType = webInputDeviceType;

            if (TVRemote.IsRunningOnWeb)
                WebInputBridge.RegisterButton(webInputDeviceType, webKeyCode);
        }

        public void PollInput()
        {
            if (TVRemote.IsRunningOnWeb)
            {
                while (WebInputBridge.HasUnreadPressEvents(_webInputDeviceType, _webKeyCode))
                {
                    PressEventHub.AddEvent(WebInputBridge.ReadPressEvents(_webInputDeviceType, _webKeyCode));
                    IsHeld = true;
                }

                while (WebInputBridge.HasUnreadReleaseEvents(_webInputDeviceType, _webKeyCode))
                {
                    ReleaseEventHub.AddEvent(WebInputBridge.ReadReleaseEvents(_webInputDeviceType, _webKeyCode));
                    IsHeld = false;
                }
            }
            else
            {
                if (UnityEngine.Input.GetKeyDown(_unityKeyCode))
                {
                    PressEventHub.AddEvent(new PressEvent(Time.realtimeSinceStartup));
                    IsHeld = true;
                }

                if (UnityEngine.Input.GetKeyUp(_unityKeyCode))
                {
                    ReleaseEventHub.AddEvent(new ReleaseEvent(Time.realtimeSinceStartup));
                    IsHeld = false;
                }
            }
        }
    }
}
