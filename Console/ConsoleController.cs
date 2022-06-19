using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Flawliz.Console
{
    public class ConsoleController : Singleton
    {
        public static ConsoleController Instance { get { return Instance<ConsoleController>(); } }
        private ConsoleView View { get; set; }
        private bool VisibleView { get; set; }

        private Dictionary<string, Command> commands = new Dictionary<string, Command>();
        private List<string> commands_prev = new List<string>();
        private int idx_commands;

        protected override void Initialize()
        {
            base.Initialize();
            var prefab = Resources.Load<ConsoleView>(nameof(ConsoleView));
            View = Instantiate(prefab).GetComponent<ConsoleView>();
            View.SetVisible(false);
            DontDestroyOnLoad(View.gameObject);
            VisibleView = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
                ToggleView();
            }

            if (!VisibleView)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                TryExecuteCommand();
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                AutofillSuggestion();
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                SetCommandIndex(idx_commands - 1);
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                SetCommandIndex(idx_commands + 1);
            }
        }

        private void TryExecuteCommand()
        {
            var input = View.Input;
            var args = input.Split(' ');
            if (commands.ContainsKey(input))
            {
                commands[args[0]].Execute(args);
                View.WriteMessage(string.Format("> {0}", input), "");
            }
            else
            {
                View.WriteMessage("Invalid command:", string.Format("{0}", input));
            }

            commands_prev.Add(input);
            idx_commands = commands_prev.Count - 1;

            View.Input = "";
            View.FocusInputField();
        }

        public void LogError(string msg)
        {
            View.WriteMessage("ERROR", msg);
        }

        private void ToggleView()
        {
            VisibleView = !VisibleView;
            View.SetVisible(VisibleView);
        }

        public string GetSuggestion(string input)
        {
            return input.Length == 0 ? "" : commands.Keys.Where(name => name.ToLower().StartsWith(input.ToLower())).FirstOrDefault() ?? "";
        }

        private void AutofillSuggestion()
        {
            var suggestion = GetSuggestion(View.Input);
            if (!string.IsNullOrEmpty(suggestion))
            {
                View.SetInput(suggestion);
            }
        }

        private void SetCommandIndex(int idx)
        {
            idx_commands = Mathf.Clamp(idx, 0, commands_prev.Count - 1);
            if(commands_prev.Count > 0)
            {
                View.SetInput(commands_prev[idx_commands]);
            }
        }

        #region COMMANDS
        public void RegisterCommand(string name, System.Action action)
        {
            RegisterCommand(name, new CommandNoArgs(action));
        }

        public void RegisterCommand(string name, System.Action<string[]> action)
        {
            RegisterCommand(name, new CommandArgs(action));
        }

        private void RegisterCommand(string name, Command command)
        {
            if (!commands.ContainsKey(name))
            {
                commands.Add(name, command);
            }
            else
            {
                print("Command already exists: " + name);
            }
        }
        #endregion
    }
}