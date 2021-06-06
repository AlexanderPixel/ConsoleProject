using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleProject
{
    public class ConsoleManager
    {
        private DirectoryInfo _directoryInfo;
        private ConsoleCommand _command;

        public DirectoryInfo CurrentDirectory
        {
            get => _directoryInfo;
            set => _directoryInfo = value;
        }

        public ConsoleManager()
        {
            _directoryInfo = new DirectoryInfo("/Users/aleksandrtkacenko/Desktop/Шаг/C#/ConsoleProject");
            _command = new ConsoleCommand();
        }

        public void StartMenu()
        {
            string input = String.Empty;
            while (true)
            {
                Console.Write($"{_directoryInfo.Name}:   ");
                input = Console.ReadLine();
                _command.Parse(input);
                MakeAction();
                Console.WriteLine();
            }
        }

        private void MakeAction()
        {
            List<string> attributes = _command.GetAttributes();
            
            switch (_command.Command.ToString())
            {
                case "cd":
                    CdMethod(new DirectoryInfo(attributes[0]));
                    break;
                case "cls":
                    CLSMethod();
                    break;
                case "help":
                    HelpMethod();
                    break;
                
            }
        }

        public void HelpMethod()
        {
            Console.WriteLine("Type cls to clear console");
            Console.WriteLine("Type dir to see containing");
            Console.WriteLine("Type cd + folder name to move");
            Console.WriteLine("Type copy + folder name to copy");
            Console.WriteLine("Type del + folder name to delete");
            Console.WriteLine("Type mkdir + folder name to create");
        }

        public void CLSMethod()
        {
            Console.Clear();
        }

        public void DirMethod(DirectoryInfo obj)
        {
            Console.WriteLine($"{"Name",-15}      {"Time",-10}       Length");
            foreach (var d in obj.GetDirectories())
            {
                Console.WriteLine($"{d.Name,-15}  DIR {d.CreationTime.ToShortTimeString(),-10}");
            }
            foreach (var f in obj.GetFiles())
            {
                Console.WriteLine($"{f.Name,-15} {f.Extension,4} {f.CreationTime.ToShortTimeString(),-10} {f.Length/1024.0:0.00} KB");
            }
        }

        public void CdMethod(DirectoryInfo obj)
        {
            _directoryInfo = new DirectoryInfo(obj.Name);
            Directory.SetCurrentDirectory(_directoryInfo.Name);
        }

        public void CopyMethod(DirectoryInfo source, DirectoryInfo dest)
        {
            _directoryInfo = new DirectoryInfo(source.Name);
            DirectoryInfo destination = new DirectoryInfo(dest.Name);
            
            if (!System.IO.Directory.Exists(destination.Name))
            {
                System.IO.Directory.CreateDirectory(destination.Name);
            }
            String[] files = Directory.GetFiles(_directoryInfo.Name);
            String[] directories = Directory.GetDirectories(_directoryInfo.Name);
            foreach (string s in files)
            {
                File.Copy(s, Path.Combine(destination.Name, Path.GetFileName(s)), true);     
            }
            foreach(string d in directories)
            {
                Directory.Move(Path.Combine(_directoryInfo.Name, Path.GetFileName(d)), Path.Combine(destination.Name, Path.GetFileName(d)));
            }
        }

        public void DelMethod(FileInfo obj)
        {
            if(File.Exists(obj.Name) == true)
                File.Delete(obj.Name);
        }

        public void RmdirMethod(DirectoryInfo obj)
        {
            if (Directory.Exists(obj.Name) == true)
            {
                _directoryInfo = new DirectoryInfo(obj.Name);
                _directoryInfo.Delete();
            }
            else
            {
                
            }
        }

        public void MkdirMethod(DirectoryInfo obj)
        {
            _directoryInfo = new DirectoryInfo(obj.Name);
            _directoryInfo.CreateSubdirectory("New folder");
        }

        public void MoveMethod(FileInfo file, DirectoryInfo destination)
        {
            if (Directory.Exists(destination.Name) == true)
                File.Move(file.Name, destination.Name);
        }
        
        public void MoveMethod(DirectoryInfo source, DirectoryInfo destination)
        {
            if (Directory.Exists(destination.Name) == true)
                source.MoveTo(destination.Name);
        }
        
    }
}