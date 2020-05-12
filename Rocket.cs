using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    
    Rigidbody rigidBody;
    [SerializeField] float rcsThrust = 50f;  //constatnt
    [SerializeField] float mainThrust = 100f;  //constatnt
    [SerializeField] float levelLoadDelay = 2f;  //constatnt

   
    [SerializeField] AudioClip mainEngine;    //multiple sounds
    [SerializeField] AudioClip Success;    //multiple sounds
    [SerializeField] AudioClip Death;    //multiple sounds

    [SerializeField] ParticleSystem mainEngineParticles;    //multiple sounds
    [SerializeField] ParticleSystem SuccessParticles;    //multiple sounds
    [SerializeField] ParticleSystem DeathParticles;    //multiple sounds






    
    AudioSource audioSource;
    void Start()
    {
        rigidBody=GetComponent<Rigidbody>(); //Get access to rigidbody
        audioSource=GetComponent<AudioSource>();
        
    }

    enum State{Alive, Dying, Transcending};

    State state = State.Alive;
    bool collisionDisabled=false;


    // Update is called once per frame
    void Update()
    {
        if(state==State.Alive)
        
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if(Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
        
    }

    private void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;//toggle collision
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        
        if(state !=State.Alive || collisionDisabled)
        {
            return;
        }
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                    //do nothing
                    break;
            case "Finish":
                    //print("finish");
                    
                    StartSuccessSequence();
                    //SceneManager.LoadScene(1);
                    break;
            
            default :
                    StartDeathSequence();
                    //do nothing
                     break;   
        }
    }
    private void StartSuccessSequence()
    {

                   state=State.Transcending;
                    audioSource.Stop();
                    audioSource.PlayOneShot(Success);
                    SuccessParticles.Play();
                    Invoke("LoadNextLevel",levelLoadDelay);
    }
    
    private void StartDeathSequence()
    {
        state=State.Dying;
        audioSource.Stop();
        DeathParticles.Play();
        audioSource.PlayOneShot(Death);
        
        Invoke("LoadFirstLevel",levelLoadDelay);
       


    }

    
   // private int nextSceneToLoad = SceneManager.GetActiveScene().buildIndex+1;
   int i=0;
    private  void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
       // print(currentSceneIndex);
       int nextSceneIndex = currentSceneIndex+1;
       if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
       {
           nextSceneIndex=0;
       }
       SceneManager.LoadScene(nextSceneIndex);  //to do allow for more than 1 level
    }

     private  void LoadFirstLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);  //to do allow for more than 1 level
    }



    private void RespondToThrustInput()
    {

        if(CrossPlatformInputManager.GetButton("Jump")) // can thrust while rotating
        {
            
            ApplyThrust();
        }
            else
            {
                    audioSource.Stop();
                    mainEngineParticles.Stop();

            }

        
    }
    private void ApplyThrust()
    {
      rigidBody.AddRelativeForce(Vector3.up*mainThrust );
      if(audioSource.isPlaying==false)
      {
          audioSource.PlayOneShot(mainEngine);
      }
      mainEngineParticles.Play();
    }
    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true;
       
       float rotationThisFrame=rcsThrust * Time.deltaTime;
       

       if(CrossPlatformInputManager.GetButton("A"))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if(CrossPlatformInputManager.GetButton("D"))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
         rigidBody.freezeRotation = false;
    }
    
}
