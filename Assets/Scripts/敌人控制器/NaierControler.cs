using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaierControler : MonoBehaviour
{
    private Role role;
    private Animator anim;
    private CharacterController2D cc2d;
    public bool an;

    private GameObject PreFalling;
    private GameObject PreBoom;


    private Entity Falling;
    void Start()
    {
        PreFalling = GameManager.Instance.Db.Effects[4].gameObject;
        PreBoom = GameManager.Instance.Db.Effects[5].gameObject;
        role = GetComponent<Role>();
        anim = GetComponent<Animator>();
        cc2d = GetComponent<CharacterController2D>();
        anim.SetBool("FallSkill", true);
        an = true;


        var p = transform.position;
        p.y += 10;
        transform.position = p;


        Falling = GameManager.Instance.ActiveWorld.Value.CreateEntity(PreFalling);
        Falling.transform.position = role.transform.position;
        Falling.Camp = role.Camp;
        Falling.GetComponent<NaierEffectFallingControler>().SetMaster(this.gameObject);


        var _dam = new DamageClass();
        _dam.SetMag();
        _dam.Damage = 100;
        _dam.Element.Dark = true;

        //Falling.GetComponent<Projectile>().SetManyTimes();
        Falling.GetComponent<Projectile>().damage = _dam;
    }
    private bool trigger1 = false;
    private bool trigger2 = false;
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
            if (!trigger2)
            {
                Falling.GetComponent<Animator>().SetBool("base", true);
                trigger2 = true;
            }
            if (!anim.GetBool("FallLater"))
            {
                if (!trigger1)
                {
                    Falling.GetComponent<NaierEffectFallingControler>().Delete();
                    trigger1 = true;
                    /*
                    var k = GameManager.Instance.ActiveWorld.Value.CreateEntity(PreBoom);
                    k.transform.position = role.transform.position;
                    k.Camp = role.Camp;
                    k.GetComponent<NaierEffectFallingControler>().SetMaster(this.gameObject); 
                    var _dam = new DamageClass();
                    _dam.SetMag();
                    _dam.Damage = 100;
                    _dam.Element.Dark = true;
                    k.GetComponent<Projectile>().damage = _dam;
                    */
                }
                anim.SetBool("Second", true);
            }
        }

    }
}
