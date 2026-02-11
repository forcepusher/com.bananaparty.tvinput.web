using UnityEngine;
using UnityEngine.UI;

namespace BananaParty.Input.TVRemote.Sample
{
    public class ButtonStateLog : MonoBehaviour
    {
        [SerializeField]
        private Text _text;

        private string _eventLog;

        private EventQueue<PressEvent> _okButtonPressEventQueue;
        private EventQueue<ReleaseEvent> _okButtonReleaseEventQueue;

        private EventQueue<PressEvent> _upButtonPressEventQueue;
        private EventQueue<ReleaseEvent> _upButtonReleaseEventQueue;

        private EventQueue<PressEvent> _downButtonPressEventQueue;
        private EventQueue<ReleaseEvent> _downButtonReleaseEventQueue;

        private EventQueue<PressEvent> _leftButtonPressEventQueue;
        private EventQueue<ReleaseEvent> _leftButtonReleaseEventQueue;

        private EventQueue<PressEvent> _rightButtonPressEventQueue;
        private EventQueue<ReleaseEvent> _rightButtonReleaseEventQueue;

        private void OnEnable()
        {
            _okButtonPressEventQueue = TVRemote.OkButton.PressEventHub.Subscribe();
            _okButtonReleaseEventQueue = TVRemote.OkButton.ReleaseEventHub.Subscribe();

            _upButtonPressEventQueue = TVRemote.UpButton.PressEventHub.Subscribe();
            _upButtonReleaseEventQueue = TVRemote.UpButton.ReleaseEventHub.Subscribe();

            _downButtonPressEventQueue = TVRemote.DownButton.PressEventHub.Subscribe();
            _downButtonReleaseEventQueue = TVRemote.DownButton.ReleaseEventHub.Subscribe();

            _leftButtonPressEventQueue = TVRemote.LeftButton.PressEventHub.Subscribe();
            _leftButtonReleaseEventQueue = TVRemote.LeftButton.ReleaseEventHub.Subscribe();

            _rightButtonPressEventQueue = TVRemote.RightButton.PressEventHub.Subscribe();
            _rightButtonReleaseEventQueue = TVRemote.RightButton.ReleaseEventHub.Subscribe();
        }

        private void OnDisable()
        {
            TVRemote.OkButton.PressEventHub.Unsubscribe(_okButtonPressEventQueue);
            TVRemote.OkButton.ReleaseEventHub.Unsubscribe(_okButtonReleaseEventQueue);

            TVRemote.UpButton.PressEventHub.Unsubscribe(_upButtonPressEventQueue);
            TVRemote.UpButton.ReleaseEventHub.Unsubscribe(_upButtonReleaseEventQueue);

            TVRemote.DownButton.PressEventHub.Unsubscribe(_downButtonPressEventQueue);
            TVRemote.DownButton.ReleaseEventHub.Unsubscribe(_downButtonReleaseEventQueue);

            TVRemote.LeftButton.PressEventHub.Unsubscribe(_leftButtonPressEventQueue);
            TVRemote.LeftButton.ReleaseEventHub.Unsubscribe(_leftButtonReleaseEventQueue);

            TVRemote.RightButton.PressEventHub.Unsubscribe(_rightButtonPressEventQueue);
            TVRemote.RightButton.ReleaseEventHub.Unsubscribe(_rightButtonReleaseEventQueue);
        }

        // Yes, FixedUpdate with input is intentional
        private void FixedUpdate()
        {
            while (_okButtonPressEventQueue.HasUnreadEvents)
                _eventLog = $"{nameof(TVRemote.OkButton)} press at {_okButtonPressEventQueue.Read().Time}\n" + _eventLog;

            while (_okButtonReleaseEventQueue.HasUnreadEvents)
                _eventLog = $"{nameof(TVRemote.OkButton)} release at {_okButtonReleaseEventQueue.Read().Time}\n" + _eventLog;


            while (_upButtonPressEventQueue.HasUnreadEvents)
                _eventLog = $"{nameof(TVRemote.UpButton)} press at {_upButtonPressEventQueue.Read().Time}\n" + _eventLog;

            while (_upButtonReleaseEventQueue.HasUnreadEvents)
                _eventLog = $"{nameof(TVRemote.UpButton)} release at {_upButtonReleaseEventQueue.Read().Time}\n" + _eventLog;


            while (_downButtonPressEventQueue.HasUnreadEvents)
                _eventLog = $"{nameof(TVRemote.DownButton)} press at {_downButtonPressEventQueue.Read().Time}\n" + _eventLog;

            while (_downButtonReleaseEventQueue.HasUnreadEvents)
                _eventLog = $"{nameof(TVRemote.DownButton)} release at {_downButtonReleaseEventQueue.Read().Time}\n" + _eventLog;


            while (_leftButtonPressEventQueue.HasUnreadEvents)
                _eventLog = $"{nameof(TVRemote.LeftButton)} press at {_leftButtonPressEventQueue.Read().Time}\n" + _eventLog;

            while (_leftButtonReleaseEventQueue.HasUnreadEvents)
                _eventLog = $"{nameof(TVRemote.LeftButton)} release at {_leftButtonReleaseEventQueue.Read().Time}\n" + _eventLog;


            while (_rightButtonPressEventQueue.HasUnreadEvents)
                _eventLog = $"{nameof(TVRemote.RightButton)} press at {_rightButtonPressEventQueue.Read().Time}\n" + _eventLog;

            while (_rightButtonReleaseEventQueue.HasUnreadEvents)
                _eventLog = $"{nameof(TVRemote.RightButton)} release at {_rightButtonReleaseEventQueue.Read().Time}\n" + _eventLog;


            string currentStateText = string.Empty;

            currentStateText = $"{nameof(TVRemote.OkButton)} held = {TVRemote.OkButton.IsHeld}\n" + currentStateText;
            currentStateText = $"{nameof(TVRemote.UpButton)} held = {TVRemote.UpButton.IsHeld}\n" + currentStateText;
            currentStateText = $"{nameof(TVRemote.DownButton)} held = {TVRemote.DownButton.IsHeld}\n" + currentStateText;
            currentStateText = $"{nameof(TVRemote.LeftButton)} held = {TVRemote.LeftButton.IsHeld}\n" + currentStateText;
            currentStateText = $"{nameof(TVRemote.RightButton)} held = {TVRemote.RightButton.IsHeld}\n" + currentStateText;

            _text.text = currentStateText + "\n" + _eventLog;
        }
    }
}
