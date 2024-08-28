/*****************************************************************************
// File Name :         Enemy Controller.cs
// Author :            Amber C. Cardamone
// Creation Date :     August 21st, 2024
//
// Brief Description : Controls enemy movement, speed, health, and lets the enemy die.
*****************************************************************************/
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private float _speed;
    [SerializeField] private float _health;
    private bool stunned;
    private bool beginStun;
    private bool killed;
    [SerializeField] private float _stunDuration;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _hurtSound;

    /// <summary>
    /// Finds the game manager in the scene, allowing the Enemy Controller to access it.
    /// </summary>
    void Start()
    {
        // Finds the Game Manager in the scene
        _gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Makes the enemy move, stop moving when hit for a moment, and die.
    /// </summary>
    void Update()
    {
        // Checks if the player is not stunned, making it able to move
        if (stunned == false)
        {
            _animator.SetBool("Moving", true);
            _animator.SetBool("Stunned", false);
            // Moves the enemy along the x axis with variable speed
            _rigidBody.velocity = new Vector2(_speed, 0);
        }
        else if (stunned == true)
        {
            _animator.SetBool("Moving", false);
            _animator.SetBool("Stunned", true);
            // Makes a check so the "Stunned()" coroutine doesn't get spammed
            if (beginStun == true)
            {
                beginStun = false;
                // Starts the coroutine "Stunned()", which makes the enemy stay stunned for a variable duration
                StartCoroutine(Stunned());
            }
            // Stops the enemy movement while stunned
            _rigidBody.velocity = new Vector2(0, 0);
        }
        // Checks if the enemy has no health
        if (_health <= 0)
        {
            killed = true;
            stunned = true;
            _animator.SetBool("Dead", true);
        }
    }

    /// <summary>
    /// Makes the enemy lose health or get destroyed when being hit by a bullet or player.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checks if the enemy collides with a bullet
        if (collision.gameObject.tag == "Bullet")
        {
            AudioSource.PlayClipAtPoint(_hurtSound, transform.position);
            // Makes the enemy lose 1 health
            _health -= 1;
            // Makes the enemy stunned for a short duration
            stunned = true;
            // Makes it so the "Stunned()" Coroutine can be activated
            beginStun = true;
        }
        // Checks if the enemy hits the player or the back wall.
        if (collision.gameObject.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_hurtSound, transform.position);
            _gameManager.PlayerLoseHealth();
            stunned = true;
            // Destroys the enemy
            _animator.SetBool("Dead", true);
        }
    }

    /// <summary>
    /// Makes the enemy have a duration it will be stunned before it is no longer stunned.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Stunned()
    {
        // Makes the coroutine wait for a variable amount of time, extending the stun duration
        yield return new WaitForSeconds(_stunDuration);
        // Makes the enemy no longer stunned, allowing it to move
        stunned = false;
    }

    /// <summary>
    /// Makes the enemy die after the death animation plays, and gives the player points if the player kills the enemy.
    /// </summary>
    private void Dead()
    {
        if (killed == true)
        {
            _gameManager.PlayerAddScore();
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Plays the death sound, accessed through the animator.
    /// </summary>
    private void PlayDeadSound()
    {
        AudioSource.PlayClipAtPoint(_deathSound, transform.position);
    }
}
