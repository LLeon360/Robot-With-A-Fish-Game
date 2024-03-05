using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousetrapScript : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivateTrap() {
        animator.SetBool("Triggered", true);
    }
}
