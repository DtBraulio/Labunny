using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

    public float rotationX;
    public float rotationY;
    public float sensitibity;
    private Transform cameraTransform;
    public Transform playerTransform;
    public Quaternion cameraRotation;
    public Quaternion playerRotation;

    public float smooth;

    // Use this for initialization
    void Start ()
    {

        cameraTransform = transform;
        cameraRotation = cameraTransform.rotation;

        playerRotation = playerTransform.rotation;
	}
	
	// Update is called once per frame
	void Update ()
    {
        ReadInput();

        //camera rotation X
        cameraRotation *= Quaternion.Euler (-rotationY, 0, 0);
        playerRotation *= Quaternion.Euler (0, rotationX, 0);

        cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, cameraRotation, Time.deltaTime * smooth);
        playerTransform.localRotation = Quaternion.Slerp(playerTransform.localRotation, playerRotation, Time.deltaTime * smooth);
    }

    void ReadInput()
    {
        rotationX = Input.GetAxis("Mouse X") * sensitibity;
        rotationY = Input.GetAxis("Mouse Y") * sensitibity;

    }
}
