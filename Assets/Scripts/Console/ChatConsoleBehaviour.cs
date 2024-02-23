using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Utilities.Console
{
    public class ChatConsoleBehaviour : ConsoleBehaviour
    {
        private Console Console
        {
            get
            {
                if (_console != null) { return _console; }
                return _console = new ChatConsole(prefix, commands);
            }
        }

        public override void Toggle(InputAction.CallbackContext context)
        {
            if (!context.action.triggered) { return; }

            if (uiCanvas.activeSelf)
            {
                uiCanvas.SetActive(false);
            }
            else
            {
                uiCanvas.SetActive(true);
                inputField.ActivateInputField();
            }
        }

        // Use this for initialization
        void Start()
        {
            Debug.Log("Test me this");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}