using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flawliz.Console
{
    public abstract class Command
    {
        public abstract void Execute(string[] args);
    }

}