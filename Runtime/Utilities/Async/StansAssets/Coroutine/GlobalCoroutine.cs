using System;
using System.Collections;
using UnityEngine;
using Utilities.Patterns;

namespace Utilities.Async
{
    internal class GlobalCoroutine : MonoSingleton<GlobalCoroutine>
    {
        public void StartInstruction(YieldInstruction instruction, Action action)
        {
            StartCoroutine(RunActionAfterInstruction(instruction, action));
        }

        private IEnumerator RunActionAfterInstruction(YieldInstruction instruction, Action action)
        {
            yield return instruction;
            action.Invoke();
        }

        public void StartInstruction(CustomYieldInstruction instruction, Action action)
        {
            StartCoroutine(RunActionAfterInstruction(instruction, action));
        }

        private IEnumerator RunActionAfterInstruction(CustomYieldInstruction instruction, Action action)
        {
            yield return instruction;
            action.Invoke();
        }
    }
}
