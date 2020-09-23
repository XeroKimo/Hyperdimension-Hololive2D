using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//V0.1

public class CollisionCallbacks2D : MonoBehaviour
{
    public delegate void CollisionCallback2D(Collision2D collision);
    public delegate void TriggerCallback2D(Collider2D collision);

    public event CollisionCallback2D CollisionEnter2D;
    public event CollisionCallback2D CollisionStay2D;
    public event CollisionCallback2D CollisionExit2D;

    public event TriggerCallback2D TriggerEnter2D;
    public event TriggerCallback2D TriggerStay2D;
    public event TriggerCallback2D TriggerExit2D;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionEnter2D?.Invoke(collision);
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        CollisionStay2D?.Invoke(collision);
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        CollisionExit2D?.Invoke(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerEnter2D?.Invoke(collision);   
    }    
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        TriggerStay2D?.Invoke(collision);
    }    
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        TriggerExit2D?.Invoke(collision);
    }
}
