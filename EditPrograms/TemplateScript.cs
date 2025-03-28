using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ProgramAppEditor
{
    public class ProgramEditor
    {
        private const string DEFAULT_CONFIG_PATH = "programs.json";

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\nProgram Configuration Editor");
                Console.WriteLine("1. View Current Programs");
                Console.WriteLine("2. Add New Program");
                Console.WriteLine("3. Remove Program");
                Console.WriteLine("4. Edit Program");
                Console.WriteLine("5. Build Main Application");
                Console.WriteLine("6. Exit");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewPrograms();
                        break;
                    case "2":
                        AddProgram();
                        break;
                    case "3":
                        RemoveProgram();
                        break;
                    case "4":
                        EditProgram();
                        break;
                    case "5":
                        BuildApplication();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        private static void ViewPrograms()
        {
            var config = LoadConfig();
            Console.WriteLine("\nCurrent Programs:");
            foreach (var program in config["programs"].AsArray())
            {
                Console.WriteLine($"- {program["name"]} (Version: {program["version"]})");
            }
        }

        private static void AddProgram()
        {
            var config = LoadConfig();
            var programs = config["programs"].AsArray();

            Console.Write("Enter Program Name: ");
            var name = Console.ReadLine();

            Console.Write("Enter Icon Filename: ");
            var icon = Console.ReadLine();

            Console.Write("Enter Description: ");
            var description = Console.ReadLine();

            Console.Write("Enter Version: ");
            var version = Console.ReadLine();

            Console.Write("Enter Command Template: ");
            var commandTemplate = Console.ReadLine();

            Console.Write("Requires Removable Drive? (true/false): ");
            var requiresRemovableDrive = bool.Parse(Console.ReadLine());

            Console.Write("Default Checked? (true/false): ");
            var defaultChecked = bool.Parse(Console.ReadLine());

            var newProgram = new JsonObject
            {
                ["name"] = name,
                ["icon"] = icon,
                ["description"] = description,
                ["version"] = version,
                ["commandTemplate"] = commandTemplate,
                ["requiresRemovableDrive"] = requiresRemovableDrive,
                ["defaultChecked"] = defaultChecked
            };

            programs.Add(newProgram);
            SaveConfig(config);
            Console.WriteLine("Program added successfully!");
        }

        private static void RemoveProgram()
        {
            var config = LoadConfig();
            var programs = config["programs"].AsArray();

            Console.Write("Enter Program Name to Remove: ");
            var nameToRemove = Console.ReadLine();

            for (int i = 0; i < programs.Count; i++)
            {
                if (programs[i]["name"].ToString() == nameToRemove)
                {
                    programs.RemoveAt(i);
                    SaveConfig(config);
                    Console.WriteLine("Program removed successfully!");
                    return;
                }
            }

            Console.WriteLine("Program not found.");
        }

        private static void EditProgram()
        {
            var config = LoadConfig();
            var programs = config["programs"].AsArray();

            Console.Write("Enter Program Name to Edit: ");
            var nameToEdit = Console.ReadLine();

            for (int i = 0; i < programs.Count; i++)
            {
                if (programs[i]["name"].ToString() == nameToEdit)
                {
                    Console.WriteLine("Enter new values (leave blank to keep current value)");

                    Console.Write("Icon Filename: ");
                    var icon = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(icon))
                        programs[i]["icon"] = icon;

                    Console.Write("Description: ");
                    var description = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(description))
                        programs[i]["description"] = description;

                    // Add similar input blocks for other properties

                    SaveConfig(config);
                    Console.WriteLine("Program updated successfully!");
                    return;
                }
            }

            Console.WriteLine("Program not found.");
        }

        private static void BuildApplication(string outputPath = null)
        {
            outputPath ??= Path.Combine(Environment.CurrentDirectory, "build");

            // This is a placeholder. In a real scenario, you'd:
            // 1. Copy your main application files
            // 2. Copy the programs.json 
            // 3. Set up any necessary configuration

            Directory.CreateDirectory(outputPath);
            File.Copy("programs.json", Path.Combine(outputPath, "programs.json"), true);

            Console.WriteLine($"Application built to: {outputPath}");
        }

        private static JsonNode LoadConfig(string path = null)
        {
            path ??= DEFAULT_CONFIG_PATH;
            var jsonString = File.ReadAllText(path);
            return JsonNode.Parse(jsonString);
        }

        private static void SaveConfig(JsonNode config, string path = null)
        {
            path ??= DEFAULT_CONFIG_PATH;
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(path, config.ToJsonString(options));
        }
    }
}