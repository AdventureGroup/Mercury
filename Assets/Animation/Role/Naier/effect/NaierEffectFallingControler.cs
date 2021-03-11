using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaierEffectFallingControler : MonoBehaviour
{
    public GameObject master;
    CharacterController2D r;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetMaster(GameObject mt)
    {
        master = mt;
        r = master.GetComponent<CharacterController2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (r.IsGrounded)
        {
            anim.SetBool("base", true);
        }
        var k = transform.position;
        k = master.transform.position;
        k.y += 0.5f;
        transform.position = k;
    }
    public void Delete()
    {
        GameManager.Instance.ActiveWorld.Value.DestroyEntity(GetComponent<Entity>());
    }
}
