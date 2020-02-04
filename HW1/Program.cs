using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HW1
{
    delegate double MathOperation(double a, double b);

    class MainClass
    {
        const string ExprPath = "expressions.txt";
        const string ExprAnswPath = "answers.txt";
        const string ExprChecker = "expressions_checker.txt";
        const string Result = "results.txt";
        static Dictionary<String, MathOperation> operations;
        static MainClass()
        {
            operations = new Dictionary<string, MathOperation>();
            operations.Add("+", (x, y) => x + y);
            operations.Add("-", (x, y) => x - y);
            operations.Add("*", (x, y) => x * y);
            operations.Add("/", (x, y) => x / y);
            operations.Add("^", (x, y) => Math.Pow(x, y));
        }
        public static double Calculate(string expr)
        {
            string[] arguments = expr.Split(' ');
            double operA = double.Parse(CheckDouble(arguments[0]));
            double operB = double.Parse(CheckDouble(arguments[2]));
            string operP = arguments[1];
            return operations[operP](operA, operB);
        }
        public static void Main(string[] args)
        {
            try
            {
                //Console.WriteLine("Поиск решения начался...");
                //FindAnswer();
                //Console.WriteLine("Поиск решения закончился");
                Console.WriteLine("Поиск решения начался...");
                CheckAnswers();
                Console.WriteLine("Поиск решения закончился");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }


        }
        public static string CheckDouble(string str)
        {
            str = str.Replace('.', ',');
            return str;
        }
        public static void CheckAnswers()
        {
            string[] answers = ReadAllLines(ExprAnswPath);
            string[] checker = ReadAllLines(ExprChecker);
            string[] result = new string[checker.Length + 1];
            double a, c;
            int count = 0;
            for (int i = 0; i < checker.Length; i++)
            {
                if (GetDoubleNum(answers[i], out a) && GetDoubleNum(checker[i], out c))
                {
                    if (Math.Abs(a - c) < 0.001)
                    {
                        result[i] = "OK";
                    }
                    else
                    {
                        result[i] = "Error";
                        count++;
                    }
                }
                else
                {
                    result[i] = "Error";
                    count++;
                }
            }
            result[result.Length - 1] = $"Ошибок/несовпадений значений:{count}";
            WriteAllLines(Result,result);
        }
        public static bool GetDoubleNum(string s, out double n)
        {
            return double.TryParse(CheckDouble(s), out n);
        }

        public static void FindAnswer()
        {
            try
            {
                string[] expr = ReadAllLines(ExprPath);
                string[] answ = new string[expr.Length];
                int i = 0;
                foreach (var item in expr)
                {
                    answ[i] = $"{Calculate(item):f3}";
                    i++;
                }
                WriteAllLines(ExprAnswPath, answ);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Ошибка ввода/вывода");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("У вас нет разрешения на создание файла.");
            }
            catch (System.Security.SecurityException ex)
            {
                Console.WriteLine("Ошибка безопасности.");
            }

        }
        public static void WriteAllLines(string path, string[] str)
        {
            File.WriteAllLines(path, str);
        }
        public static string[] ReadAllLines(string path)
        {
            string[] str;
            str = File.ReadAllLines(path);

            return str;
        }
    }
}