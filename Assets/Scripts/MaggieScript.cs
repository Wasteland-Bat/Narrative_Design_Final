using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaggieScript : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ClosedSmile()
    {
        anim.Play("closedSmile");
    }

    public void OpenSmile()
    {
        anim.Play("openSmile");
    }

    public void Sad()
    {
        anim.Play("sad");
    }

    public void Idle()
    {
        anim.Play("Idle");
    }
}
