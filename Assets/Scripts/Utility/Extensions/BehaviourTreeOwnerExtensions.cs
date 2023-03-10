using System.Collections.Generic;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;

namespace Utility.Extensions
{
    public static class BehaviourTreeOwnerExtensions
    {
        public static void AddBinds(this BehaviourTreeOwner behaviorTreeOwner, Dictionary<string, Object> variables)
        {
            BehaviourTree behaviourTree = behaviorTreeOwner.behaviour;
            IBlackboard blackboard = behaviourTree.blackboard;
            
            foreach (KeyValuePair<string, Object> variable in variables)
            {
                blackboard.GetVariable(variable.Key).value = variable.Value;
            }
        }
    }
}