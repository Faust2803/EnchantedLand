using UnityEngine;
using UnityEngine.AI;

namespace Tanks.NetGame.Scripts
{
    public class Navigation : MonoBehaviour
    {
        [SerializeField] private LayerMask _targetLayer;

        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();

            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }

        private void Move()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            if (_agent == null || !_agent.isActiveAndEnabled || !_agent.isOnNavMesh)
            {
                Debug.LogWarning($"Agent не готов. Active: {_agent != null && _agent.isActiveAndEnabled}, " + $"OnNavMesh: {_agent != null && _agent.isOnNavMesh}");
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            // Переводим позицию мыши в мировые координаты
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -Camera.main.transform.position.z));

            worldPosition.z = transform.position.z;

            SetPosition(worldPosition);


            //Debug.Log($"Клик по позиции : {worldPosition}");
        }

        public void SetPosition(Vector3 position)
        {
            if (_agent == null || !_agent.isActiveAndEnabled || !_agent.isOnNavMesh) return;

            position.z = transform.position.z;

            _agent.isStopped = false;
            
            if (!_agent.SetDestination(position)) Debug.LogWarning("Не удалось установить пункт назначения.");
        }

        private void Update()
        {
            Move();
        }
    }
        
        
        
}
    
    
