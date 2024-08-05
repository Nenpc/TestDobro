using UnityEngine;

public class Observer : MonoBehaviour
{
    private string _playerTag = "Player"; 
    private bool _playerInRange;
    public bool PlayerInRange => _playerInRange;

    public void Init(string playerTag)
    {
        _playerTag = playerTag;
    }
    
    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            Debug.Log("Enter trigger");
            _playerInRange = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            Debug.Log("Exit trigger"); 
            _playerInRange = false;
        }
    }
}
