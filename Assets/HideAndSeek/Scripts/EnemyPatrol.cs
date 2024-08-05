using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{    
    private const string PlayerTag = "Player";
    private const int CheckLayer = 9;
    private const int PlayerLayer = 3;
    
    public static event Action CatchPlayerAction;
    
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private Transform _shootPosition;
    [SerializeField] private Observer _observer;
    [SerializeField] private PlayerMovement _player;
    
    private int _curWaypointIndex;
    private bool _stop;

    private void Awake()
    {
        _observer.Init(PlayerTag);
        _stop = false;
    }

    void Start()
    {
        _navMeshAgent.SetDestination (_waypoints[0].position);
    }

    void Update()
    {
        Move();
        CheckPlayer();
    }

    private void Move()
    {
        if (_stop) return;
        if(_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
        {
            _curWaypointIndex = (_curWaypointIndex + 1) % _waypoints.Length;
            _navMeshAgent.SetDestination(_waypoints[_curWaypointIndex].position);
        }
    }

    private void CheckPlayer()
    {
        if (_observer.PlayerInRange)
        {
            Vector3 direction = _player.transform.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;
            
            if (Physics.Raycast(ray, out raycastHit, int.MaxValue, CheckLayer)) 
            {
                if (raycastHit.transform.gameObject.layer == PlayerLayer && !_player.Hidden)
                {
                    _stop = true;
                    _navMeshAgent.ResetPath();
                    transform.LookAt(_player.transform);
                    _animator.SetBool("Shoot",true);
                    CatchPlayerAction?.Invoke();
                }
            }
        }
    }
}
