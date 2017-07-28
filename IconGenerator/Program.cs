using CommandLineParser.Arguments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IconGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var arg = new FileArgument('i', "inputFile")
            {
                FileMustExist = true
            };
            var parser = new CommandLineParser.CommandLineParser
            {
                AcceptEqualSignSyntaxForValueArguments = true,
                AcceptHyphen = true,
                AcceptSlash = true
            };
            parser.Arguments.Add(arg);

            try
            {
                parser.ParseCommandLine(args);
                if (!arg.Parsed)
                    throw new NotSupportedException("No argument provided.");

                var icons = Collect(arg.Value.FullName).ToList();
                Generate(icons);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process was aborted mid-way. Some icons may have been generated.");
                Console.WriteLine(e.Message);
                return;
            }
            Console.WriteLine("All done!");
        }

        private static void Generate(List<Icon> icons)
        {
            foreach (var ico in icons)
            {
                ico.Generate();
                Console.WriteLine($"Generated '{ico.OutputPath}'.");
            }
        }

        private static IEnumerable<Icon> Collect(string inputFile)
        {
            var fileDir = new FileInfo(inputFile).DirectoryName;
            var lines = File.ReadAllLines(inputFile);

            var variables = lines.TakeWhile(l => l != "---").ToArray();
            var transforms = lines.SkipWhile(l => l != "---").Skip(1).ToArray();
            if (variables.Any() && !transforms.Any())
            {
                // no "---" line, thus everything is transforms
                transforms = variables;
                variables = new string[0];
            }

            var vars = new Dictionary<string, string>();
            foreach (var v in variables)
            {
                var idx = v.IndexOf('=');
                if (idx == -1)
                    throw new NotSupportedException("Key/Value pair does not contain required char '='.");
                var name = v.Substring(0, idx);
                var value = v.Substring(idx + 1);
                name = $"%{name}%";
                if (vars.ContainsKey(name))
                    throw new NotSupportedException($"Variable '{name}' already declared.");

                vars.Add(name, value);
            }

            foreach (var transform in transforms)
            {
                if (transform.StartsWith("#"))
                    continue;

                var line = transform.Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (!line.Contains("->"))
                    throw new NotSupportedException($"Missing '->' in line '{line}'.");

                var split = line.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length != 2)
                    throw new NotSupportedException($"More than one '->' in line '{line}'.");

                var input = split[0];
                var output = split[1];

                string[] inputFiles;
                if (input.Contains("*"))
                {
                    var idx = input.IndexOf('*');
                    var dir = Path.Combine(fileDir, input.Substring(0, idx));
                    var filter = input.Substring(idx);
                    inputFiles = Directory.GetFiles(dir, filter);
                }
                else
                {
                    inputFiles = new[] { Path.Combine(fileDir, input) };
                }

                var lastBracket = output.LastIndexOf('(');
                var path = Path.Combine(fileDir, output.Substring(0, lastBracket).Trim());
                var cfg = output.Substring(lastBracket + 1);
                cfg = cfg.Substring(0, cfg.IndexOf(')'));
                string[] options;
                if (cfg.Contains(","))
                {
                    options = cfg.Split(',');
                }
                else
                {
                    options = new[] { cfg };
                }
                int w = -1, h = -1;
                int margin = 0;
                foreach (var o in options)
                {
                    var opt = o.Trim();
                    if (opt.Contains("x"))
                    {
                        // size in WxH
                        var spl = opt.Split('x');
                        w = int.Parse(spl[0]);
                        h = int.Parse(spl[1]);
                    }
                    else if (opt.EndsWith("%"))
                    {
                        // margin
                        margin = int.Parse(opt.Substring(0, opt.Length - 1));
                        if (margin > 49)
                            throw new NotSupportedException("Margin must be between 0 and 49 as it is applied equally from all sides.");
                    }
                }
                if (w == -1 || h == -1)
                    throw new NotSupportedException("Width must be explicitely set!");

                foreach (var v in vars)
                {
                    path = path.Replace(v.Key, v.Value);
                }

                if (inputFiles.Length > 1)
                {
                    if (!path.Contains("*"))
                        throw new NotSupportedException("Output path must also contain '*' when input path contains '*'.");
                }

                foreach (var f in inputFiles)
                {
                    if (!File.Exists(f))
                        throw new NotSupportedException($"Input file '{f}' not found.");

                    var name = Path.GetFileNameWithoutExtension(f);
                    var currPath = path.Replace("*", name);
                    yield return new Icon(f, currPath, w, h, margin);
                }
            }
        }
    }
}