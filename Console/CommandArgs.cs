using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flawliz.Console
{
    public class CommandArgs : Command
    {
        private System.Action<string[]> Action { get; set; }

        public CommandArgs(System.Action<string[]> action)
        {
            Action = action;
        }

        public override void Execute(string[] args)
        {
            Action(args);
        }
    }

}