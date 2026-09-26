using System;
using System.Collections.Generic;
using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "TripConfig", menuName = "Configs/Game/SO Trip Config", order = 1)]
    public class TripConfig : ScriptableObject
    {
        [SerializeField] private  List<TripList> _tripList = null;

        public TripList GeTrip(int type)
        {
            for (var i = 0; i < _tripList.Count; i++)
            {
                if (_tripList[i].Id == type)
                {
                    return _tripList[i];
                }
            }
            Debug.LogWarning("<color=#FF6600>AvatarConfig: Trip not found!!!</color>");
            return null;
        }
    }
    
    
    [Serializable]
    public class TripList
    {
        public int Id;
        public Sprite Image;
    }
}