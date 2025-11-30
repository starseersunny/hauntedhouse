using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public UIDocument uiDocument;
    public string KeyName;
    public string KeyName2;

    public AudioSource exitAudio;
    public AudioSource caughtAudio;
    bool m_HasAudioPlayed;

    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer;

    private VisualElement m_EndScreen;
    private VisualElement m_CaughtScreen;

    void Start()
    {
        m_EndScreen = uiDocument.rootVisualElement.Q<VisualElement>("EndScreen");
        m_CaughtScreen = uiDocument.rootVisualElement.Q<VisualElement>("CaughtScreen");
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = false;
        }
    }

    public void CaughtPlayer ()
    {
        m_IsPlayerCaught = true;
    }

    // Here, I updated the update for the ending trigger to check if the player owns the correct keys to leave the door.
    // The ending cutscene only triggers when the door has been approached with the keys.
    void Update()
    {
        if (m_IsPlayerAtExit && player.GetComponent<PlayerMovement>().OwnKey(KeyName) && player.GetComponent<PlayerMovement>().OwnKey(KeyName2))
        {
            EndLevel (m_EndScreen, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel (m_CaughtScreen, true, caughtAudio);
        }
    }

    void EndLevel (VisualElement element, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }
        m_Timer += Time.deltaTime;
        element.style.opacity = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene ("Main");
            }
            else
            {
                Application.Quit ();
                Time.timeScale = 0;
            }
        }
    }
}
