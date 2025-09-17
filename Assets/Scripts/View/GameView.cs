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
        private void Clear()
        {
            inputText.text = "0";
        }

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

        private void OnOperationButtonPressed(char value)
        {
            UpdateText(value);
        }

        private void OnNumberButtonPressed(char value)
        {
            UpdateText(value);
        }

        private void OnEqualsPressed()
        {
            string expression = inputText.text;
            string result = GetController<GameController>().EvaluateExpression(expression);
            inputText.text = result;
        }

        private void UpdateText(char value)
        {
            if (inputText.text.Length > maxInputLength)
            {
                return;
            }

            if (inputText.text.Length == 1 && (inputText.text[0] == '0' || inputText.text[0] == '-'))
            {
                if (value == '-')
                {
                    inputText.text = value.ToString();
                    return;
                }

                if (GetController<GameController>().IsOperator(value))
                {
                    return;
                }

                if (!(inputText.text[0] == '-'))
                {
                    inputText.text = "";
                }
            }

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
