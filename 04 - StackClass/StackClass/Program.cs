using System;

namespace StackClass
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (input.Trim().ToLower() == "quit")
                    break;

                // The stack of integers not yet operated on.
                Stack<int> values = new Stack<int>();

                foreach (string token in input.Split(new char[] { ' ' }))
                {
                    // If the value is an integer...
                    int value;
                    if (int.TryParse(token, out value))
                    {
                        // ... push it to  the stack/
                        values.Push(value);
                    }
                    else
                    {
                        // Otherwise evaluate the expression
                        int rhs = values.Pop();
                        int lhs = values.Pop();

                        // ... and pop the result back to the stack.
                        switch (token)
                        {
                            case "+":
                                values.Push(lhs + rhs);
                                break;

                            case "-":
                                values.Push(lhs - rhs);
                                break;

                            case "*":
                                values.Push(lhs * rhs);
                                break;

                            case "/":
                                values.Push(lhs / rhs);
                                break;

                            case "%":
                                values.Push(lhs % rhs);
                                break;

                            default:
                                throw new ArgumentException(
                                    string.Format("Unrecognised token: {0}", token));
                        }
                    }
                }

                // The last item on the stack is the result.
                Console.WriteLine(values.Pop());

                Console.ReadLine();
            }
        }
    }
}