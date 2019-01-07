using UnityEngine;

public enum VoiceCommandInstruction
{
    Forward,
    Backwards,
    Left,
    Right,
    Stop,
    None
}

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(SpeechReceiver))]
public class MoveOnVoiceCommand : MonoBehaviour
{
    [SerializeField] [Tooltip("The character's movement speed in meters per second.")]
    float movementSpeed = 70f;
    [SerializeField] [Tooltip("The character's rotation speed in degrees per second.")]
    float rotationSpeed = 15f;

    CharacterController characterController;
    SpeechReceiver speechReceiver;
    VoiceCommandInstruction currentInstruction = VoiceCommandInstruction.Stop;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        speechReceiver = GetComponent<SpeechReceiver>();
    }

    void Start()
    {
        speechReceiver.OnInstructionGiven.AddListener(ChangeCurrentInstruction);
    }

    void Update()
    {
        FollowCurrentInstruction();
    }

    void FollowCurrentInstruction()
    {
        Vector3 movement = Vector3.zero;
        Quaternion rotation = Quaternion.AngleAxis(0f, transform.up);

        switch (currentInstruction)
        {
            case VoiceCommandInstruction.Forward:
                movement = transform.forward;
                break;
            case VoiceCommandInstruction.Backwards:
                movement = -transform.forward;
                break;
            case VoiceCommandInstruction.Left:
                rotation = Quaternion.AngleAxis(-rotationSpeed * Time.deltaTime, transform.up);
                break;
            case VoiceCommandInstruction.Right:
                rotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, transform.up);
                break;
            case VoiceCommandInstruction.Stop:
            default:
                break;
        }

        characterController.SimpleMove(movement * movementSpeed * Time.deltaTime);
        transform.rotation *= rotation;
    }

    void ChangeCurrentInstruction(VoiceCommandInstruction newInstruction)
    {
        currentInstruction = newInstruction;
    }
}