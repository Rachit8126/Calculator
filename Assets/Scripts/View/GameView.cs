using Calculator.Component;
using Calculator.Model;
using Common;
using System;
using TMPro;
using UnityEngine;
using Utils;

namespace Calculator.View
{
    public class GameView : EntityView
    {
        #region Inspector Variables
        [SerializeField] private ButtonComponent[] buttonComponentsArray;
        [SerializeField] private TextMeshProUGUI inputText;
        [SerializeField] private int maxInputLength;
        #endregion Inspector Variables

        #region Public Variables
        #endregion Public Variables

        #region Private Variables
        #endregion Private Variables

        #region Private Methods
        private void Clear()
        {
            inputText.text = "";
        }

        private void OnButtonPressed(ButtonType type, string value)
        {
            LoggerUtility.LogInEditor($"{type} pressed with value {value}", Color.green);

            switch (type)
            {
                case ButtonType.OPERATION:
                    OnOperationButtonPressed(value);
                    break;
                case ButtonType.NUMBER:
                    OnNumberButtonPressed(value);
                    break;
                case ButtonType.EQUALS:
                    OnEqualsPressed();
                    break;
                case ButtonType.CLEAR:
                    Clear();
                    break;
                default:
                    break;
            }
        }

        private void OnOperationButtonPressed(string value)
        {
            UpdateText(value);
        }

        private void OnNumberButtonPressed(string value)
        {
            UpdateText(value);
        }

        private void OnEqualsPressed()
        {
            throw new NotImplementedException();
        }

        private void UpdateText(string value)
        {
            if (inputText.text.Length > maxInputLength)
            {
                return;
            }

            inputText.text += value;
        }
        #endregion Private Methods

        #region Public Methods
        public override void OnGameStart()
        {
            Clear();

            foreach (var buttonComponent in buttonComponentsArray)
            {
                buttonComponent.OnButtonPressed += OnButtonPressed;
            }
        }

        public override void OnGameOver()
        {
            foreach (var buttonComponent in buttonComponentsArray)
            {
                buttonComponent.OnButtonPressed -= OnButtonPressed;
            }
        }
        #endregion Public Methods
    }
}
