using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utilities.Console
{
    public class ChatConsole : Console
    {
        public ChatConsole(string prefix, IEnumerable<IConsoleCommand> commands) : base(prefix, commands)
        {
            
        }
        public new void ProcessCommand(string inputValue)
        {
            if (!inputValue.StartsWith(prefix)) { 
                Debug.Log(inputValue); 
                return; 
            } 
            inputValue = inputValue.Remove(0, prefix.Length);
            string[] tmp = inputValue.Split(" ");
            ExecuteCommand(tmp[0], tmp[1..tmp.Length]);
            // array slicing is only supported for #Net 8.0 or higher
        }
    }
}