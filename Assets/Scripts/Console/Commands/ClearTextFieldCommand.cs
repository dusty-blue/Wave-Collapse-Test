using Assets.Scripts.Utilities.Console.Commands;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Utilities.Console.Commands
{
    [CreateAssetMenu(fileName = "New Clear Textfield command", menuName = "Utilities/Console/Command/Clear Text Field Command")]
    public class ClearTextFieldCommand : ConsoleCommand
    {
        private TextMeshProUGUI _textField;
        private DebugLogOutput _debugLogOutput;
        [SerializeField] string textFieldName;


        public override bool Process(string[] args)
        {
            if(_textField == null || _debugLogOutput == null)
            {
                _textField = GameObject.Find(textFieldName).GetComponent<TextMeshProUGUI>();
                _debugLogOutput = GameObject.Find(textFieldName).transform.parent.GetComponentInParent<DebugLogOutput>();
                if (_textField == null || _debugLogOutput == null)
                {
                    Debug.Log($"Could not find textField with name {textFieldName}.");
                } else
                {
                    _debugLogOutput.ClearQueue();
                    _textField.text = string.Empty;
                }
            } else
            {
                _textField.text = string.Empty;
                _debugLogOutput.ClearQueue();
            }
            
            return true;
        }

    }
}