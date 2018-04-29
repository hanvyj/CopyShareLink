using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileSharePath
{
    class Program
    {
        private const string V = "%1";

        [STAThread]
        static void Main(string[] args)
        {
            Program p = new Program();

            if (args.Length == 0)
            {
                Console.WriteLine("Do you wish to add or remove the contect menu? Enter 'a' or 'r'");

                var result = Console.ReadKey();
                Console.WriteLine("");

                if (result.Key == ConsoleKey.A)
                {
                    p.AddOption();
                }
                else if (result.Key == ConsoleKey.R)
                {
                    p.RemoveOption();
                }
                else
                {
                    Console.WriteLine("Not recognized");
                }
                Console.WriteLine("Press any key to quit");
                Console.ReadKey();
            }
            else
            {
                var file = args[0];
                
                file = file.Replace("\\", " /");
                file = file.Replace(" ", "%20");
                
                Clipboard.SetText($"file:///{file}");
            }
        }

        private void RemoveOption()
        {
            try
            {
                RegistryKey shell = Registry.CurrentUser.OpenSubKey(@"Software\Classes\*\shell\CopyShare", true);
                if (shell != null)
                {
                    Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\*\shell\CopyShare");
                }
                Console.WriteLine("Removed context menu successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine($"There was an error: {e.Message}");
            }
        }
        
        public void AddOption()
        {
            try
            {
                RegistryKey shell = Registry.CurrentUser.OpenSubKey(@"Software\Classes\*\shell\CopyShare", true);
                if (shell == null)
                {
                    shell = Registry.CurrentUser.CreateSubKey(@"Software\Classes\*\shell\CopyShare");
                }
                shell.SetValue(String.Empty, "Copy Share Link");
                var command = Registry.CurrentUser.OpenSubKey(@"Software\Classes\*\shell\CopyShare\command", true);
                if (command == null)
                {
                    command = Registry.CurrentUser.CreateSubKey(@"Software\Classes\*\shell\CopyShare\command");
                }
                command.SetValue(String.Empty, $"\"{System.Reflection.Assembly.GetExecutingAssembly().Location}\" \"%1\"");
                shell.Close();
                command.Close();
                Console.WriteLine("Added context menu successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine($"There was an error: {e.Message}");
            }
}
    }
}
