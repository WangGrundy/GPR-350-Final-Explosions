using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public PlayerInputactions controls;

    private InputAction move;
    private InputAction fire;
    private InputAction look;
    [SerializeField] private float moveSpeed = 5;

    //camera vars
    private Camera mainCamera;
    public float mouseSensitivity = 0.2f;
    public float cameraVerticalRotation = 0f;
    [SerializeField] private GameObject grenadeDirection;
    private Vector3 playerDirection = Vector3.zero;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float grenadeVelocityMultiplyer;
    private Sphere sphere;

    void Awake()
    {
        controls = new PlayerInputactions();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        move = controls.Player.Move; //player action map inside input actions
        move.Enable();

        look = controls.Player.Look;
        look.Enable();

        fire = controls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        fire.Disable();
    }

    // Update is called once per frame
    void Update()
    {

        //camera
        Vector2 lookInput = look.ReadValue<Vector2>() * mouseSensitivity;

        cameraVerticalRotation -= lookInput.y;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        mainCamera.transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        this.gameObject.transform.Rotate(Vector3.up * lookInput.x);

        //movement
        Vector2 moveInput = move.ReadValue<Vector2>();
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0; // Ignore vertical component for forward movement
        right.y = 0; // Ignore vertical component for right movement

        //vector maths, adding vectors = new destination, multiplying a vector gives you a bigger arrow
        Vector3 desiredMoveDirection = (forward * moveInput.y + right * moveInput.x) * moveSpeed * Time.deltaTime;

        // Apply movement
        transform.position += desiredMoveDirection;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("firing");
        GameObject grenade = Instantiate(grenadePrefab, transform.position + transform.forward * 1, Quaternion.identity);
        Particle3D particle3D = grenade.GetComponent<Particle3D>();
        particle3D.ignoreIntialVelocity = true;
        //Set initial grenade velocity
        particle3D.velocity = (grenadeDirection.transform.position - transform.position).normalized * grenadeVelocityMultiplyer;

        ParticleSpawner spawner = grenade.GetComponent<ParticleSpawner>();
        spawner.StartCoroutine(spawner.delayExplosion());

       
    }
}
