using System.Linq;
using UnityEngine;
using VContainer;

namespace Factory.DebugWindows
{
    public class BeltsSegmentDebugWindow : MonoBehaviour
    {
        [Inject]
        private BeltsSegmentSystem beltsSegmentSystem;

        private Vector2 scroll;

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 350, 500), GUI.skin.box);

            GUILayout.Label($"Segments: {beltsSegmentSystem.Segments.Count()}");

            scroll = GUILayout.BeginScrollView(scroll);

            foreach (var segment in beltsSegmentSystem.Segments)
            {
                GUILayout.BeginVertical(GUI.skin.box);

                GUILayout.Label($"ID: {segment.Id}");
                GUILayout.Label($"Belts: {segment.Belts.Length}");
                GUILayout.Label($"Items: {segment.Data.count}");

                GUILayout.EndVertical();
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
}