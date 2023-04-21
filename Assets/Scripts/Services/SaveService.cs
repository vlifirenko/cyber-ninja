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
        
        public static void Save(LobbyData lobbyData)
        {
            var json = JsonUtility.ToJson(lobbyData);
            Debug.Log(json);
            PlayerPrefs.SetString("SAVE_LOBBY", json);
        }

        public static GameData LoadGameData()
        {
            var json = PlayerPrefs.GetString("SAVE_GAME", null);
            var gameData = JsonUtility.FromJson<GameData>(json);
            if (gameData == null)
                gameData = new GameData();
            else
                Debug.Log(json);

            return gameData;
        }
        
        public static LobbyData LoadLobbyData()
        {
            var json = PlayerPrefs.GetString("SAVE_LOBBY", null);
            var gameData = JsonUtility.FromJson<LobbyData>(json);
            if (gameData == null)
                gameData = new LobbyData();
            else
                Debug.Log(json);

            return gameData;
        }
    }
}