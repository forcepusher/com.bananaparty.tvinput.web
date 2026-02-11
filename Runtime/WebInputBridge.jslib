const webInputBridgeLibrary = {

  // Class definition.

  $webInputBridge: {
    keyboardDeviceType: 0,
    gamepadDeviceType: 1,

    onButtonPressCallbackPtr: undefined,
    onButtonReleaseCallbackPtr: undefined,

    registeredKeyboardButtons: {},
    registeredGamepadButtons: {},

    previousGamepadButtonStates: {},

    initialize: function (onButtonPressCallbackPtr, onButtonReleaseCallbackPtr) {
      webInputBridge.onButtonPressCallbackPtr = onButtonPressCallbackPtr;
      webInputBridge.onButtonReleaseCallbackPtr = onButtonReleaseCallbackPtr;

      document.addEventListener('keydown', function (keyEvent) {
        if (webInputBridge.registeredKeyboardButtons[keyEvent.keyCode] && !keyEvent.repeat) {
          webInputBridge.invokeButtonCallback(webInputBridge.onButtonPressCallbackPtr, webInputBridge.keyboardDeviceType, keyEvent.keyCode);
        }
      });

      document.addEventListener('keyup', function (keyEvent) {
        if (webInputBridge.registeredKeyboardButtons[keyEvent.keyCode]) {
          webInputBridge.invokeButtonCallback(webInputBridge.onButtonReleaseCallbackPtr, webInputBridge.keyboardDeviceType, keyEvent.keyCode);
        }
      });
    },

    pollGamepadInput: function () {
      const gamepads = navigator.getGamepads ? navigator.getGamepads() : [];
      for (var gamepadIndex = 0; gamepadIndex < gamepads.length; gamepadIndex++) {
        const gamepad = gamepads[gamepadIndex];
        if (!gamepad) {
          continue;
        }
        
        var previousButtonStates = webInputBridge.previousGamepadButtonStates[gamepadIndex];
        if (!previousButtonStates) {
          previousButtonStates = webInputBridge.previousGamepadButtonStates[gamepadIndex] = [];
        }

        for (var buttonIndex = 0; buttonIndex < gamepad.buttons.length; buttonIndex++) {
          if (!webInputBridge.registeredGamepadButtons[buttonIndex]) {
            continue;
          }

          const previousButtonState = previousButtonStates[buttonIndex] || false;
          const currentButtonState = gamepad.buttons[buttonIndex].pressed;

          if (currentButtonState && !previousButtonState) {
            webInputBridge.invokeButtonCallback(webInputBridge.onButtonPressCallbackPtr, webInputBridge.gamepadDeviceType, buttonIndex);
          } else if (!currentButtonState && previousButtonState) {
            webInputBridge.invokeButtonCallback(webInputBridge.onButtonReleaseCallbackPtr, webInputBridge.gamepadDeviceType, buttonIndex);
          }

          previousButtonStates[buttonIndex] = currentButtonState;
        }
      }
    },

    registerButton: function (webInputDeviceType, webKeyCode) {
      if (webInputDeviceType === webInputBridge.keyboardDeviceType) {
        webInputBridge.registeredKeyboardButtons[webKeyCode] = true;
      } else {
        webInputBridge.registeredGamepadButtons[webKeyCode] = true;
      }
    },

    invokeButtonCallback: function (callbackPtr, webInputDeviceType, webKeyCode) {
      {{{ makeDynCall('vii', 'callbackPtr') }}}(webInputDeviceType, webKeyCode);
    },
  },

  // External C# calls.

  WebInputBridgeInitialize: function (onButtonPressCallbackPtr, onButtonReleaseCallbackPtr) {
    webInputBridge.initialize(onButtonPressCallbackPtr, onButtonReleaseCallbackPtr);
  },

  WebInputBridgeRegisterButton: function (webInputDeviceType, webKeyCode) {
    webInputBridge.registerButton(webInputDeviceType, webKeyCode);
  },

  WebInputBridgePollInput: function () {
    webInputBridge.pollGamepadInput();
  },
}

autoAddDeps(webInputBridgeLibrary, '$webInputBridge');
mergeInto(LibraryManager.library, webInputBridgeLibrary);
