using System;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Utility.Extensions
{
    public static class PlayableDirectorExtensions
    {
        public static void BindAnimator(this PlayableDirector playableDirector, string trackName, Animator animator)
        {
             BindObject(playableDirector, trackName, animator);
        }
        
        public static void BindCinemachineBrain(this PlayableDirector playableDirector, string trackName, CinemachineBrain cinemachineBrain)
        {
            BindObject(playableDirector, trackName, cinemachineBrain);
        }

        private static void BindObject(this PlayableDirector playableDirector, string trackName, UnityEngine.Object bind)
        {
            TimelineAsset timelineAsset = playableDirector.playableAsset as TimelineAsset
                                          ?? throw new NullReferenceException("no timeline asset in playable director");

            TrackAsset trackAsset = timelineAsset.GetOutputTracks().FirstOrDefault(track => track.name == trackName);
            
            playableDirector.SetGenericBinding(trackAsset, bind);

        }
    }
}