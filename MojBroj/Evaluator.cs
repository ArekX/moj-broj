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
        List<string> operators;

        List<List<int>> valuesPermutationList;
        int valuesPermutationIndex;

        string[] operatorList = { "+", "-", "*", "/" };

        public Evaluator(List<int> values)
        {
            this.values = values;
            operators = new List<string>();

            valuesPermutationList = values.GetAllPermutations();

            for (int i = 0; i < values.Count - 1; i++)
            {
                operators.Add("+");
            }

            this.values.Sort();
        }

        private float Evaluate(Stack<float> stack)
        {
            int opIndex = 0;

            foreach (float value in this.values)
            {
                stack.Push(value);
            }

            for (int i = 0; i < this.values.Count - 1; i++)
            {
                float rightOperand = stack.Pop();
                float leftOperand = stack.Pop();

                switch (this.operators[opIndex++])
                {
                    case "+":
                        leftOperand += rightOperand;
                        break;
                    case "-":
                        leftOperand -= rightOperand;
                        break;
                    case "*":
                        leftOperand *= rightOperand;
                        break;
                    case "/":
                        if (rightOperand == 0)
                        {
                            return 999999999;
                        }

                        float testNumber = leftOperand / rightOperand;

                        if (testNumber * leftOperand != rightOperand)
                        {
                            return 999999999;
                        }

                        leftOperand = testNumber;
                        break;
                }

                stack.Push(leftOperand);
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

            int opIndex = 0;

            foreach (float value in this.values)
            {
                stringStack.Push(value.ToString());
            }

            for (int i = 0; i < this.values.Count - 1; i++)
            {
                string rightOperand = stringStack.Pop();
                string leftOperand = stringStack.Pop();

                stringStack.Push(MakeExpression(leftOperand, this.operators[opIndex++], rightOperand));
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
