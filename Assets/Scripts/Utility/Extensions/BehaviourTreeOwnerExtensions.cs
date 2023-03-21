using System.Collections.Generic;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;

namespace Utility.Extensions
{
    public static class BehaviourTreeOwnerExtensions
    {
        public static void AddBinds(this BehaviourTreeOwner behaviorTreeOwner, Dictionary<string, object> variables)
        {
            BehaviourTree behaviourTree = behaviorTreeOwner.behaviour;
            IBlackboard blackboard = behaviourTree.blackboard;
            
            foreach (KeyValuePair<string, object> variable in variables)
            {
                blackboard.GetVariable(variable.Key).value = variable.Value;
            }
        }
        
        public static void Enable(this BehaviourTreeOwner behaviorTreeOwner)
        {
            behaviorTreeOwner.enabled = true;
        }
        
        public static void Disable(this BehaviourTreeOwner behaviorTreeOwner)
        {
            behaviorTreeOwner.enabled = false;
        }
    }
}