using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRocket : MonoBehaviour {

    [SerializeField] Rocket rocket;

    void Start()
    {
        transform.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update () {
        transform.position = new Vector3(rocket.transform.position.x, transform.position.y, transform.position.z);
    }
}
