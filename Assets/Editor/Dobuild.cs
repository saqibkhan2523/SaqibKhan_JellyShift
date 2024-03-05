using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Dobuild : MonoBehaviour
{
    public static void BuildAndroidAPK()
    {
        // Define the scenes to include in the build
        string[] scenes = {
            "Assets/Scenes/Main Menu.unity",
            "Assets/Scenes/Levels/Level1.unity",
            "Assets/Scenes/Levels/Level2.unity",
            "Assets/Scenes/Levels/Level3.unity",
            "Assets/Scenes/Levels/Level4.unity",
            "Assets/Scenes/Levels/Level5.unity",
            "Assets/Scenes/Levels/Level6.unity",
            "Assets/Scenes/Levels/Level7.unity",
            "Assets/Scenes/Levels/Level8.unity",
            "Assets/Scenes/Levels/Level9.unity",
            "Assets/Scenes/Levels/Level10.unity",
            "Assets/Scenes/Levels/Level11.unity",
            "Assets/Scenes/Levels/Level12.unity",
            "Assets/Scenes/Levels/Level13.unity",
            "Assets/Scenes/Levels/Level14.unity",
            "Assets/Scenes/Levels/Level15.unity",
            "Assets/Scenes/Levels/Level16.unity",
            "Assets/Scenes/Levels/Level17.unity",
            "Assets/Scenes/Levels/Level18.unity",
            "Assets/Scenes/Levels/Level19.unity",
            "Assets/Scenes/Levels/Level20.unity",

        };

        // Define the output path for the APK
        string outputPath = "BuildsOutput/YourGameName.apk";

        // Perform the build
        BuildPipeline.BuildPlayer(scenes, outputPath, BuildTarget.Android, BuildOptions.None);
    }
}


//Assets/Scenes/Gameplay.unity
//Assets / Scenes / Level1.unity
//Assets / Scenes / SampleScene 1.unity