using UnityEngine;
using UnityEngine.UI;

public enum SpeechLanguage
{
    English,
    Spanish
}

public class SpeechListener : MonoBehaviour
{
    #region Singleton
    static SpeechListener instance;

    public static SpeechListener Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<SpeechListener>();
            if (!instance)
            {
                GameObject gameObj = new GameObject("Speech Listener");
                instance = gameObj.AddComponent<SpeechListener>();
                instance.hearInstructionButton = GameObject.Find("Hear Instruction Button").GetComponent<Button>();
                instance.lastInstructionText = GameObject.Find("Last Instruction Text").GetComponent<Text>();
                instance.speechReceiver = FindObjectOfType<SpeechReceiver>();

                if (!instance.hearInstructionButton)
                    Debug.LogError("Warning: there is no button to prompt the 'Speech Listener' in the scene.", instance.gameObject);
                if (!instance.lastInstructionText)
                    Debug.LogError("Warning: there is no text to display the speech recognition results in the scene.", instance.gameObject);
                if (!instance.speechReceiver)
                    Debug.LogError("Warning: there is no 'Speech Receiver' to get the info from the 'Speech Listener' in the scene.", 
                                    instance.gameObject);
            }

            return instance;
        }
    }
    #endregion


    [SerializeField] [Tooltip("The language the listener will try to understand.")]
    SpeechLanguage speechLanguage = SpeechLanguage.English;
    [SerializeField] [Tooltip("The prompt text that will appear when a new instruction shall be given.")]
    string speechInstructionPromptText = "Hearing instruction...";
    [SerializeField] [Tooltip("The button that will prompt the speech recognition.")]
    Button hearInstructionButton;
    [SerializeField] [Tooltip("The text that will display the last recognized words.")]
    Text lastInstructionText;
    [SerializeField] [Tooltip("The 'Speech Receiver' that the listener will try to contact after hering instructions.")]
    SpeechReceiver speechReceiver;

    void Awake()
    {
        if (Instance != this)
        {
            Debug.LogError("Warning: more than one 'Speech Listener' in the scene.", gameObject);
            return;
        }
    }

    void Start()
    {
        hearInstructionButton.onClick.AddListener(PromptSpeechRecognition);
        speechReceiver.OnSpeechRecognition.AddListener(ChangeLastInstruction);
    }

    void PromptSpeechRecognition()
    {
        AndroidJavaClass speechEnabler = new AndroidJavaClass("com.plugin.speech.pluginlibrary.TestPlugin");

        speechEnabler.CallStatic("setReturnObject", speechReceiver.gameObject.name);

        switch (speechLanguage)
        {
            case SpeechLanguage.English:
                speechEnabler.CallStatic("setLanguage", "en_US");
                break;
            case SpeechLanguage.Spanish:
                speechEnabler.CallStatic("setLanguage", "es_ES");
                break;
            default:
                speechEnabler.CallStatic("setLanguage", "en_US");
                break;
        }

        speechEnabler.CallStatic("setMaxResults", 3);
        speechEnabler.CallStatic("changeQuestion", speechInstructionPromptText);
        speechEnabler.CallStatic("promptSpeechInput");
    }

    void ChangeLastInstruction(string instructionText)
    {
        lastInstructionText.text = "Last Instruction: " + instructionText;
    }

    public SpeechLanguage SpeechLanguage
    {
        get { return speechLanguage; }
    }
}