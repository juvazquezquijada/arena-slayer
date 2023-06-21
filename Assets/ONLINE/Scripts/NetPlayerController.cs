using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetPlayerController : MonoBehaviour
{
   [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
   [SerializeField] GameObject cameraHolder;
   [SerializeField] GameObject Camera;
  
   bool grounded;
   Vector3 smoothMoveVelocity;
   Vector3 moveAmount;
   
   Rigidbody rb;
   PhotonView PV;

   void Awake()
   {
		rb = GetComponent<Rigidbody>();
		PV = GetComponent<PhotonView>();

   }

   void Start()
   {
		{
		if (!PV.IsMine)
		{
       Camera.SetActive(false);
		}
		
	}

   }

   void Update()
   {
		if(!PV.IsMine)
			return;

		Look();
		Move();
		Jump();
   }

   void Look()
	{
		transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

		cameraHolder.transform.localEulerAngles = Vector3.left;
	}

	void Move()
	{
		Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

		moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
	}
	void Jump()
	{
		if(Input.GetKeyDown(KeyCode.Space) && grounded)
		{
			rb.AddForce(transform.up * jumpForce);
		}
	}
	public void SetGroundedState(bool _grounded)
	{
		grounded = _grounded;
	}
	void FixedUpdate()
	{
		if(!PV.IsMine)
			return;

		rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
	}
}
