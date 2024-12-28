using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation ;
    [SerializeField] float thrustStrength = 1000f;
    [SerializeField] float rotationStrength = 100f;
    [SerializeField] AudioClip MainEngineSFX;
    [SerializeField] ParticleSystem MainEngineParticles;
    [SerializeField] ParticleSystem RightThrustParticles;
    [SerializeField] ParticleSystem LeftThrustParticles;
    
    Rigidbody rb ;
    AudioSource audioSource ;

    private void Start() {
       rb = GetComponent<Rigidbody>();  
       audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        thrust.Enable();
        rotation.Enable();  
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust() {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StartThrusting() {
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(MainEngineSFX);
        }
        if (!MainEngineParticles.isPlaying)
        {
            MainEngineParticles.Play();
        }
    }

    private void StopThrusting() {
        audioSource.Stop();
        MainEngineParticles.Stop();
    }

    private void ProcessRotation() {
        float rotationinput = rotation.ReadValue<float>() ; 
        if(rotationinput > 0)
        {
            RotateRight();
        }
        else if(rotationinput < 0)
        {
            RotateLeft();
        }
        else
        {
            StopRotating();
        }
    }    

    private void RotateRight() {
        ApplyRotation(-rotationStrength);
        if (!LeftThrustParticles.isPlaying)
        {
            LeftThrustParticles.Play();
            RightThrustParticles.Stop();
        }
    }

    private void RotateLeft() {
        ApplyRotation(rotationStrength);
        if (!RightThrustParticles.isPlaying)
        {
            RightThrustParticles.Play();
            LeftThrustParticles.Stop();
        }
    }

    private void StopRotating() {
        RightThrustParticles.Stop();
        LeftThrustParticles.Stop();
    }    

    private void ApplyRotation(float rotationframe) {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationframe * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }
}
