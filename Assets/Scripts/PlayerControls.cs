using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("References")]
    public Transform vrPlayer;
    public Transform leftHandTransform;
    public Transform moveDirArrow;

    [Header("Input Actions")]
    public InputActionProperty moveAction;

    [Header("Player Settings")]
    public float moveSpeed = 2.0f;

    private Vector3 moveDir;

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 joystickVector = moveAction.action.ReadValue<Vector2>();
        moveDir = Quaternion.Euler(0, leftHandTransform.eulerAngles.y, 0) * new Vector3(joystickVector.x, 0, joystickVector.y);

        moveDirArrow.localEulerAngles = new(0, leftHandTransform.eulerAngles.y, 0);


    }

    void FixedUpdate()
    {
        vrPlayer.transform.position += moveDir * moveSpeed;
    }
}
