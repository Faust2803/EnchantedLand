using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class LevelMap : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spawnPoints;
        public List<Transform> SpawnPoints => _spawnPoints;
    }
}