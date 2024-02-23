using Assets.Scripts.Utilities.Console.Commands;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utilities.Console.Commands
{
    [CreateAssetMenu(fileName = "New Log command", menuName = "Utilities/Console/Command/Log Command")]
    public class LogCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            string logText = string.Join(" ", args);
            Debug.Log(logText);
            return true;
        }

    }
}