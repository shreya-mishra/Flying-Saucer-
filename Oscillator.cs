using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] float period = 2f;
    [SerializeField] Vector3 movementVector= new Vector3(10f, 10f , 10f);
    [Range(0,1)][SerializeField] float movementFactor;
    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time / period;
        const float tau= Mathf.PI * 2;
        float rawSineWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSineWave/2f+0.5f;
        Vector3 offset =movementFactor * movementVector;
        transform.position=startingPos + offset;
        
    }
}
