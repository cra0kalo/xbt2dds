using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

using System.IO;
using System.Text;

namespace xbt2dds
{
internal static class _tmain
{


    private static bool flag_verbose = false;
    private static MOD_XBT2 myParser;
    private static string Me_WorkingPath;
    private static string In_FilePath;
    private static string Out_FolderPath;



    public static void Main()
    {
        Console.WriteLine(" ---Watch_dogs xbt2dds---");
        Console.WriteLine(string.Empty);
        CMDCheck();
        Console.WriteLine(string.Empty);
        Console.WriteLine(string.Empty);
        //' Console.WriteLine("Press enter to exit")
        //' Console.ReadLine()

    }

    public static void CMDCheck()
    {


        // Get the values of the command line in an array
        // Index  Discription
        // 0      Full path of executing prograsm with program name
        // 1      First switch in command in your example
        string[] clArgs = Environment.GetCommandLineArgs();

        if (clArgs.Count() < 4 || clArgs.Count() > 6)
        {
            Console.WriteLine("Usage: xbt2dds -t path/to/texture.xbt path/to/outputfolder");
            Console.WriteLine("");
            Console.WriteLine("Example: xbt2dds -t O:/RamBox/F41F83B5.xbt O:/RamBox/output");
        }
        else
        {

            Console.WriteLine("Starting...");
            flag_verbose = true;

            //set working path
            Me_WorkingPath = Path.GetDirectoryName(clArgs[0]);


            //check input -t followed by it's path
            int ac = 0;
            foreach (var arg in clArgs)
            {
                if (arg == "-t")
                {
                    In_FilePath = clArgs[ac + 1];
                    Out_FolderPath = clArgs[ac + 2];
                    break;
                }
                ac += 1;
            }

            if (string.IsNullOrEmpty(In_FilePath) || string.IsNullOrEmpty(Out_FolderPath))
            {
                Console.WriteLine("Error check input arguments!");
                return;
            }



            //filechecks I/O
            if (!(File.Exists(In_FilePath)))
            {

                //maybe its in the working folder
                if (File.Exists(Path.Combine(Me_WorkingPath, In_FilePath)))
                {
                    In_FilePath = Path.Combine(Me_WorkingPath, In_FilePath);
                }
                else
                {
                    In_FilePath = string.Empty;
                    Console.WriteLine("Error Input file doesn't seem to exist!");
                    return;
                }
            }
            else
            {
                //working folder
                In_FilePath = Path.Combine(Me_WorkingPath, In_FilePath);
            }

            try
            {
                CreatePath(Out_FolderPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return;
            }



            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("InputFile: " + Path.GetFileName(In_FilePath));
            Console.WriteLine("InputFilePath: " + In_FilePath);
            Console.WriteLine("ExportPath: " + Out_FolderPath);


            //Pass to class object to export
            VText("");
            myParser = new MOD_XBT2(In_FilePath, Out_FolderPath);
            //Call export and finish
            myParser.ParseExport();
        }
    }

    public static void VText(string text)
    {
        if (flag_verbose == true)
        {
            Console.WriteLine(text);
        }
    }


    public static void PError(string text)
    {
        Console.Clear();
        Console.WriteLine("----------------------------------------------------------------------------");
        Console.WriteLine("----------------------------------------------------------------------------");
        Console.WriteLine(text);
        Console.WriteLine("----------------------------------------------------------------------------");
        Console.WriteLine("----------------------------------------------------------------------------");
    }



    public static void CreatePath(string pathName)
    {
        if (!(Directory.Exists(pathName)))
        {
            Directory.CreateDirectory(pathName);
        }
    }




}

}