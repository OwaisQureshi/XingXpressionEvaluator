using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XingXpressionEvaluator
{
    public sealed class XpressionEvaluator
    {
        private Stack<double> tokens;
        private Stack<char> operators;
        private Dictionary<char, OperatorInfo> operatorList;
        private string expression = null;
        private bool isOperand = false;
        private bool isOperator = false;
        //private string Expression
        //{
        //    get { return expression; }
        //    set { expression = value; }
        //}
        private double expressionResult;

        /// <summary>
        /// It intializes XpressionEvaluator's basic required fields.
        /// </summary>
        /// <param name="expr"></param>
        public XpressionEvaluator()
        {
            //two stacks needed
            tokens = new Stack<double>();
            operators = new Stack<char>();

            operatorList = new Dictionary<char, OperatorInfo> 
            {
                {'+',new OperatorInfo('+',"Add",2,OperatorFixity.Left)},
                {'-',new OperatorInfo('-',"Minus",2,OperatorFixity.Left)},
                {'/',new OperatorInfo('/',"Divide",3,OperatorFixity.Left)},
                {'*',new OperatorInfo('*',"Multiply",3,OperatorFixity.Left)},
                {'^',new OperatorInfo('^',"Caret",4,OperatorFixity.Right)},
                {'(',new OperatorInfo('(',"OpenBracket",-1,OperatorFixity.Left)},//5
                {')',new OperatorInfo(')',"CloseBracket",-1,OperatorFixity.Left)}//5
            };
        }

        /// <summary>
        /// It calulates valid Math expression
        /// </summary>
        /// <returns>
        /// Math expression with number precision of 15 digits
        /// </returns>
        public double CalculateExpression(string expr)
        {
            CheckExpression(expr);

            for (int i = 0; i < expression.Length; i++)
            {
                if (IsNumber(expression[i]))//if its a number
                {
                    //extract number
                    string[] getNumberNCount = getNumberAndCount(expression.Substring(i)).Split(',');
                    double val = double.Parse(getNumberNCount[0]);
                    int count = int.Parse(getNumberNCount[1]);
                    tokens.Push(val);
                    i += count;
                }
                else if (IsOperator(expression[i]))//if its an operator
                {
                    if (IsNegativeNumber(expression, i))
                    {
                        //add negativeNumber on operand stack
                        string[] getNumberNCount = getNumberAndCount(expression.Substring(i + 1)).Split(',');
                        double val = double.Parse(expression[i] + getNumberNCount[0]);//multiply it by - ,or concat with minus
                        int count = int.Parse(getNumberNCount[1]);
                        tokens.Push(val);
                        i += count + 1;
                        //continue;
                    }

                    //Maintain precedence on stack
                    else if (operators.Count > 0 && expression[i] != '(' && expression[i] != ')')
                    {
                        CheckPrecedence(expression[i], ref tokens, ref operators, operatorList);
                    }
                    else if (expression[i] == ')')
                    {
                        while (operators.Peek() != '(')
                        {
                            var LastOperator = operators.Pop();
                            var LastOperand = tokens.Pop();
                            var SecondLastOperand = tokens.Pop();
                            tokens.Push(Calculate(LastOperator, LastOperand, SecondLastOperand));
                        }
                        operators.Pop();
                    }
                    else
                    {
                        operators.Push(expression[i]);
                    }
                }

                if (IsNumber(expression[i]) == false && IsOperator(expression[i]) == false)
                {
                    throw new ArithmeticException("Not a Number or an Operator");
                }
            }

            //now stack is made of RPN and all the precedence is solved
            while (operators.Count != 0)
            {
                var LastOperand = tokens.Pop();
                var SecondLastOperand = tokens.Pop();
                var lastOperator = operators.Pop();
                tokens.Push(Calculate(lastOperator, LastOperand, SecondLastOperand));
            }
            return tokens.Peek();
        }

        private void CheckExpression(string expr)
        {
            //define a method which can validate an expression /or simply implement expression property for validation
            //define custom exceptions and think of exception which this library can raise
            if (expr == null)
                throw new ArgumentNullException("Expression was null!");
            else if (expr == String.Empty)
                throw new ArgumentException("Expression was an empty string!");
            else
            {
                expression = expr.Replace(" ", string.Empty);
                expression = expression.Replace(",", string.Empty);
            }
        }

        /// <summary>
        /// It performs validations and make sure the expression supplied is a valid expression and can be calculated.
        /// </summary>
        /// <returns>returns true if the expression is valid</returns>
        public bool ValidateExpression(string expr)
        {
            CheckExpression(expr);

            double result = double.NegativeInfinity;
            bool isValidExpression = false;

                try
                {
                    result = CalculateExpression(expression);
                    //ArgumentException
                    //ArgumentNullException
                    //ArithmeticException
                }
                catch (Exception exc)
                {
                    result = double.NegativeInfinity;   
                }
                if ( double.IsInfinity(result) != true )
                {
                    if (double.IsNaN(result) != true)
                        isValidExpression = true;
                }
            return isValidExpression;
        }

        private void CheckPrecedence(char currentOp, ref Stack<double> values, ref Stack<char> operators, Dictionary<char, OperatorInfo> operatorList)
        {
            char lastStackOp = operators.Peek();
            //if same precedence of both Op are same
            //OR lastOp > than CurrentOp
            while (((operatorList[lastStackOp].Precedence == operatorList[currentOp].Precedence) ||
                    (operatorList[lastStackOp].Precedence > operatorList[currentOp].Precedence)))
            {
                var LastOperand = values.Pop();
                var SecondLastOperand = values.Pop();
                var TopMostOperator = operators.Pop();
                values.Push(Calculate(TopMostOperator, LastOperand, SecondLastOperand));

                if (operators.Count == 0)
                    break;

                lastStackOp = operators.Peek();
            }
            operators.Push(currentOp);//handles operatorList[lastStackOp].Precedence < operatorList[currentOp].Precedence
        }

        private double Calculate(char operatr, double LastOperand, double SecondLastOperand)
        {
            double result = 0;

            if (operatr == '+')
            {
                result = SecondLastOperand + LastOperand;
            }
            else if (operatr == '-')
            {
                result = SecondLastOperand - LastOperand;
            }
            else if (operatr == '/')
            {
                result = SecondLastOperand / LastOperand;
            }
            else if (operatr == '*')
            {
                result = SecondLastOperand * LastOperand;
            }
            else if (operatr == '^')
            {
                result = Math.Pow(SecondLastOperand, LastOperand);
            }
            else
            {
                throw new ArithmeticException("Invalid Operator found in the expression!");
            }

            return result;
        }

        private bool IsNumber(char value)
        {
            bool isNumber = false;
            double numb = 0;
            if (value == '.')
            {
                return isNumber = true;
            }

            isNumber = double.TryParse(value.ToString(), out numb);
            return isNumber;
        }

        private bool IsOperator(char value)
        {
            bool isOperator = false;
            string operators = "+-/*^)(";
            //if (value == '+' || value == '-' || value == '/' || value == '*' || value == ')' || value == '(')
            if (operators.Contains(value.ToString()))
            {
                isOperator = true;
            }
            return isOperator;
        }

        //return array of double
        private string getNumberAndCount(string numberStr)
        {
            var number = "";
            int count = 0;
            if (numberStr.Length >= 1)
            {
                while (IsNumber(numberStr[count]))
                {
                    number += numberStr[count];
                    count++;
                    //don't exceed the max length of string
                    if (numberStr.Length == 1 || count == numberStr.Length)
                        break;
                }
            }
            return number + "," + (count == 0 ? count : count - 1);
        }

        private bool IsNegativeNumber(string expression, int index)
        {
            bool isNegativeNumber = false;

            if (expression[index] == '-')
            {
                //is a operator is after another operator AND
                //after the operator is there a number ??
                if (IsOperator(expression[index - 1]) && IsNumber(expression[index + 1]))
                {
                    isNegativeNumber = true;
                }
            }
            return isNegativeNumber;
        }
    }
}
