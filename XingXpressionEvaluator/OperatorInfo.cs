using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XingXpressionEvaluator
{
    class OperatorInfo
    {
        private string name;
        public string Name
        {
            get 
            {
                return name;
            }
            set
            {
                //if (value),  shudnt be a number ,special character
                name = value;//by using Name in the setter , by using the setter than results in SOException
            }

        }

        private char symbol;
        public char Symbol
        {
            get 
            { 
                return symbol;
            }
            set 
            {
                symbol = value; 
            }
        }

        private int precedence;//1 is lowest ,4 is higher
        public int Precedence
        {
            get { return precedence; }
            set
            {
                if (value >= 0)
                {
                    precedence = value;
                }
            }
        }
        OperatorFixity Associativity;

        /// <summary>
        /// OperatorInfo is used for defining
        /// </summary>
        /// <param name="Symbol">Symbol used as operator.</param>
        /// <param name="Name">Name of the symbol.</param>
        /// <param name="Precedence">Operator precedence,which defines.</param>
        /// <param name="Associativity">Defines operator asscoiativity.</param>
        /// <returns>??</returns>
        public OperatorInfo(char Symbol,string Name,int Precedence, OperatorFixity Associativity)
        {
            this.Symbol = Symbol;
            this.Name = Name;
            this.Precedence = Precedence;
            this.Associativity = Associativity;
        }
    }

    enum OperatorFixity
    {
        Left = 1,
        Right = 2,
    }
}
