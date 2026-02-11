using UnityEngine;
using UnityEngine.LowLevel;

namespace BananaParty.Input.TVRemote
{
    public static class TVRemote
    {
        public static readonly TVRemoteButton OkButton = new(WebInputDeviceType.Keyboard, 13, KeyCode.JoystickButton0);
        public static readonly TVRemoteButton UpButton = new(WebInputDeviceType.Gamepad, 12, KeyCode.JoystickButton12);
        public static readonly TVRemoteButton DownButton = new(WebInputDeviceType.Gamepad, 13, KeyCode.JoystickButton13);
        public static readonly TVRemoteButton LeftButton = new(WebInputDeviceType.Gamepad, 14, KeyCode.JoystickButton14);
        public static readonly TVRemoteButton RightButton = new(WebInputDeviceType.Gamepad, 15, KeyCode.JoystickButton15);

        public static bool IsRunningOnWeb
        {
            get
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity InitializeOnLoadMethod")]
        private static void Initialize()
        {
            InjectPollInputIntoPlayerLoop();
        }

        private static void PollInput()
        {
            if (IsRunningOnWeb)
                WebInputBridge.PollInput();
            
            OkButton.PollInput();
            UpButton.PollInput();
            DownButton.PollInput();
            LeftButton.PollInput();
            RightButton.PollInput();
        }

        private static void InjectPollInputIntoPlayerLoop()
        {
            PlayerLoopSystem loop = PlayerLoop.GetCurrentPlayerLoop();
            PlayerLoopSystem[] root = loop.subSystemList;
            if (root == null) return;

            int insertIndex = -1;
            for (int i = 0; i < root.Length; i++)
            {
                if (root[i].type != null && root[i].type.Name == "Update")
                {
                    insertIndex = i;
                    break;
                }
            }
            if (insertIndex < 0) return;

            var newList = new PlayerLoopSystem[root.Length + 1];
            for (int i = 0; i < insertIndex; i++)
                newList[i] = root[i];
            newList[insertIndex] = new PlayerLoopSystem
            {
                type = typeof(TVRemote),
                updateDelegate = PollInput
            };
            for (int i = insertIndex; i < root.Length; i++)
                newList[i + 1] = root[i];

            loop.subSystemList = newList;
            PlayerLoop.SetPlayerLoop(loop);
        }
    }
}
