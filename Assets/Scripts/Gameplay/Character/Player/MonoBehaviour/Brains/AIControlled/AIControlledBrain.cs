using System.Collections.Generic;
using Modules.MonoBehaviour;
using NodeCanvas.BehaviourTrees;
using Scene;
using Scene.Ring;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;

namespace Gameplay.Character.Player.MonoBehaviour.Brains.AIControlled
{
    public class AIControlledBrain : SwitchableComponent
    {
       
        [SerializeField] private BehaviourTreeOwner _behaviourTreeOwner;

        public AI CurrentAIType { get; private set; }
        
       public void Initialize(PlayerFacade[] allies, Ring oppositeRing, Ring playerRing, CourtDimensions courtDimensions, PlayerFacade[] oppositeTeam)
        {
            _behaviourTreeOwner.AddBinds(new Dictionary<string, object>
            {
                [BehaviourTreeVariableNames.AITypeVariableName] = CurrentAIType,
                [BehaviourTreeVariableNames.AlliesVariableName] = allies,
                [BehaviourTreeVariableNames.OppositeRingVariableName] = oppositeRing,
                [BehaviourTreeVariableNames.PlayerRingVariableName] = playerRing,
                [BehaviourTreeVariableNames.CourtDimensionsVariableName] = courtDimensions,
                [BehaviourTreeVariableNames.OppositeTeamVariableName] = oppositeTeam
            });
        }
       
       private void OnDisable()
       {
           _behaviourTreeOwner.Disable();
       }

       public void Enable(AI aiType)
       {
           base.Enable();
           CurrentAIType = aiType;
           _behaviourTreeOwner.Enable();
       }
    }
}