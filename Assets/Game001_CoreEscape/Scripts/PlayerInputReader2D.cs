using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 将 Input System 的输入转换为玩家意图。
/// 本脚本只读取输入，不负责移动、跳跃规则或修改 Rigidbody2D。
/// </summary>
public sealed class PlayerInputReader2D : MonoBehaviour
{
    [Header("输入配置")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference sprintAction;

    public Vector2 MoveInput { get; private set; }
    public bool SprintHeld { get; private set; }

    private bool jumpRequested;
    private bool isListening;

    private void OnEnable()
    {
        if (moveAction == null || jumpAction == null || sprintAction == null)
        {
            Debug.LogError("PlayerInputReader2D 的 Input Action 尚未绑定完整。", this);
            return;
        }

        moveAction.action.performed += OnMoveChanged;
        moveAction.action.canceled += OnMoveChanged;
        jumpAction.action.performed += OnJumpPerformed;
        sprintAction.action.performed += OnSprintPerformed;
        sprintAction.action.canceled += OnSprintCanceled;

        moveAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();
        isListening = true;
    }

    private void OnDisable()
    {
        if (isListening)
        {
            moveAction.action.performed -= OnMoveChanged;
            moveAction.action.canceled -= OnMoveChanged;
            jumpAction.action.performed -= OnJumpPerformed;
            sprintAction.action.performed -= OnSprintPerformed;
            sprintAction.action.canceled -= OnSprintCanceled;

            moveAction.action.Disable();
            jumpAction.action.Disable();
            sprintAction.action.Disable();
            isListening = false;
        }

        MoveInput = Vector2.zero;
        SprintHeld = false;
        jumpRequested = false;
    }

    private void OnMoveChanged(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        jumpRequested = true;
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        SprintHeld = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        SprintHeld = false;
    }

    public bool ConsumeJumpPressed()
    {
        if (!jumpRequested)
        {
            return false;
        }

        jumpRequested = false;
        return true;
    }
}
