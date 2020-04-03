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
using DistSysACWClient.Extensions;
using Newtonsoft.Json;

namespace DistSysACWClient
{
    class Client
    {
        static string CommandHandlerNamespace = "DistSysACWClient.CommandHandlers.";

        static void Main(string[] args)
        {
            UserClient client = new UserClient("http://distsysacw.azurewebsites.net/6170585/api/");
            Console.WriteLine("Hello. What would you like to do?");

            string userInput = "";
            do
            {
                userInput = Console.ReadLine();
                Console.Clear();
                Console.WriteLine(userInput);

                if (userInput == "Exit")
                    continue;

                var command = userInput.Split(" ");

                if (command.Length < 2)
                {
                    Console.WriteLine("All commands have two parts at minimum. Please try again.");
                    continue;
                }

                var classInfo = Assembly.GetCallingAssembly().GetType(CommandHandlerNamespace + command[0] + "CommandHandler");
                if (classInfo == null)
                {
                    Console.WriteLine($"Couldn't resolve the first part of the command path \"{command[0]}\". Please try again.");
                    continue;
                }

                var constructionMethod = classInfo.GetMethod("GetInstance", BindingFlags.Static | BindingFlags.Public);

                if (constructionMethod == null)
                {
                    Console.WriteLine($"Implementation Error: No GetInstance method for the {command[0]} command handler. Please try again.");
                    continue;
                }

                var methodInfo = classInfo.GetMethod(command[1], BindingFlags.Instance | BindingFlags.Public);
                if (methodInfo == null)
                {
                    Console.WriteLine($"Couldn't resolve the second part of the command path \"{command[1]}\". Please try again.");
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
                }
                else
                {
                    methodInfo.Invoke(constructionMethod.Invoke(null, new object[] { client }), parameters.ToArray());
                }

                Console.WriteLine("What would you like to do next ?");
            } while (userInput != "Exit");
        }
    }
}
