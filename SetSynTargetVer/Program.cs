// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SetSynTargetVer
{
    using System;
    using System.IO;
    using NDesk.Options;

    /// <summary>
    /// Utility Program to Set Synergy Target Versions.
    /// </summary>
    class Program
    {
        const string PROGRAM_NAME = "SetSynTargetVer";

        static int Main(string[] args)
        {
            string rootDirectory = string.Empty;
            string targetRuntime = string.Empty;
            string languageCompatibilityLevel = string.Empty;
            bool showHelp = false;

            // Parse our Options
            OptionSet options = new OptionSet()
            {
                { "Root|R=", "The {ROOT} directory to scan for projects to modify.", rt => rootDirectory = rt },
                { "TargetRuntime|TR=","(Optional) The {TARGETRUNTIME} to set projects to.", tr => targetRuntime = tr },
                { "LanguageCompatibilityLevel|LCL=", "(Optional) The {LanguageCompatibilityLevel} to set projects to.", lcl => languageCompatibilityLevel = lcl },
                { "help|?", "Show Help", h => showHelp = h != null }
            };

            try
            {
                options.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write($"{PROGRAM_NAME}: ");
                Console.WriteLine(e.Message);
                Console.WriteLine($"Try `{PROGRAM_NAME} --help' for more information.");
                return 1;
            }

            if (showHelp)
            {
                return _ShowHelp(options);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(rootDirectory) || !Directory.Exists(rootDirectory))
                {
                    Console.WriteLine($"You must provide a valid directory for this tool. Try `{PROGRAM_NAME} --help' for more information.");

                    // Return ERROR_PATH_NOT_FOUND
                    return 3;
                }
                else
                {
                    return SetSynTargetVer.ProcessDirectory(rootDirectory, targetRuntime, languageCompatibilityLevel);
                }
            }
        }

        private static int _ShowHelp(OptionSet options)
        {
            Console.WriteLine($"Usage: {PROGRAM_NAME} [OPTIONS]");
            Console.WriteLine("Assists in setting various Synergy Target Framework options in multiple SYNPROJ files.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            options.WriteOptionDescriptions(Console.Out);
            Console.WriteLine();
            Console.WriteLine("See https://www.synergex.com/docs/#tools/toolsChap1Compilingatraditionalsynergyroutine.htm for Valid Values (Under Runtime Targeting)");
            return 1;
        }
    }
}
