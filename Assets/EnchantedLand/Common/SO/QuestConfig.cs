using System;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "QuestConfig", menuName = "Configs/Game/SO Quest Config", order = 1)]
    public class QuestConfig : ScriptableObject
    {
        [SerializeField] private  List<QuestList> _questList = null;

        public QuestList GeQuest(int type)
        {
            for (var i = 0; i < _questList.Count; i++)
            {
                if (_questList[i].Id == type)
                {
                    return _questList[i];
                }
            }
            Debug.LogWarning("<color=#FF6600>AvatarConfig: Quest not found!!!</color>");
            return null;
        }
    }
    
    
    [Serializable]
    public class QuestList
    {
        public int Id;
        public Sprite Image;
    }
}