using System;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "СastleConfig", menuName = "Configs/Game/SO Сastle Config", order = 1)]
    public class СastleConfig : ScriptableObject
    {
        [SerializeField] private  List<СastleList> _castleList = null;

        public СastleList GeСastle(int type)
        {
            for (var i = 0; i < _castleList.Count; i++)
            {
                if (_castleList[i].Id == type)
                {
                    return _castleList[i];
                }
            }
            Debug.LogWarning("<color=#FF6600>MonsterConfig: Сastle not found!!!</color>");
            return null;
        }
    }
    
    
    [Serializable]
    public class СastleList
    {
        public int Id;
        public Sprite Image;
    }
}