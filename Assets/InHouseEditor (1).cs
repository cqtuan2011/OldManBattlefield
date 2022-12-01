using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

public class InHouseEditor : MonoBehaviour
    {
        [MenuItem("InHouseSDK/Take Screenshot", false, 1)]
        static void TakeScreenshot()
        {
            double unixTimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string nameFile = "Screenshot_" + Screen.width + "x" + Screen.height + "- {0}_{1}_{2}_{3}" + ".png";
            nameFile = string.Format(nameFile, DateTime.Now.Day,
                DateTime.Now.Month, DateTime.Now.Year, unixTimeStamp.ToString());
            string filePath = Application.dataPath.Remove(Application.dataPath.Length - 7) + "/Screenshot";
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                AssetDatabase.Refresh();
            }
            ScreenCapture.CaptureScreenshot(filePath + "/" + nameFile);
        }

        [MenuItem("InHouseSDK/Delete/Data Folder", false, 4)]
        static void DeleteDataFolder()
        {
            foreach (var directory in Directory.GetDirectories(Application.persistentDataPath))
            {
                DirectoryInfo data_dir = new DirectoryInfo(directory);
                data_dir.Delete(true);
            }

            foreach (var file in Directory.GetFiles(Application.persistentDataPath))
            {
                FileInfo file_info = new FileInfo(file);
                file_info.Delete();
            }

            Caching.ClearCache();
        }

        [MenuItem("InHouseSDK/Delete/PlayPref", false, 1)]
        static void DeleteAllPlayPref()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log(" ======== Delete All PlayPref ======== ");
        }

        [MenuItem("InHouseSDK/Open Folder/Persistent Data ", false, 4)]
        static void OpenDataFolder()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("InHouseSDK/Open Folder/Screenshot", false, 4)]
        static void OpenScreenShotFolder()
        {
            string filePath = Application.dataPath.Remove(Application.dataPath.Length - 7) + "/Screenshot";
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                AssetDatabase.Refresh();
            }
            EditorUtility.RevealInFinder(filePath);
        }

        [MenuItem("InHouseSDK/Open Scene/PlayerName &1", false, 5)]
        static void OpenScenePlayerName()
        {
            OpenScene("PlayerName");
        }

        [MenuItem("InHouseSDK/Open Scene/Game &2", false, 5)]
        static void OpenSceneGame()
        {
            OpenScene("Game");
        }
        [MenuItem("InHouseSDK/Open Scene/Menu &3", false, 5)]
        static void OpenSceneMenu()
        {
            OpenScene("Menu");
        }

        static void OpenScene(string sceneName)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity");
            }
        }
    }

#endif
