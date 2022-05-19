using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flawliz.Console
{
    public class CommandNoArgs : Command
    {
        private System.Action Action { get; set; }

        public CommandNoArgs(System.Action action)
        {
            Action = action;
        }

        public override void Execute(string[] args)
        {
            Action();   
        }
    }

}