using System.Linq.Expressions;
using System.Xml.Linq;

namespace Turing
{
    internal class Program
    {
        static string input;
        static Stack<string> pilaFirst = new();
        static Stack<string> pilaSecond = new();
        static Stack<string> pilaPostFija = new();

        static Dictionary<string, int> Operators = new Dictionary<string, int>();
        static void Main(string[] args)
        {
            Console.Write("Expresion a evaluar: ");
            input = Console.ReadLine();

            SepararEnPila(input);
            Console.WriteLine("Valores Pila Originales");
            MostratPila(pilaFirst);

            PilaSignos(pilaFirst);
            MostratPilaSecond(pilaSecond);

            /*PostFija();
            Console.WriteLine("Pila Post Fija");
            MostratPila(pilaPostFija);*/

            ///////1+2*4+5-3
            string infix = "";
            string postfix = "";

            foreach(var elemento in pilaSecond) 
            { 
                infix += elemento.ToString();
            }

            Console.WriteLine("INFIX: " + infix);
            InfixToPostfix(ref pilaSecond, out postfix);
            Console.WriteLine("POSTFIX: " + postfix + "\n");


            InitializeOperators();
            decimal output = EvaluarPostFija(postfix);
            Console.WriteLine("\nResult : {0}", output);
            Console.WriteLine("Termina programa....");

        }

        public static void SepararEnPila(string input)
        {
            string numero = "";
            bool addedLast = false;
            for (int c = 0; c < input.Length; c++)
            {
                if (!IsOperator(input[c].ToString()))
                {
                    numero += input[c];
                    addedLast = false;
                }
                else
                {
                    if (numero != "")
                    {
                        //Console.WriteLine($"Pushing: {numero} in FOR");
                        pilaFirst.Push(numero);
                        numero = "";
                        addedLast = true;
                    }

                    numero += input[c];
                    pilaFirst.Push(numero);
                    numero = "";
                }
            }

            if (!addedLast)
            {
                //Console.WriteLine($"Pushing: {numero} in LAST");
                pilaFirst.Push(numero);
            }
        }

        public static void PilaSignos(Stack<string> pila)
        {
            string operador1 = "", operador2 = "", operadorFinal;

            Console.WriteLine("\nEvaluar Signos");
            while (pila.Count > 0)
            {
                string elemento = pila.Pop();

                if (elemento == "(" || elemento == ")" || elemento == "*")
                {
                    pilaSecond.Push(elemento);
                    continue;
                }

                if (IsOperator(elemento))
                {
                    if (operador1 == "")
                    {
                        operador1 = GetOperator(elemento);
                    }
                    else
                    {
                        operador2 = GetOperator(elemento);
                    }
                }
                else
                {
                    pilaSecond.Push(elemento);
                    operador1 = "";
                    operador2 = "";
                }

                if (operador1 != "" && operador2 != "")
                {
                    //Console.WriteLine($"Operador1 {operador1} | Operador2 {operador2}");
                    if ((operador1 == "+" || operador1 == "-") && (operador2 == "+" || operador2 == "-"))
                    {
                        if ((operador1 == "-" && operador2 == "-") || (operador1 == "+" && operador2 == "+"))
                        {
                            operadorFinal = "+";
                        }
                        else
                        {
                            operadorFinal = "-";
                        }
                        //Console.WriteLine($"Pushed new operator: {operadorFinal}");
                        pilaSecond.Push(operadorFinal);
                        operador1 = "";
                        operador2 = operadorFinal;
                    }
                    else
                    {
                        pilaSecond.Push(elemento);
                        operador1 = "";
                        operador2 = "";
                    }
                }
                else if (operador1 != "" && !IsOperator(pila.Peek()))
                {
                    pilaSecond.Push(elemento);
                    operador1 = "";
                    operador2 = "";
                }
            }
        }

        public static void PostFija()
        {
            Stack<string> pila = new Stack<string>();
            Queue<string> salida = new Queue<string>();

            string[] elementos = pilaSecond.ToArray();

            // Iterar a través de cada elemento de la expresión
            foreach (string elemento in elementos)
            {
                // Si es un número, añadirlo a la cola de salida
                double numero;
                if (double.TryParse(elemento, out numero))
                {
                    salida.Enqueue(elemento);
                }
                // Si es un operador, añadirlo a la pila
                else if (elemento == "+" || elemento == "-" || elemento == "*" || elemento == "/" || elemento == "^" || elemento == "(")
                {
                    while (pila.Count > 0 && (precedencia(pila.Peek()) >= precedencia(elemento)))
                    {
                        salida.Enqueue(pila.Pop());
                    }
                    pila.Push(elemento);
                }
                // Si es un paréntesis derecho, desapilar los elementos hasta encontrar el paréntesis izquierdo
                else if (elemento == ")")
                {
                    while (pila.Peek() != "(")
                    {
                        salida.Enqueue(pila.Pop());
                    }
                    pila.Pop(); // Eliminar el paréntesis izquierdo de la pila
                }
                // Si es otro tipo de elemento (por ejemplo, un paréntesis izquierdo), ignorarlo
                else
                {
                    continue;
                }
            }

            // Desapilar los elementos restantes y añadirlos a la cola de salida
            while (pila.Count > 0)
            {
                salida.Enqueue(pila.Pop());
            }

            // Imprimir la expresión en notación postfija
            foreach (string elemento in salida)
            {
                //Console.Write(elemento + " ");
                pilaPostFija.Push(elemento);
            }
            pilaPostFija.Reverse();
            Console.WriteLine();

        }

        // Función para determinar la precedencia de los operadores
        static int precedencia(string operador)
        {
            switch (operador)
            {
                case "^":
                    return 3;
                case "*":
                case "/":
                    return 2;
                case "+":
                case "-":
                    return 1;
                default:
                    return 0;
            }
        }

        public static void MostratPila(Stack<string> pila)
        {
            foreach (string obj in pila.Reverse())
            {
                Console.WriteLine($"[{obj}]");
            }
        }

        public static void MostratPilaSecond(Stack<string> pila)
        {
            foreach (string obj in pila)
            {
                Console.WriteLine($"[{obj}]");
            }
        }

        public static bool IsOperator(string caracter)
        {
            if (caracter.Contains("+") || caracter.Contains("-") || caracter.Contains("*") || caracter.Contains("/") || caracter.Contains("^") || caracter.Contains("(") || caracter.Contains(")"))
            {
                return true;
            }
            else { return false; }
        }

        public static string GetOperator(string caracter)
        {
            if (caracter.Contains("+"))
                return "+";
            if (caracter.Contains("-"))
                return "-";
            if (caracter.Contains("*"))
                return "*";
            if (caracter.Contains("/"))
                return "/";
            if (caracter.Contains("^"))
                return "^";
            else
                return "CARACTER INVALIDO";
        }


        public static decimal EvaluarPostFija(string PostfixString)
        {
            try
            {
                char[] separator = { ' ' };
                string[] tokensArray = PostfixString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                Stack<decimal> argumentsStack = new Stack<decimal>();
                decimal number;
                string token;

                //Read the tokens one by one
                for (int i = 0; i < tokensArray.Length; i++)
                {
                    token = tokensArray[i];
                    number = decimal.Zero;

                    //If the token is a value - Push it onto the stack.
                    if (decimal.TryParse(token, out number))
                    {
                        argumentsStack.Push(number);
                    }
                    //if the token is an operator
                    else if (isOperator(token))
                    {
                        int ArgumentsRequired = Operators[token];

                        //If there are fewer values on the stack than the number of arguments(n) required by the operator
                        if (argumentsStack.Count < ArgumentsRequired)
                        {
                            throw new Exception("The user has not provided sufficient values in the expression.");
                        }
                        else
                        {
                            //Pop the top n values from the stack.
                            List<decimal> argsList = new List<decimal>();
                            for (int k = 0; k < ArgumentsRequired; k++)
                            {
                                argsList.Add(argumentsStack.Pop());
                            }

                            //We need to reverse the order of the elements in the list to resemble the order of stack.
                            argsList.Reverse();

                            //Evaluate the operator, with the values as arguments.
                            IOperation IObj = ObjectFactory(token);
                            decimal val = IObj.CalculateResultOfOperation(argsList);

                            //Push the returned results, if any, back onto the stack.
                            argumentsStack.Push(val);
                        }
                    }
                    else
                    {
                        throw new Exception("Unknown operator found in the input: " + token);
                    }
                }

                //If there is only one value in the stack
                if (argumentsStack.Count == 1)
                {
                    //That value is the result of the calculation.
                    return argumentsStack.Pop();
                }
                else
                {
                    throw new Exception("The user has provided too many values.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void InitializeOperators()
        {
            //Operators.Add(<Operator>, <Number of Arguments which the operator takes>);
            Operators.Add("^", 2);
            Operators.Add("/", 2);
            Operators.Add("*", 2);
            Operators.Add("-", 2);
            Operators.Add("+", 2);
        }

        private static bool isOperator(String token)
        {
            return Operators.ContainsKey(token);
        }

        //Factory design pattern
        static public IOperation ObjectFactory(string choice)
        {
            IOperation objOperator = null;

            switch (choice)
            {
                case "+":
                    objOperator = new Addition();
                    break;
                case "-":
                    objOperator = new Subtraction();
                    break;
                case "*":
                    objOperator = new Multiplication();
                    break;
                case "/":
                    objOperator = new Division();
                    break;
                case "^":
                    objOperator = new Exponent();
                    break;
            }
            return objOperator;

        }

        static bool InfixToPostfix(ref Stack<string> pilaInfix, out string postfix)
        {
            string[] infix = pilaInfix.ToArray();
            int prio = 0;
            postfix = "";
            Stack<string> s1 = new Stack<string>();
            for (int i = 0; i < infix.Length; i++)
            {
                string ch = infix[i];
                if (ch == "+" || ch == "-" || ch == "*" || ch == "/")
                {
                    if (s1.Count <= 0)
                        s1.Push(ch);
                    else
                    {
                        if (s1.Peek() == "*" || s1.Peek() == "/")
                            prio = 1;
                        else
                            prio = 0;
                        if (prio == 1)
                        {
                            if (ch == "+" || ch == "-")
                            {
                                postfix += s1.Pop() + " ";
                                i--;
                            }
                            else
                            {
                                postfix += s1.Pop() + " ";
                                i--;
                            }
                        }
                        else
                        {
                            if (ch == "+" || ch == "-")
                            {
                                postfix += s1.Pop() + " ";
                                s1.Push(ch);

                            }
                            else
                                s1.Push(ch);
                        }
                    }
                }
                else
                {
                    postfix += ch + " ";
                }
            }
            int len = s1.Count;
            for (int j = 0; j < len; j++)
                postfix += s1.Pop() + " ";
            return true;
        }
    }
}