using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaierControler : MonoBehaviour
{
    private Role role;
    private Animator anim;
    private CharacterController2D cc2d;
    public bool an;
    void Start()
    {
        role = GetComponent<Role>();
        anim = GetComponent<Animator>();
        cc2d = GetComponent<CharacterController2D>();
        anim.SetBool("FallSkill", true);
        an = true;


        var p = transform.position;
        p.y += 10;
        transform.position = p;
    }

    void Update()
    {
        if (transform.position.y > -3.5f) 
        {
            var p = transform.position;
            p.y -= Time.deltaTime * 30;
            transform.position = p;
            anim.SetBool("FallLater", true);
        }
        if (transform.position.y <= -3.5f)
        {
            cc2d._GravityEffect = false;
            anim.SetBool("FallSkill", false);

            if (!anim.GetBool("FallLater"))
            {
                anim.SetBool("Second", true);
            }
        }

    }
}
