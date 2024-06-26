﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utilities.Console.Commands
{
    public abstract class ConsoleCommand : ScriptableObject, IConsoleCommand
    {
        [SerializeField] private string commandWord = string.Empty;

        public string CommandWord => commandWord;

        public abstract bool Process(string[] args);
    }
}
