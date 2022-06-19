using System;
using System.Collections.Generic;

namespace TypisheApi.Input
{
    public static class ApplicationCommander
    {
        public delegate void CommandExecutionHandler(string command, ref bool executed);
        public static event CommandExecutionHandler OnCommandExecution;

        public static Dictionary<string, Action> Commands = new Dictionary<string, Action>();

        public static bool ExecuteCommand(string commandName)
        {
            var success = Commands.TryGetValue(commandName, out var action);
            if (!success)
            {
                var successRequest = false;
                OnCommandExecution?.Invoke(commandName, ref successRequest);

                return successRequest;
            }

            try
            {
                action();
                return true;
            }

            catch
            {
                return false;
            }
        }
    }
}
