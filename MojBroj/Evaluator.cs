using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojBroj
{
    class Evaluator
    {
        List<int> values;
        List<int> selectors;
        List<string> operators;

        List<List<int>> valuesPermutationList;
        int valuesPermutationIndex;

        string[] operatorList = { "+", "-", "*", "/" };

        public Evaluator(List<int> values)
        {
            this.values = values;
            selectors = new List<int>();
            operators = new List<string>();

            valuesPermutationList = values.GetAllPermutations();

            for (int i = 0; i < values.Count - 1; i++)
            {
                operators.Add("+");
            }

            for (int i = 0; i < 2 * values.Count - 1; i++)
            {
                selectors.Add(0);
            }

            for (int i = values.Count; i < selectors.Count; i++)
            {
                selectors[i] = 1;
            }

            this.values.Sort();
        }

        private float Evaluate(Stack<float> stack)
        {
            int valueIndex = 0, opIndex = 0;

            foreach(int selector in this.selectors)
            {
                if (selector == 0)
                {
                    if (valueIndex < this.values.Count)
                    stack.Push(this.values[valueIndex++]);
                    continue;
                }

                float lastNumber = stack.Pop();
                float returnNumber = stack.Pop();
                

                switch(this.operators[opIndex++])
                {
                    case "+":
                        returnNumber += lastNumber;
                        break;
                    case "-":
                        returnNumber -= lastNumber;
                        break;
                    case "*":
                        returnNumber *= lastNumber;
                        break;
                    case "/":
                        if (lastNumber == 0)
                        {
                            return 999999999;
                        }

                        float testNumber = returnNumber / lastNumber;

                        if (testNumber * lastNumber != returnNumber)
                        {
                            return 999999999;
                        }

                        returnNumber = testNumber;
                        break;
                }

                stack.Push(returnNumber);
            }

            return stack.Pop();
        }

        private bool NextValuesPermutation()
        {
            if (this.valuesPermutationIndex >= valuesPermutationList.Count)
            {
                return false;
            }

            this.values = valuesPermutationList[this.valuesPermutationIndex++];
            return true;
        }

        private int GetIndexOfOperator(string op)
        {
            for(int i = 0; i < this.operatorList.Length; i++)
            {
                if (this.operatorList[i].Equals(op)) {
                    return i;
                }
            }

            return -1;
        }

        private bool NextOperation()
        {
            for (int i = this.operators.Count - 1; i > -1; i--)
            {
                if (GetIndexOfOperator(this.operators[i]) + 1 < this.operatorList.Length)
                {
                    this.operators[i] = this.operatorList[GetIndexOfOperator(this.operators[i]) + 1];
                    return true;
                }

                this.operators[i] = "+";
            }

            return false;
        }

        private string MakeExpression(string leftOperand, string op, string rightOperand)
        {
            return "(" + leftOperand + " " + op + " " + rightOperand + ")";
        }

        private string GetExpressionString()
        {
            Stack<string> stringStack = new Stack<string>();

            int valueIndex = 0, opIndex = 0;

            foreach (int selector in this.selectors)
            {
                if (selector == 0)
                {
                    if (valueIndex < this.values.Count)
                    stringStack.Push(this.values[valueIndex++].ToString());
                    continue;
                }

                string stackTop = stringStack.Pop();
                string lastStack = stringStack.Pop();

                stringStack.Push(MakeExpression(lastStack, this.operators[opIndex++], stackTop));
            }

            return stringStack.Peek();
        }

        public List<string> GetExpressions(int targetNumber)
        {
            Stack<float> floatStack = new Stack<float>();
            List<string> expressions = new List<string>();

            do
            {
                do
                {
                    float value = Evaluate(floatStack);

                    if (value == targetNumber)
                    {
                        expressions.Add(targetNumber + " = " + GetExpressionString());
                    }

                } while (NextOperation());
            } while (NextValuesPermutation());

            return expressions;
        }
    }
}
