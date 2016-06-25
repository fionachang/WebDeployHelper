using System;
using System.Text;

namespace WebDeployHelper
{
    class Util
    {
        #region Helper functions

        public static void ConsoleWriteWithColor(String content, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(content);
            Console.ResetColor();
        }

        public static void IgnoreException()
        {
        }

        public static void DisplayWarning(string content, Exception e)
        {
            ConsoleWriteWithColor(content, ConsoleColor.Red);
            throw new InvalidOperationException(content, e);
        }

        public static void DisplayDone(string content)
        {
            ConsoleWriteWithColor(content + "\n", ConsoleColor.Green);
        }

        public static void DisplayEndMessage()
        {
            DisplayDone("All Done!");
            Console.WriteLine("Have a nice day :)");
        }

        public static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    password.Append(info.KeyChar);
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.Length--;
                    }
                }
                info = Console.ReadKey(true);
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password.ToString();
        }

        #endregion
    }
}
