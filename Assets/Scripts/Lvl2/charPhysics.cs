using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charPhysics : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    public charPhysics(string name, Rigidbody2D rb, Animator anim, Collider2D coll)
    {
        this.name = name;
        this.rb = rb;
        this.anim = anim;
        this.coll = coll;
    }

    public Rigidbody2D getRb()
    {
        return rb;
    }

    public Animator getAnim()
    {
        return anim;
    }

    public Collider2D getColl()
    {
        return coll;
    }

    public void setRb(Rigidbody2D rb)
    {
        this.rb = rb;
    }

    public void setAnim(Animator anim)
    {
        this.anim = anim;
    }

    public void setColl(Collider2D coll)
    {
        this.coll = coll;
    }
}
