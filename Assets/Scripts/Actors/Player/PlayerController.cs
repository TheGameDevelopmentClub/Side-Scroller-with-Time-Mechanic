using UnityEngine;

/// <summary>
/// Translates player input into moving the CharacterController
/// </summary>
/// <remarks>Based on the CharacterController2D project by prime31. https://github.com/prime31/CharacterController2D </remarks>
[RequireComponent(typeof(Animator), typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
    public float jumpDelay = 1f;
    public float jumpHoldMax = 2f;
    public int MaxNumberOfJumps = 1;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
    private float _jumpHold;
    private float _jumpTimer;
    private int _numOfJumps;

	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();
	}


	#region Event Listeners

	void OnControllerCollided( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void OnTriggerEnter2D( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
	}


	void OnTriggerExit2D( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;

	    if (_controller.isGrounded)
	    {
	        _numOfJumps = 0;
	        _velocity.y = 0;
	    }
			

		if( Input.GetKey( KeyCode.RightArrow ) )
		{
			normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else if( Input.GetKey( KeyCode.LeftArrow ) )
		{
			normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else
		{
			normalizedHorizontalSpeed = 0;

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Idle" ) );
		}


		// we can only jump whilst grounded
	    if (Input.GetKeyDown(KeyCode.UpArrow))
	    {
	        if (canJump())
	        {
                _numOfJumps++;
                _jumpTimer = 0f;
	            _jumpHold = 1f;
                _velocity.y = Mathf.Sqrt(_jumpHold*jumpHeight * -gravity);
                _animator.Play(Animator.StringToHash("Jump"));
	        }
            else if (_numOfJumps > 0)
            {
                _jumpHold += Time.deltaTime;
                _velocity.y = Mathf.Sqrt(_jumpHold*jumpHeight*-gravity);
            }
	    }
	    else
	    {
	        _jumpTimer += Time.deltaTime;
	    }


		// apply horizontal speed smoothing it
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;

		_controller.move( _velocity * Time.deltaTime );
	}

    private bool canJump()
    {
        if (_controller.isGrounded)
            return true;
        if (_jumpTimer >= jumpDelay && _numOfJumps < MaxNumberOfJumps)
            return true;
        return false;
    }

}
