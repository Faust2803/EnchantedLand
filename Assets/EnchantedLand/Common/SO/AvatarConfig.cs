using System;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "AvatarConfig", menuName = "Configs/Game/SO Avatar Config", order = 1)]
    public class AvatarConfig : ScriptableObject
    {
        [SerializeField] private  List<AvatarList> _avatarList = null;

        public AvatarList GeAvatar(int type)
        {
            for (var i = 0; i < _avatarList.Count; i++)
            {
                if (_avatarList[i].Id == type)
                {
                    return _avatarList[i];
                }
            }
            Debug.LogWarning("<color=#FF6600>AvatarConfig: Avatar not found!!!</color>");
            return null;
        }
    }
    
    
    [Serializable]
    public class AvatarList
    {
        public int Id;
        public Sprite Image;
        public string Title;
        public string Description;
        public Characteristics Characteristics;
    }
}