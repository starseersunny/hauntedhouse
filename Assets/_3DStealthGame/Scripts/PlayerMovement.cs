using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    private List<string> m_OwnedKeys = new List<string>();
    Animator m_Animator;
    public InputAction MoveAction;
    public InputAction GhostAction;
    public SkinnedMeshRenderer John;
    public Material JohnPBR;
    public Material JohnToonShaded;
    public Material JohnNothing;
    public Slider GhostSlider;
    public TextMeshProUGUI BarText;
    float GhostDuration;
    float GhostCooldown;

    public bool ghostMode = false;

    public float walkSpeed = 1.0f;
    public float turnSpeed = 20f;

    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody> ();
        MoveAction.Enable();
        m_Animator = GetComponent<Animator> ();
        GhostAction.Enable();
    }

    public void AddKey(string keyName)
    {
        m_OwnedKeys.Add(keyName);
    }

    public bool OwnKey(string keyName)
    {
        return m_OwnedKeys.Contains(keyName);
    }

    void Update()
    {
        // Here, we trigger the ghost mode effect with a spacebar key press. The binding is configured in the inspector.
        // Duration = how long the effect lasts, cooldown = how long the cooldown before the next time you can use it.
        if (GhostAction.WasPressedThisFrame() && GhostCooldown <= 0)
        {
            ghostMode = true;
            GhostDuration = 5;
            GhostCooldown = 15;
        }

        GhostDuration -= Time.deltaTime;
        GhostCooldown -= Time.deltaTime;

        if (GhostDuration < 0)
        {
            ghostMode = false;
        }

        if (ghostMode)
        {
            // Here is when John swaps his materials to enter ghost mode. John swaps to a shader that was ripped from the ghost
            // enemy to make him transparent and ghostly. We additionally define the slider bar here.
            Material[] materials = John.materials;
            materials[0] = JohnNothing;
            materials[1] = JohnNothing;
            John.materials = materials;
            GhostSlider.value = GhostDuration / 5;
            BarText.text = "Ghost Mode Activated";
        }
        else 
        {
            Material[] materials = John.materials;
            materials[0] = JohnPBR;
            materials[1] = JohnToonShaded;
            John.materials = materials;
            GhostSlider.value = 1 - (GhostCooldown / 10);
        // Here is where the slider bar swaps between text for Ghost Mode
            if (GhostCooldown < 0)
            {
                BarText.text = "Press SPACE to activate Ghost Mode";
            }
            else
            {
                BarText.text = "On Cooldown...";
            }
        }

    }

    void FixedUpdate()
    {
        var pos = MoveAction.ReadValue<Vector2>();

        float horizontal = pos.x;
        float vertical = pos.y;

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);

        m_Rigidbody.MoveRotation (m_Rotation);
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * walkSpeed * Time.deltaTime);
    } 
}
