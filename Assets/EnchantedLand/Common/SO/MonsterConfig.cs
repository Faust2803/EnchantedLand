using System;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "MonsterConfig", menuName = "Configs/Game/SO Monster Config", order = 1)]
    public class MonsterConfig : ScriptableObject
    {
        [SerializeField] private  List<MonsterList> _monsterList = null;

        public MonsterList GeMonster(int type)
        {
            for (var i = 0; i < _monsterList.Count; i++)
            {
                if (_monsterList[i].Id == type)
                {
                    return _monsterList[i];
                }
            }
            Debug.LogWarning("<color=#FF6600>MonsterConfig: Monster not found!!!</color>");
            return null;
        }
    }
    
    
    [Serializable]
    public class MonsterList
    {
        public int Id;
        public Sprite Image;
    }
}