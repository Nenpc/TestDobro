using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputAction MoveAction;
    
    [SerializeField] private float _turnSpeed = 20f;
    [SerializeField] private float _moveSpeed = 1f;

    [SerializeField] private Animator _animatorCharacter;
    [SerializeField] private Animator _animatorBarel;
    [SerializeField] private Rigidbody _rigidbody;

    private bool _hidden;
    private Vector3 _movement;
    private Quaternion _rotation = Quaternion.identity;

    public bool Hidden => _hidden;

    void Start ()
    {
        MoveAction.Enable();
    }

    void FixedUpdate ()
    {
        var pos = MoveAction.ReadValue<Vector2>();
        
        float horizontal = pos.x;
        float vertical = pos.y;
        
        _movement.Set(horizontal, 0f, vertical);
        _movement.Normalize ();
        _movement *= _moveSpeed;

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        _hidden = !isWalking;
        _animatorCharacter.SetBool ("Go", isWalking);
        _animatorBarel.SetBool ("Go", isWalking);
        
        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, _movement, _turnSpeed * Time.deltaTime, 0f);
        _rotation = Quaternion.LookRotation (desiredForward);
        
        _rigidbody.MovePosition (_rigidbody.position + _movement * Time.deltaTime);
        _rigidbody.MoveRotation (_rotation);
    }
}