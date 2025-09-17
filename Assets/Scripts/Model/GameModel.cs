using System.Collections.Generic;

namespace Calculator.Model
{
    public class GameModel
    {
        internal static readonly Dictionary<char, int> OperatorPrecedenceDict = new()
        {
            { '+', 1 },
            { '-', 1 },
            { '×', 2 },
            { '÷', 2 }
        };

        internal static readonly string DivisionByZeroString = "Undefined";
    }

    public enum ButtonType
    {
        OPERATION,
        NUMBER,
        EQUALS,
        CLEAR
    }
}

