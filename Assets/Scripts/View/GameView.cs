using Calculator.Component;
using Calculator.Controller;
using Calculator.Model;
using Common;
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
        /// <summary>
        /// Clears the input text
        /// </summary>
        private void Clear()
        {
            inputText.text = "0";
        }

        /// <summary>
        /// Performs action based on button type
        /// </summary>
        /// <param name="type">Button Type</param>
        /// <param name="value">Button Value</param>
        private void OnButtonPressed(ButtonType type, char value)
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

        /// <summary>
        /// Handles operation button press
        /// </summary>
        /// <param name="value"></param>
        private void OnOperationButtonPressed(char value)
        {
            UpdateText(value);
        }

        /// <summary>
        /// Handles number button press
        /// </summary>
        /// <param name="value"></param>
        private void OnNumberButtonPressed(char value)
        {
            UpdateText(value);
        }

        /// <summary>
        /// Handles equals button press
        /// </summary>
        private void OnEqualsPressed()
        {
            string expression = inputText.text;
            string result = GetController<GameController>().EvaluateExpression(expression);
            inputText.text = result;
        }

        /// <summary>
        /// Handles updating the input text based on the button pressed
        /// </summary>
        /// <param name="value"></param>
        private void UpdateText(char value)
        {
            // If the input is longer than max length, ignore further input
            if (inputText.text.Length > maxInputLength)
            {
                return;
            }

            // If the input text is "0", replace it with the new value
            // If the new value is an operator, ignore it except the minus operator
            if (inputText.text.Length == 1 && (inputText.text[0] == '0' || inputText.text[0] == '-'))
            {
                // Allow minus operator to be the first character
                if (value == '-')
                {
                    inputText.text = value.ToString();
                    return;
                }

                // Ignore if the value is an operator
                if (GetController<GameController>().IsOperator(value))
                {
                    return;
                }

                // If the first character is not a minus, clear the text
                if (!(inputText.text[0] == '-'))
                {
                    inputText.text = "";
                }
            }

            // Prevent two consecutive operators
            if (inputText.text.Length > 1 &&
                GetController<GameController>().IsOperator(inputText.text[^1]) &&
                GetController<GameController>().IsOperator(value))
            {
                inputText.text = inputText.text[..^1];
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
