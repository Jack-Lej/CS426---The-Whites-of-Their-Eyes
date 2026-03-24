using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WasdMovement : MonoBehaviour
{
	#region Data
	public float moveSpeed = 6;
	public float rotSpeed = 30;

	public float headOffset = 1;
	public float cameraOffset;
	#endregion

	#region Properties
	public Rigidbody PlayerRigid { get; private set; }
	#endregion

	#region Unity Functions
	private void Awake()
	{
		PlayerRigid = GetComponent<Rigidbody>();
	}
	private void FixedUpdate()
	{
		// Grabbing User Input
		Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		float rotInput = Input.GetAxis("Mouse X") * Time.fixedDeltaTime * rotSpeed;
		cameraOffset += Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * rotSpeed;

		// Rotating a Moving Player
		PlayerRigid.rotation *= Quaternion.Euler(0, rotInput, 0);
		PlayerRigid.position += PlayerRigid.rotation * moveInput.normalized * moveSpeed * Time.fixedDeltaTime;

		// Snapping Camera to Head
		Camera.main.transform.rotation = PlayerRigid.rotation * Quaternion.Euler(cameraOffset, 0, 0);
		Camera.main.transform.position = transform.position + new Vector3(0, headOffset, 0);
	}
	#endregion
}