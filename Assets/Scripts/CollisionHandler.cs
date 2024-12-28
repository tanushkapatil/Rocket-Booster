using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] AudioClip successSFX;
    [SerializeField] ParticleSystem SuccessParticles ;
    [SerializeField] ParticleSystem CrashParticles ;
    AudioSource audioSource;
    bool isControllable = true ;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision other) {
        if(!isControllable) { return; }

        switch(other.gameObject.tag){
            case "Fuel":
                Debug.Log("You picked up fuel");
                break;
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:    
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence() {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(successSFX);
        SuccessParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }
    void StartCrashSequence() {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSFX);
        CrashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void LoadNextLevel() {
        int CurrentScene = SceneManager.GetActiveScene().buildIndex;
        int NextScene = CurrentScene + 1;
        if(NextScene == SceneManager.sceneCountInBuildSettings) {
            NextScene = 0;
        }
        SceneManager.LoadScene(NextScene);
    }

    void ReloadLevel() {
        int CurrentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(CurrentScene);
    }

}
