using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
    public float lookSpeed = 3.0f;
	private Vector3 moveDirection = Vector3.zero;
    private Transform Head;
    private CharacterController Controller;

    void Start()
    {
        Head = transform.Find("Head");
    }

	void Update() 
    {
		Controller = GetComponent<CharacterController>();

        var lookX = -Input.GetAxis("Mouse Y");
        var lookY = Input.GetAxis("Mouse X");

        Rotate(lookX, lookY);

		if (Controller.isGrounded) 
        {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
			
		}
		moveDirection.y -= gravity * Time.deltaTime;
		Controller.Move(moveDirection * Time.deltaTime);
	}

    void Rotate(float x, float y)
    {
        x *= lookSpeed;
        y *= lookSpeed;

        transform.Rotate(0, y, 0);
        Head.Rotate(x, 0, 0);
    }
}
