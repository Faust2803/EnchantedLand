using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Common.Scripts.GoogleSheets
{
    public class GoogleSheetsService
    {
        private const string TUTORIAL_DOWNLOAD_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=0&single=true&output=csv";
        private const string RACE_DOWNLOAD_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=1215578610&single=true&output=csv";
        private const string ARMOR_DOWNLOAD_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=1575724902&single=true&output=csv";
        private const string WEAPON_DOWNLOAD_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=2002456010&single=true&output=csv";
        private const string DAMAGE_DOWNLOAD_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=869588206&single=true&output=csv";
            

        public async UniTask<List<T>> LoadSheet<T>(GoogleSheetsURL type)
            where T : new()
        {
            var url = type switch
            {
                GoogleSheetsURL.Tutorial => TUTORIAL_DOWNLOAD_URL,
                GoogleSheetsURL.Race => RACE_DOWNLOAD_URL,
                GoogleSheetsURL.Armor => ARMOR_DOWNLOAD_URL,
                GoogleSheetsURL.Weapon => WEAPON_DOWNLOAD_URL,
                GoogleSheetsURL.Damage => DAMAGE_DOWNLOAD_URL,
                
                _ => null
            };

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

    public enum GoogleSheetsURL
    {
        Tutorial,
        Race,
        Armor,
        Weapon,
        Damage
    }
}