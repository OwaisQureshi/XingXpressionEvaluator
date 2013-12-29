using System;
using System.Collections.Generic;
using System.Text;
using XingXpressionEvaluator;

namespace expressionParser
{
    class Program
    {
        //Implementing Dijsktra Shunting Yard Algorithm
        //I'm using this console program for testing purposes
        static void Main(string[] args)
        {
            List<string> expression = new List<string>();
            //Console.WriteLine("Enter math expression: ");
            //string expression = Console.ReadLine();

            //expression.Add("((8+4)/(4*3))");
            //expression.Add("((8+4)/3)");
            //expression.Add("1+2*3/4*8+3/2*3^3");//[2].ToString();
            //expression.Add("2^3");
            //expression.Add("2+2*3/4"); // 2.6666666666666665 // WRONG // 3.5 CORRECT NOW
            //expression.Add("((4*58)-((89+85*3)/200)/100)");
            //expression.Add("2.5+1");
            //expression.Add("((4*58)-((89+85*3)/200)/100)");
            //expression.Add("1+2*3/4*8+3/2*3");//17.5
            //expression.Add("2*3+232+34*45/3-4*45+3");//568 WRONG // 571 CORRECT NOW
            //expression.Add("22753+3345*222*90/3.346");//-19951275.69

            ////**********Negation Numbers*********************8
            //expression.Add("2--2");//4 /Passed
            //expression.Add("22753+3345*-2*90/3.346");//-19951275.69//passed
            //expression.Add("22753+3345*-222*90/3.346");
            //expression.Add("2+-2");//0//passed
            //expression.Add("-2*90");//passed
            //expression.Add("22753+3345*-25.459*90/3.346");
            //expression.Add("2+(-2)");
            //expression.Add("(2.25/3.56695499*(9.3656654+98.2235))/0.2");
            //expression.Add("--2+3");
            //expression.Add("2+-3--3/4*898+3.45");
            //expression.Add("2+3---4");

            ////For validation checking
            //expression.Add(@"2\52"); //False
            //expression.Add(@"2)52");
            //expression.Add(@"2(52");
            //expression.Add("(2.25/3.56695499*(9.3656654+98.2235))/0.2");
            //expression.Add(@"(2.25/3.56695499*(9.3656654+98.2235))/0.2");
            //expression.Add("34/0");//for infinity

            ////For comma formatted values
            //expression.Add("24,000/10");
            //expression.Add("24,000/1,000");
            //expression.Add("24,000-1,000*8,000+30,000");
            //expression.Add("(24,000-1,000)*8,000+30,000");
            //expression.Add("24-1*8+3");
            //expression.Add("3-1*2+1");//accepts alphabets
            expression.Add("3-1");
            //expression.Add("24000-1000*8000+30000");


            var printTemplate = "Expression: {0} the result : {1}";
            XpressionEvaluator expressionCalc;
            bool isValidated;
            string result;
            foreach (string expr in expression)
            {
                expressionCalc = new XpressionEvaluator();
                isValidated = expressionCalc.ValidateExpression(expr);
                if (isValidated)
                {
                    result = expressionCalc.CalculateExpression(expr).ToString();
                }
                else
                {
                    result = "EXPRESSION IS INVALID !";
                }
                Console.WriteLine(string.Format(printTemplate, expr, result));
            }
            Console.ReadLine();
        }
    }
}