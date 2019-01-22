using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    public Rigidbody rigidBody;
    public AudioSource audioSource;

    [SerializeField] float thrusterInfluence;
    [SerializeField] float rotationInfluence;
    [SerializeField] float gravity;
    [SerializeField] int level;

    enum State {Alive, Dying, Transcending};
    State state = State.Alive;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}

    private void FixedUpdate()
    {
        //transform.rotation.x = 0;
        rigidBody.AddForce(Vector3.down * gravity * rigidBody.mass);
        if (state == State.Alive)
        {
            Rotation();
            Thruster();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {

        // If the player is in any other state than alive disallows any other collision
        if (state != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // Do nothing
                break;
            case "Finish":
                if (state == State.Alive) 
                    Invoke("LoadNextScene", 1.5f); // parameterise time
                break;
            default:
                Debug.Log("Donzo");
                audioSource.Stop();
                state = State.Dying;
                Invoke("Dying", 1.5f);
                break;

        }
    }

    private void Dying()
    {

        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        level++;
        SceneManager.LoadScene(level - 1); // todo allow for more than 2 levels
    }

    private void Rotation()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationInfluence * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationInfluence * Time.deltaTime);
        }

        rigidBody.freezeRotation = false; // take automatic control of rotation
    }

    private void Thruster()
    {
        if (Input.GetKey(KeyCode.Space)) // Can thurst while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * thrusterInfluence * Time.deltaTime);
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
   

}
