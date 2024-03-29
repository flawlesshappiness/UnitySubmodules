using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Flawliz.Console
{
    public class ConsoleController : Singleton
    {
        public static ConsoleController Instance { get { return Instance<ConsoleController>(); } }
        public bool Enabled { get; set; }
        public event System.Action<bool> onToggle;

        private ConsoleView View { get; set; }
        private bool VisibleView { get; set; }

        private Dictionary<string, Command> commands = new Dictionary<string, Command>();
        private List<string> commands_queue = new List<string>();
        private int idx_commands;

        protected override void Initialize()
        {
            base.Initialize();
            var prefab = Resources.Load<ConsoleView>(nameof(ConsoleView));
            View = Instantiate(prefab).GetComponent<ConsoleView>();
            View.SetVisible(false);
            DontDestroyOnLoad(View.gameObject);
            VisibleView = false;

            // Input
            var control = new ConsoleControls();
            var actions = control.Actions;
            actions.Toggle.started += c => ToggleView();
            actions.Enter.started += c => TryExecuteCommand();
            actions.Autofill.started += c => AutofillSuggestion();
            actions.Up.started += c => SetCommandIndex(idx_commands - 1);
            actions.Down.started += c => SetCommandIndex(idx_commands + 1);
            control.Enable();

            // Commands
            RegisterCommand("Clear", View.ClearElements);
        }

        private void TryExecuteCommand()
        {
            if (!VisibleView) return;

            var input = View.Input;
            var args = input.Split(' ');
            if (args.Length > 0 && commands.ContainsKey(args[0]))
            {
                View.WriteMessage(string.Format("> {0}", input), "");
                commands[args[0]].Execute(args);
            }
            else
            {
                View.WriteMessage("Invalid command:", string.Format("{0}", input));
            }

            commands_queue.Add(input);
            idx_commands = commands_queue.Count;

            View.Input = "";
            View.FocusInputField();
        }

        public void Log(string msg)
        {
            View.WriteMessage("", msg);
        }

        public void LogError(string msg)
        {
            View.WriteMessage("ERROR", msg);
        }

        public void LogOutput(string output)
        {
            View.SetPreviousOutput(output);
        }

        private void ToggleView()
        {
            if (!Enabled) return;

            VisibleView = !VisibleView;
            View.SetVisible(VisibleView);
            onToggle?.Invoke(VisibleView);
        }

        public string GetSuggestion(string input)
        {
            return input.Length == 0 ? "" : commands.Keys.Where(name => name.ToLower().StartsWith(input.ToLower())).FirstOrDefault() ?? "";
        }

        private void AutofillSuggestion()
        {
            if (!VisibleView) return;

            var suggestion = GetSuggestion(View.Input);
            if (!string.IsNullOrEmpty(suggestion))
            {
                View.SetInput(suggestion);
            }
        }

        private void SetCommandIndex(int idx)
        {
            if (!VisibleView) return;

            idx_commands = Mathf.Clamp(idx, 0, commands_queue.Count - 1);
            if(commands_queue.Count > 0)
            {
                View.SetInput(commands_queue[idx_commands]);
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