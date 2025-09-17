using Calculator.Model;
using Common;
using System.Collections.Generic;
using Utils;

namespace Calculator.Controller
{
    public class GameController : EntityController
    {
        #region Inspector Variables
        #endregion Inspector Variables

        #region Public Variables
        #endregion Public Variables

        #region Private Variables
        #endregion Private Variables

        #region Monobehaviour Methods
        private void Start()
        {
            OnGameStart();
        }

        private void OnDestroy()
        {
            OnGameOver();
        }
        #endregion Monobehaviour Methods

        #region Private Methods
        private Queue<string> ConvertToPostfixExpression(string expression)
        {
            Queue<string> postfixStack = new();
            Stack<string> operationStack = new();

            string number = "";

            foreach (var character in expression)
            {
                if (IsOperator(character))
                {
                    postfixStack.Enqueue(number);
                    number = "";

                    if (operationStack.Count == 0)
                    {
                        operationStack.Push(character.ToString());
                        continue;
                    }

                    int topOperatorPrecedence = GameModel.OperatorPrecedenceDict[operationStack.Peek()[0]];
                    int operatorPrecedence = GameModel.OperatorPrecedenceDict[character];

                    while (topOperatorPrecedence >= operatorPrecedence)
                    {
                        postfixStack.Enqueue(operationStack.Pop());

                        if (operationStack.Count == 0)
                        {
                            break;
                        }

                        topOperatorPrecedence = GameModel.OperatorPrecedenceDict[operationStack.Peek()[0]];
                    }

                    operationStack.Push(character.ToString());
                }
                else
                {
                    number += character;
                }
            }

            if (number != "")
            {
                postfixStack.Enqueue(number);
            }

            while (operationStack.Count > 0)
            {
                postfixStack.Enqueue(operationStack.Pop());
            }

            return postfixStack;
        }

        private string ApplyOperator(float a, float b, char operatorChar)
        {
            string result = "";
            switch (operatorChar)
            {
                case '+':
                    result = (a + b).ToString();
                    break;
                case '-':
                    result = (a - b).ToString();
                    break;
                case '×':
                    result = (a * b).ToString();
                    break;
                case '÷':
                    if (b == 0)
                    {
                        result = GameModel.DivisionByZeroString;
                    }
                    else
                    {
                        result = (a / b).ToString();
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
        #endregion Private Methods

        #region Public Methods
        public override void OnGameStart()
        {
            view.SetController(this);
            view.OnGameStart();
        }

        public override void OnGameOver()
        {
            view.OnGameOver();
        }

        internal bool IsOperator(char character)
        {
            return character == '+' || character == '-' || character == '×' || character == '÷';
        }

        internal string EvaluateExpression(string expression)
        {
            Queue<string> postfixQueue = ConvertToPostfixExpression(expression);

            Stack<string> tempStack = new();

            while (postfixQueue.Count > 0)
            {
                string value = postfixQueue.Dequeue();

                if (IsOperator(value[0]))
                {
                    string second = tempStack.Pop();
                    string first = tempStack.Pop();

                    LoggerUtility.LogInEditor($"{first} {value[0]} {second}");

                    float a = float.Parse(first);
                    float b = float.Parse(second);

                    string result = ApplyOperator(a, b, value[0]);

                    if (result == GameModel.DivisionByZeroString)
                    {
                        return result;
                    }

                    tempStack.Push(result);
                    continue;
                }
                else
                {
                    tempStack.Push(value);
                }
            }

            return tempStack.Pop();
        }
        #endregion Public Methods
    }
}
