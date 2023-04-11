namespace Utility.Constants
{
    public static class TimelineTrackNames
    {
        public const string CinemachineTrackName = "CinemachineTrack";
        public const string RefereeAnimationTrackName = "RefereeAnimation";

        public static string GetRightTeamAnimationTrackName(int playerIndex) =>
            $"RightTeamPlayer{playerIndex + 1}Animation";
        
        public static string GetLeftTeamAnimationTrackName(int playerIndex) =>
            $"LeftTeamPlayer{playerIndex + 1}Animation";
    }
}