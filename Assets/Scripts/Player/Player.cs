using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player> {

    [Header("Swipe Settings")]
    [SerializeField] private float minimumDistance = .2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = .9f;

    private Vector2 startPosition;
    private float startTime;

    private Vector2 endPosition;
    private float endTime;

    private Vector2 direction2D;

    [Header("Player Settings")]
    [SerializeField] private float moveSpeed;
    private Vector2 moveDirection ;
    private Vector2 newMoveDirection; // As player changes direction after coming out of teleport point this is the new direction on teleport
    [SerializeField] private List<GameObject> playerMeshes = new List<GameObject>();

    [Header("References")]
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private Colour currentColour;

    private TrailRenderer[] trailRenderers;

    public event EventHandler OnWinPointReached;
    public event EventHandler<OnPaintChangedEventArgs> OnPaintChanged;
    public class OnPaintChangedEventArgs : EventArgs{
        public Paint paint;
    };

    private bool generated = false; 
    private bool canMove;

    private void Start() {
        currentColour = GetComponentInChildren<PlayerColour>().GetCurrentPlayerMeshColour();
        trailRenderers = GetComponentsInChildren<TrailRenderer>(true);
    }
    private void Update() {
        // Debug.Log(moveDirection);
        canMove = CanMove();

        if (canMove) {
            // Debug.Log("CanMove");
            MovePlayer();
            RotateInMoveDirection(); // Rotate object in move direction
        }
        else {
            // Debug.Log("Cant Move");
            StopPlayer();
        }

    }


    private void OnEnable() {
        InputManager.Instance.OnStartTouch += SwipeStart;
        InputManager.Instance.OnEndTouch += SwipeEnd;
    }

    private void OnDisable() {
        InputManager.Instance.OnStartTouch -= SwipeStart;
        InputManager.Instance.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time) {
        startPosition = position;
        startTime = time;
    }

    private void SwipeEnd(Vector2 position, float time) {
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe() {
        if (Vector3.Distance(startPosition, endPosition) >= minimumDistance && (endTime - startTime) <= maximumTime) {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            direction2D = (Vector2)direction.normalized;
            // direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction) {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold) {
            moveDirection = Vector2.up;
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold) {
            moveDirection = Vector2.down;
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold) {
            moveDirection = Vector2.left;
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold) {
            moveDirection = Vector2.right;
        }
    }
    private bool CanMove() {
        return CheckForCollidersInPath() && moveDirection != Vector2.zero;
    }
    private void MovePlayer() {
        // Move the player
        moveSpeed = 8f;
        transform.position += (Vector3)moveDirection * Time.deltaTime * moveSpeed;
    }

    private void StopPlayer() {
        moveSpeed = 0;
        moveDirection = Vector3.zero;
    }
    
    private bool CheckForCollidersInPath() {

        // Use squarecast or something
        Vector2 size = new Vector2(0.4f, 0.4f);
        float length = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, size, 0, moveDirection, length, collisionLayer);
        //RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, moveDirection, raycastLength, collisionLayer);

        if (raycastHit) {
            Debug.Log(raycastHit.collider.name);
            if (CheckWallCollision(raycastHit)) {
                return false;
            }

            if (CheckPaintCollision(raycastHit)) {
                return true;
            }

            if (CheckTeleportPointCollision(raycastHit)) {
                return true;
            }


            return false;
        }

        return CheckWinPointProximity(); 
    }
    private bool CheckWallCollision(RaycastHit2D raycastHit) {
        if (raycastHit.collider.TryGetComponent<Wall>(out Wall collidedWall)) {
            RotateAwayFromCollision(raycastHit.point);
            StopPlayer();

            bool isColourDifferent = WallColourController.Instance.IsColourDifferent(collidedWall);

            if (!isColourDifferent) {
                // The player's color matches the wall's color; play particle effect

                if (moveDirection == Vector2.zero) {
                    PlayWallCollisionParticles();
                }
            }
            else {
                DestroySelf(); // Player loses when colliding with a different color wall
                GameManager.Instance.State = GameStates.Lose;
            }

            return true; // A valid collision with a wall occurred
        }

        return false; // No wall collision detected
    }

    private void PlayWallCollisionParticles() {
        // Ensure particles are played only during actual collision
        ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
        particles.Play();
    }

    private bool CheckPaintCollision(RaycastHit2D raycastHit) {
        if (raycastHit.collider.TryGetComponent<Paint>(out Paint collidedPaint)) {
            bool switchColour = ColourSwitcher.Instance.IsColourDifferent(collidedPaint);

            if (switchColour) {
                generated = false;
            }

            if (!generated) {
                ColourSwitcher.Instance.SwitchColour(collidedPaint);

                OnPaintChanged?.Invoke(this, new OnPaintChangedEventArgs {
                    paint = collidedPaint
                });

                generated = true;
            }

            return true; // Return true as player can move and will not stop at paint collisions
        }

        return false;
    }
    private bool CheckTeleportPointCollision(RaycastHit2D raycastHit) {
        if (raycastHit.collider.TryGetComponent<TeleportPoint>(out TeleportPoint teleportPoint)) {
            if (!teleportPoint.Teleported) {
                // Teleport cooldown logic is implemented in TeleportPoint script
                StopPlayer();
                // Started teleport so set trail to false
                PauseTrail();

                teleportPoint.TeleportPlayer(this.transform, out newMoveDirection);
                // Move player towards modifiedMoveDirection from TeleportPoint
                // Player teleported so set trail to true
                moveDirection = newMoveDirection;
                MovePlayer();
            }


            return true; // Return true as player can move and will not stop at teleport point
        }

        return false;
    }

    public void PauseTrail() {
        // Disable all trails in children
        foreach (TrailRenderer trail in trailRenderers) {
            trail.enabled = false;
            trail.Clear();
        }

        float resetDelay = 0.2f;
        StartCoroutine(TrailResetDelay(resetDelay));
    }

    private IEnumerator TrailResetDelay(float delay) {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Enable all trails after the delay
        foreach (TrailRenderer trail in trailRenderers) {
            trail.enabled = true;
        }
    }


    private bool CheckWinPointProximity() {
        // Winpoint detection is not implemented by raycast but by distance
        // Singleton reference is taken as there will be only one winpoint per scene
        float distanceToWinPoint = Vector2.Distance(transform.position, WinPoint.Instance.transform.position);

        if (distanceToWinPoint <= 0.5f) {
            GameManager.Instance.State = GameStates.Win;
            OnWinPointReached?.Invoke(this, EventArgs.Empty);
            HideSelf();
            return false; // Return false as the player should stop when it reaches win point   
        }

        return true;
    }


    public void DestroySelf() {
        Destroy(gameObject);
    }
    public void HideMeshWithColour(Colour hideColour) {
        // All the required meshes are a child of the player
        // Cycles through the children and hides all children
        foreach (GameObject playerMesh in playerMeshes) {
            if (playerMesh.gameObject.GetComponent<PlayerColour>().GetCurrentPlayerMeshColour() ==  hideColour) {
                playerMesh.gameObject.SetActive(false);
            }
        }
    }
    public void ShowMeshWithColour(Colour showColour) {
        // All the required meshes are a child of the player
        // Cycles through the children and shows all children

        foreach (GameObject playerMesh in playerMeshes) {
            if (playerMesh.gameObject.GetComponent<PlayerColour>().GetCurrentPlayerMeshColour() == showColour) {
                playerMesh.gameObject.SetActive(true);
            }
        }
    }
    public void HideSelf() {
        this.gameObject.SetActive(false);
    }
    private void RotateInMoveDirection() {
        float _angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -_angle));
    }
    public void RotateAwayFromCollision(Vector2 collisionPoint) {
        Vector2 directionAwayFromCollision = (Vector2)transform.position - collisionPoint;
        float angle = Mathf.Atan2(directionAwayFromCollision.x, directionAwayFromCollision.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
    }

    public Colour GetPlayerColour() {
        return currentColour;
    }

    public bool CanPlayerMove() {
        return canMove;
    }
    public void SetCurrentPlayerColour(Colour setColour) {
        currentColour = setColour;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, new Vector2(0.5f, 0.5f));
        Vector3 endPoint =  (Vector2)transform.position+ moveDirection * 0.5f;
        Gizmos.DrawWireCube(endPoint, new Vector2(0.5f, 0.5f));
    }
}
