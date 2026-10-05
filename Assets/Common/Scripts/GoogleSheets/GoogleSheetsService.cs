using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Common.Scripts.GoogleSheets
{
    public class GoogleSheetsService
    {
        public async UniTask<List<T>> LoadSheet<T>(string url) where T : new()
        {
            if (string.IsNullOrEmpty(url)) return new List<T>();
            using var request = UnityWebRequest.Get(url);
            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Google Sheets error: {request.error}");
                return new List<T>();
            }

            try
            {
                return CsvParser.Parse<T>(request.downloadHandler.text);
            }
            catch (Exception e)
            {
                Debug.LogError($"CSV parsing error: {e}");
                return new List<T>();
            }
        }
    }
}