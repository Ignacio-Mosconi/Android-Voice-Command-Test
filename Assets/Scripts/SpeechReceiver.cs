using UnityEngine;
using UnityEngine.Events;

public class InstructionEvent : UnityEvent<VoiceCommandInstruction> {}
public class SpeechRecogizedEvent : UnityEvent<string> {}

public class SpeechReceiver : MonoBehaviour
{
    [SerializeField] [Tooltip("The English keywords that the receiver will get from the 'Speech Listener'.")]
    string[] englishInstructionKeywords = { "forward", "backwards", "left", "right", "stop" };
    [SerializeField] [Tooltip("The Spanish keywords that the receiver will get from the 'Speech Listener'.")]
    string[] spanishInstructionKeywords = { "adelante", "atrás", "izquierda", "derecha", "alto" };
    
    InstructionEvent onInstructionGiven = new InstructionEvent();
    SpeechRecogizedEvent onSpeechRecognition = new SpeechRecogizedEvent();

    void onActivityResult(string recognizedText)
    {
        VoiceCommandInstruction instruction = VoiceCommandInstruction.None;
        char[] delimiterChars = { '~' };
        string[] words = recognizedText.Split(delimiterChars);

        switch (SpeechListener.Instance.SpeechLanguage)
        {
            case SpeechLanguage.English:
                for (int i = 0; i < englishInstructionKeywords.GetLength(0); i++)
                    if (words[0] == englishInstructionKeywords[i])
                        instruction = (VoiceCommandInstruction)i;
                break;
            case SpeechLanguage.Spanish:
                for (int i = 0; i < spanishInstructionKeywords.GetLength(0); i++)
                    if (words[0] == spanishInstructionKeywords[i])
                        instruction = (VoiceCommandInstruction)i;
                break;
        }

        if (instruction != VoiceCommandInstruction.None)
            onInstructionGiven.Invoke(instruction);

        onSpeechRecognition.Invoke(words[0]);
    }

    public InstructionEvent OnInstructionGiven
    {
        get { return onInstructionGiven; }
    }

    public SpeechRecogizedEvent OnSpeechRecognition
    {
        get { return onSpeechRecognition; }
    }
}