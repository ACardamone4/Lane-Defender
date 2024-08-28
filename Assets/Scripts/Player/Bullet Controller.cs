/*****************************************************************************
// File Name :         Bullet Controller.cs
// Author :            Amber C. Cardamone
// Creation Date :     August 21st, 2024
//
// Brief Description : Makes bullets play an animation when they hit an enemy, and destroys itself.
*****************************************************************************/
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidBody;

    /// <summary>
    /// Checks if the bullet hits an object, destroying it so it doesn't travel forever.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Wall")
        {
            _animator.SetBool("Explode", true);
            _rigidBody.velocity = new Vector2(0, 0);
        }
    }

    /// <summary>
    /// Destroys the bullet.
    /// </summary>
    /// <returns></returns>
    public void Die()
    {
        Destroy(gameObject);
    }
}
