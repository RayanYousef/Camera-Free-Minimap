using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AssetCharacterController
{
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField] Transform cameraParent;

        [SerializeField] float movementSpeed = 10f;
        [SerializeField] float rotationSpeed = 50f;
        [SerializeField] float cameraRotationSpeed = 50f;

        CharacterController _controller;

        Vector3 movementDirection;

        // Start is called before the first frame update
        void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {

            RotateCamera();
            CalculateDirection();
        }

        private void FixedUpdate()
        {
            PlayerMovement();
        }
        void LateUpdate()
        {
            // Camera Position
            cameraParent.position = transform.position + Vector3.up;

        }

        void PlayerMovement()
        {

            // Rotate rotate player towards camera's direction
            if (movementDirection.sqrMagnitude > 0)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementDirection), rotationSpeed / 10 * Time.deltaTime);

            // Character movement
            _controller.Move(movementDirection * movementSpeed * Time.deltaTime);

            // Character rotation
            float rotationDirection = 0;
            if (Input.GetKey(KeyCode.E))
                rotationDirection = 1;
            else
            if (Input.GetKey(KeyCode.Q))
                rotationDirection = -1;
            transform.Rotate(Vector3.up, rotationDirection * Time.deltaTime * rotationSpeed);

        }

        void RotateCamera()
        {
            float mouseX = Input.GetAxis("Mouse X");
            cameraParent.transform.Rotate(Vector3.up * mouseX * cameraRotationSpeed * Time.deltaTime);
        }

        public void CalculateDirection()
        {
            Vector3 TransformedDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) ;

            if (Camera.main != null)
            {
                Vector3 InputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                TransformedDirection = Camera.main.transform.TransformDirection(InputDirection);
            }
            movementDirection = new Vector3(TransformedDirection.x, 0, TransformedDirection.z).normalized;


        }
    }

}
