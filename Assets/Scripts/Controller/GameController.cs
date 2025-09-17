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
        /// <summary>
        /// Converts an infix expression to a postfix expression using the Shunting Yard algorithm.
        /// </summary>
        /// <param name="expression">The expression to convert</param>
        /// <returns>Queue of strings</returns>
        private Queue<string> ConvertToPostfixExpression(string expression)
        {
            Queue<string> postfixStack = new();
            Stack<string> operationStack = new();

            string number = "";

            // Handle negative numbers at the start of the expression
            if (expression[0] == '-')
            {
                number += '-';
                expression = expression[1..];
            }

            foreach (var character in expression)
            {
                if (IsOperator(character))
                {
                    // If we encounter an operator, push the current number to the postfix stack
                    postfixStack.Enqueue(number);
                    number = "";

                    if (operationStack.Count == 0)
                    {
                        operationStack.Push(character.ToString());
                        continue;
                    }

                    // Pop operators from the operation stack to the postfix stack based on precedence
                    // until we find an operator with less precedence or the stack is empty
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
                    // If we encounter an integer character or a decimal point, append it to the current number string
                    number += character;
                }
            }

            // Push the last number to the postfix stack if it exists
            if (number != "")
            {
                postfixStack.Enqueue(number);
            }

            // Pop any remaining operators from the operation stack to the postfix stack
            while (operationStack.Count > 0)
            {
                postfixStack.Enqueue(operationStack.Pop());
            }

            return postfixStack;
        }

        /// <summary>
        /// Applies the given operator on the two operands and returns the result as a string.
        /// </summary>
        /// <param name="a">first operand</param>
        /// <param name="b">second operand</param>
        /// <param name="operatorChar">operator</param>
        /// <returns>result in string format</returns>
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

        /// <summary>
        /// Checks if the given character is an operator.
        /// </summary>
        /// <param name="character">the character to check</param>
        /// <returns>true if the character is operator else false</returns>
        internal bool IsOperator(char character)
        {
            return character == '+' || character == '-' || character == '×' || character == '÷';
        }

        /// <summary>
        /// Evaluates the given infix expression and returns the result as a string.
        /// </summary>
        /// <param name="expression">expression to solve</param>
        /// <returns>result in string format</returns>
        internal string EvaluateExpression(string expression)
        {
            Queue<string> postfixQueue = ConvertToPostfixExpression(expression);

            // evaluate postfix expression
            Stack<string> tempStack = new();

            while (postfixQueue.Count > 0)
            {
                string value = postfixQueue.Dequeue();

                // Checking for operator
                if (value.Length == 1 && IsOperator(value[0]))
                {
                    string second = tempStack.Pop();
                    string first = tempStack.Pop();

                    LoggerUtility.LogInEditor($"{first} {value[0]} {second}");

                    float a = float.Parse(first);
                    float b = float.Parse(second);

                    string result = ApplyOperator(a, b, value[0]);

                    // If at any point division by zero occurs, return "Undefined"
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

            // final result
            return tempStack.Pop();
        }
        #endregion Public Methods
    }
}
