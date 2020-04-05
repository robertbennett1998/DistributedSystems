using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DistSysACWClient.Attributes;
using DistSysACWClient.Extensions;
using Newtonsoft.Json;

namespace DistSysACWClient
{
    class Client
    {
        const string CommandHandlerSuffix = "CommandHandler";

        static void Main(string[] args)
        {
            UserClient client = new UserClient();

            if (client.BaseUri == null)
                client.BaseUri = "http://distsysacw.azurewebsites.net/6170585/api/";

            Console.WriteLine("Hello. What would you like to do?");

            string userInput = "";
            do
            {
                userInput = Console.ReadLine();
                Console.Clear();
                Console.WriteLine(userInput);

                if (userInput == "Exit")
                    continue;

                if (userInput == "Help")
                {
                    PrintCommandList();
                    Console.WriteLine("What would you like to do next ?");
                    continue;
                }

                var command = userInput.Split(" ");

                if (command.Length < 2)
                {
                    Console.WriteLine("Most commands have two parts at minimum. Did you mean \"Exit\"? Please try again.");
                    continue;
                }

                var classInfo = Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(t => t.Name == command[0] + CommandHandlerSuffix);
                if (classInfo == null)
                {
                    Console.WriteLine($"Couldn't resolve the first part of the command path \"{command[0]}\". Please try again.");
                    continue;
                }

                //TODO: Store an instance in a dict so they aren't constantly created...
                var constructionMethod = classInfo.GetMethod("GetInstance", BindingFlags.Static | BindingFlags.Public);

                if (constructionMethod == null)
                {
                    Console.WriteLine($"Implementation Error: No GetInstance method for the {command[0]} command handler. Please try again.");
                    continue;
                }

                var methodInfo = classInfo.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.GetCustomAttribute<CommandAttribute>() != null).FirstOrDefault(m => m.Name == command[1]);
                if (methodInfo == null)
                {
                    Console.WriteLine($"{command[0]} has no command \"{command[1]}\". Please try again or type \"Help\" to display a list of available commands.");
                    continue;
                }


                if (command.Length != (methodInfo.GetParameters().Length + 2))
                {
                    Console.WriteLine($"Expected {methodInfo.GetParameters().Length} parameters. But {command.Length - 2} were given. Please try again.");
                    continue;
                }

                List<object> parameters = new List<object>();
                foreach (var commandParameter in command.Skip(2).Take(command.Length - 2))
                    parameters.Add((object)commandParameter);

                if (methodInfo.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) as AsyncStateMachineAttribute != null)
                {
                    var commandTask = methodInfo.InvokeAsync(constructionMethod.Invoke(null, new object[] { client }), parameters.ToArray());

                    if (!commandTask.IsCompleted)
                        Console.WriteLine("...please wait...");

                    while (!commandTask.IsCompleted) ;

                    commandTask.ContinueWith((task) =>
                    {
                        if (task.Exception != null)
                        {
                            Console.WriteLine($"Error: {task.Exception.Message}");
                        }
                    });
                }
                else
                {
                    try
                    {
                        methodInfo.Invoke(constructionMethod.Invoke(null, new object[] { client }), parameters.ToArray());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }

                Console.WriteLine("What would you like to do next ?");
            } while (userInput != "Exit");
        }

        private static void PrintCommandList()
        {
            var commandHandlers = Assembly.GetCallingAssembly().GetTypes().Where(t => t.Name.EndsWith(CommandHandlerSuffix));
            foreach (var commandHandler in commandHandlers)
            {
                var commandMethods = commandHandler.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(m => m.GetCustomAttribute<CommandAttribute>() != null);

                if (commandMethods.Count() > 0)
                    Console.WriteLine($"{commandHandler.Name.Replace(CommandHandlerSuffix, "")}:");

                foreach (var commandMethod in commandMethods)
                {
                    Console.WriteLine($"\t[{commandHandler.Name.Replace(CommandHandlerSuffix, "")}] {commandMethod.Name} - {commandMethod.GetCustomAttribute<CommandAttribute>().Description}");
                }
            }

            Console.WriteLine("Exit - Exit the client application.");
        }
    }
}
