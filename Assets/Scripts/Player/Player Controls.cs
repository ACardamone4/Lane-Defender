/*****************************************************************************
// File Name :         Player Controls.cs
// Author :            Amber C. Cardamone
// Creation Date :     August 21st, 2024
//
// Brief Description : Makes the player able to move, shoot, restart, and die.
*****************************************************************************/
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    private InputAction move;
    [SerializeField] private bool _moving;
    [SerializeField] private float _moveSpeed;
    private float moveDirection;
    private InputAction shoot;
    private bool shooting;
    private bool onCooldown;
    private bool dead;
    [SerializeField] private bool _canShoot;
    [SerializeField] private float _maxCooldown;
    [SerializeField] private float _currentCooldown;
    private InputAction restart;
    private InputAction quit;
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Animator _bulletSpawnPointAnimator;
    [SerializeField] private AudioClip _bulletShootSound;
    //[SerializeField] private SaveLoadJSON _saveLoadJSON;
    [SerializeField] private DataPersistenceManager _dataPersistenceManager;

    /// <summary>
    /// Sets up connection with the new input system.
    /// </summary>
    void Start()
    {
        // Sets up the new input system and connects it to the Player Controls.
        _playerInput = GetComponent<PlayerInput>();
        move = _playerInput.currentActionMap.FindAction("Move");
        restart = _playerInput.currentActionMap.FindAction("Restart");
        quit = _playerInput.currentActionMap.FindAction("Quit");
        shoot = _playerInput.currentActionMap.FindAction("Shoot");
        _playerInput.currentActionMap.Enable();
        move.started += MoveStarted;
        move.canceled += MoveCanceled;
        restart.performed += RestartPerformed;
        quit.performed += QuitPerformed;
        shoot.started += ShootStarted;
        shoot.canceled += ShootCanceled;
    }

    /// <summary>
    /// Disables all actions on restart, removing errors on quit.
    /// </summary>
    public void OnDestroy()
    {
        move.started -= MoveStarted;
        move.canceled -= MoveCanceled;
        restart.performed -= RestartPerformed;
        quit.performed -= QuitPerformed;
        shoot.started -= ShootStarted;
        shoot.canceled -= ShootCanceled;
    }
    #region Actions
    #region Movement
    /// <summary>
    /// Makes the player start moving once the player presses a movement button.
    /// </summary>
    /// <param name="obj"></param>
    private void MoveStarted(InputAction.CallbackContext obj)
    {
        _moving = true;
    }
    /// <summary>
    /// Makes the player stop moving once the player releases a movement button.
    /// </summary>
    /// <param name="obj"></param>
    private void MoveCanceled(InputAction.CallbackContext obj)
    {
        _moving = false;
    }
    #endregion
    #region Shooting
    /// <summary>
    /// Makes the player start shooting once the player presses the spacebar.
    /// </summary>
    /// <param name="obj"></param>
    private void ShootStarted(InputAction.CallbackContext obj)
    {
        shooting = true;
    }
    /// <summary>
    /// Makes the player stop shooting once the player releases the spacebar.
    /// </summary>
    /// <param name="obj"></param>
    private void ShootCanceled(InputAction.CallbackContext obj)
    {
        shooting = false;
    }
    #endregion
    #region Restart
    /// <summary>
    /// Restarts the game.
    /// </summary>
    /// <param name="obj"></param>
    private void RestartPerformed(InputAction.CallbackContext obj)
    {
        _dataPersistenceManager.Save();
        //_saveLoadJSON.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
    #region Quit
    /// <summary>
    /// Quits the game.
    /// </summary>
    /// <param name="obj"></param>
    private void QuitPerformed(InputAction.CallbackContext obj)
    {
        _dataPersistenceManager.Save();
        //_saveLoadJSON.SaveGame();
        Application.Quit();
    }
    #endregion
    #endregion
    /// <summary>
    /// Makes the player unable to die and triggers the death event
    /// </summary>
    public void Die()
    {
        _canShoot = false;
        dead = true;
        _rigidBody.velocity = new Vector2(0, 0);
    }

    /// <summary>
    /// Allows the player to move and shoot.
    /// </summary>
    void FixedUpdate()
    {
        #region Movement
        // Checks if the player is moving
        if (_moving == true && dead == false)
        {
            // Makes the game check what button the player is pressing, and takes that value and puts it into code
            moveDirection = move.ReadValue<float>();
            // Makes the player able to move up and down  
            _rigidBody.velocity = new Vector2(0, _moveSpeed * moveDirection);
        }
        // Checks if the player is not pressing a movement input
        else if (_moving == false || dead == true)
        {
            // Stops the player in its track
            _rigidBody.velocity = new Vector2(0, 0);
        }
        #endregion
        #region Shoot
        // Makes the game check if the player is pressing a shooting button and the shoot is not on cooldown
        if (shooting == true && onCooldown == false && _canShoot == true)
        {
            // Plays a shooting sound
            AudioSource.PlayClipAtPoint(_bulletShootSound, transform.position);
            // Makes an explosion effect to hide the bullet spawn
            _bulletSpawnPointAnimator.Play("Spawnpoint_Shoot");
            // Makes the shooting go on cooldown, so it can't be spammed
            onCooldown = true;
            // Spawns a bullet object
            GameObject BulletInstance = Instantiate(_bullet, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
            // Makes the bullet move right with a velocity chosen in the inspector
            BulletInstance.GetComponent<Rigidbody2D>().AddForce(BulletInstance.transform.right * _bulletSpeed);
        }
        if (onCooldown == true)
        {
            // Starts a clock that counts down to 0, allowing the player to shoot again
            _currentCooldown -= Time.deltaTime;
            if (_currentCooldown <= 0)
            {
                // Resets the timer
                _currentCooldown = _maxCooldown;
                // Allows the player to shoot again
                onCooldown = false;
            }
        }
        #endregion
    }
}
