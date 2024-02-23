using Assets.Scripts.Utilities.Console.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Utilities.Console
{
    public class DevConsoleBehaviour : ConsoleBehaviour
    {
        private float prevTimeScale;
        private static DevConsoleBehaviour instance;

        private void Awake()
        {
            //this seems like overkill and doesnt allow from multiple windows/views
            if(instance != null && instance != this) {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public override void Toggle(CallbackContext context)
        {
            //left at 13:41 https://www.youtube.com/watch?v=usShGWFLvUk&ab_channel=DapperDino

            if (!context.action.triggered) { return; }

            if (uiCanvas.activeSelf)
            {
                Time.timeScale = prevTimeScale;
                uiCanvas.SetActive(false);
            } else
            {
                prevTimeScale = Time.timeScale;
                Time.timeScale = 0;
                uiCanvas.SetActive(true);
                inputField.ActivateInputField();
            }
        }

    }
}
