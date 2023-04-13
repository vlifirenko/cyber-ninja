using CyberNinja.Models;
using UnityEngine;

namespace CyberNinja.Services
{
    public class SaveService
    {
        public static void Save(GameData gameData)
        {
            var json = JsonUtility.ToJson(gameData);
            Debug.Log(json);
            PlayerPrefs.SetString("SAVE", json);
        }

        public static GameData Load()
        {
            var json = PlayerPrefs.GetString("SAVE", null);
            var gameData = JsonUtility.FromJson<GameData>(json);
            if (gameData == null)
                gameData = new GameData();
            else
                Debug.Log(json);

            return gameData;
        }
    }
}