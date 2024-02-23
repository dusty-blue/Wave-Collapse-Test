using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Utilities.Console.Commands;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Utilities.Console
{
    public abstract class ConsoleBehaviour : MonoBehaviour
    {
        [SerializeField] protected string prefix = string.Empty;
        [SerializeField] protected ConsoleCommand[] commands = new ConsoleCommand[0];

        [Header("UI")]
        [SerializeField] protected GameObject uiCanvas = null;
        [SerializeField] protected TMPro.TMP_InputField inputField = null;

        protected Console _console;
        private Console Console
        {
            get
            {
                if (_console != null) { return _console; }
                return _console = new Console(prefix, commands);
            }
        }

        public void ProcessCommand(string inputValue)
        {
            Console.ProcessCommand(inputValue);
            inputField.text = string.Empty;
        }

        public abstract void Toggle(CallbackContext context); 
    }
}
