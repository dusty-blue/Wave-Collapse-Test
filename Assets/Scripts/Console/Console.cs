using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utilities.Console
{
    public class Console
    {
        protected readonly string prefix;
        protected readonly IEnumerable<IConsoleCommand> commands;
        public Console(string prefix, IEnumerable<IConsoleCommand> commands) {
            this.prefix = prefix;
            this.commands = commands;
        }
        
        public void ProcessCommand(string inputValue) {
            if(!inputValue.StartsWith(prefix)) {  return;} 
            inputValue = inputValue.Remove(0, prefix.Length);
            string[] tmp =inputValue.Split(" ");
            ExecuteCommand(tmp[0], tmp[1..tmp.Length]);
            // array slicing is only supported for #Net 8.0 or higher
        }

        public void ExecuteCommand(string commandInput, string[] args) {
            foreach (var command in commands) {
                if(!commandInput.Equals(command.CommandWord, StringComparison.OrdinalIgnoreCase)) {
                    continue;
                }
                if (command.Process(args))
                {
                    return;
                }
            }
            
        }
    }
}
