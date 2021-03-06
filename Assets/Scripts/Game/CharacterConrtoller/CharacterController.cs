﻿using UnityEngine;

// Standard character controller used for this game
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class CharacterController : MonoBehaviour
{
    #region Private Variables
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;
    private Vector3 _currentVelocity = Vector3.zero;
    private float _moveFactor = 0.0f;
    public int _jumpCounter = 0;
    private int _groundLayerId = 0;
    #endregion

    #region Public Variables
    public float jumpForce = 10.0f;
    public int jumpAmount = 1;
    public float jumpTriggerWidth = 0.4f;
    public float jumpTriggerHeight = 0.2f;
    public float moveSpeed = 5.0f;
    public float damping = 0.1f;
    public string groundLayerName = "";
    #endregion


    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();

        _groundLayerId = LayerMask.NameToLayer(groundLayerName);
    }


    // Update is called once per frame
    private void Update()
    {
#if DEBUG
        // Draw debug for collider
        DrawDebugBounds(_boxCollider.bounds, Color.blue);
#endif

        // Velocity
        Vector2 targetVelocity = new Vector2(_moveFactor * moveSpeed, _rigidBody.velocity.y);
        _rigidBody.velocity = Vector3.SmoothDamp(_rigidBody.velocity, targetVelocity, ref _currentVelocity, damping);

        // Check ground
        ResolveGrounded();


    }

    private void DrawDebugBounds(in Bounds bounds, Color color)
    {
        Debug.DrawLine(new Vector2(bounds.min.x, bounds.min.y), new Vector2(bounds.max.x, bounds.min.y), color); // down
        Debug.DrawLine(new Vector2(bounds.min.x, bounds.min.y), new Vector2(bounds.min.x, bounds.max.y), color); // left
        Debug.DrawLine(new Vector2(bounds.min.x, bounds.max.y), new Vector2(bounds.max.x, bounds.max.y), color); // up
        Debug.DrawLine(new Vector2(bounds.max.x, bounds.min.y), new Vector2(bounds.max.x, bounds.max.y), color); // right
    }

    private void ResolveGrounded()
    {
        Bounds bounds = new Bounds(
            new Vector3(_boxCollider.transform.position.x, _boxCollider.transform.position.y - _boxCollider.bounds.extents.y),
            new Vector3(jumpTriggerWidth, jumpTriggerHeight)
        );
#if DEBUG
        DrawDebugBounds(in bounds, Color.green);
#endif

 
        if (Physics2D.OverlapBox(
                new Vector2(_boxCollider.transform.position.x, _boxCollider.transform.position.y - _boxCollider.bounds.extents.y),
                bounds.size, 0.0f, 1 << _groundLayerId))
        {
            _jumpCounter = 1;
        }
        
    }

    public void Jump()
    {
        if (_jumpCounter < jumpAmount)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, jumpForce);
            ++_jumpCounter;
        }
       
    
    }

    public void Move(float move)
    {
        _moveFactor = Mathf.Clamp(move, -1.0f, 1.0f);
    }
}
