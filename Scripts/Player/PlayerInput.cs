using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent (typeof (Player))]
public class PlayerInput : MonoBehaviour
{

	Player player;
	Animator anim;
	Vector2 directionalInput;
	public Joystick joystick;
	public LayerMask enemyLayer;
	float joystickTolerance = .3f;
	public InGameController inGameController;

	[SerializeField] private Button goLeft;
	[SerializeField] private Button goRight;
    void Start()
    {
        player = GetComponent<Player>();
		anim = GetComponent<Animator>();

		if (!GameController.isControllerTypeJoystick)
		{
			joystick.gameObject.SetActive(false);
			goLeft.gameObject.SetActive(true);
			goRight.gameObject.SetActive(true);
		}
    }

    void Update()
    {
		Collider2D hit = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y) + GetComponent<BoxCollider2D>().offset, GetComponent<BoxCollider2D>().size + new Vector2(.1f,.1f), 0f, enemyLayer);
		if (hit)
		{
			Die();
		}
		/*
		if(Application.platform == RuntimePlatform.WindowsEditor)
		{
			directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		}
		else if(Application.platform == RuntimePlatform.Android)
		{
			directionalInput = new Vector2(Mathf.Abs(joystick.Horizontal) >= joystickTolerance ? joystick.Horizontal : 0, joystick.Vertical);
		}
		*/
		directionalInput = new Vector2(Input.GetAxisRaw("Horizontal") + (CrossPlatformInputManager.GetButton("GoLeft") ? -1 : (CrossPlatformInputManager.GetButton("GoRight") ? 1 : 0)) + (Mathf.Abs(joystick.Horizontal) >= joystickTolerance ? joystick.Horizontal : 0), joystick.Vertical);

		directionalInput.x = directionalInput.x > 0 ? 1 : (directionalInput.x < 0 ? -1 : 0);
		player.SetDirectionalInput(directionalInput);

		if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Jump"))
		{
			player.OnJumpInputDown();
			anim.SetBool("animJump", true);
		}

		if (Input.GetKeyUp(KeyCode.Space)  || CrossPlatformInputManager.GetButtonUp("Jump"))
		{
			player.OnJumpInputUp();
		}

		ControlAnimations();
    }

	void ControlAnimations()
	{
		if (CrossPlatformInputManager.GetButtonDown("Crouch"))
		{
			if(player.controller.hit)
			{
				if(player.controller.hit.collider.tag != "PlatformMoveThrough")
				{
					anim.SetBool("animCrouch", true);
				}
			}
		}
		if (CrossPlatformInputManager.GetButtonUp("Crouch"))
		{
			anim.SetBool("animCrouch", false);
		}

		if (anim.GetBool("animJump"))
		{
			Invoke("CheckGrounded", .05f);
		}

		anim.SetFloat("animMoveSpeed", Mathf.Abs(directionalInput.x));
		anim.SetFloat("animVerticalSpeed", player.velocity.y);
	}

	void CheckGrounded()
	{
		if (player.controller.collisions.below)
		{
			anim.SetBool("animJump", false);
		}
	}
	private int deathCount = 0;
    IEnumerator sceneReloader()
    {
        yield return new WaitForSeconds(.25f);

		deathCount = PlayerPrefs.GetInt("deathCountTemp" +  (SceneManager.GetActiveScene().buildIndex - 1), 0);
		PlayerPrefs.SetInt("deathCountTemp" +  (SceneManager.GetActiveScene().buildIndex - 1), deathCount + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		
    }

	public void Die()
	{
		CrossPlatformInputManager.SetButtonUp("GoLeft");
		CrossPlatformInputManager.SetButtonUp("GoRight");
		anim.SetBool("animDead", true);
		StartCoroutine(sceneReloader());
	}
}
