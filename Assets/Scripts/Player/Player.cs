using System;
using System.Collections;
using System.Collections.Generic;
using TechJuego.InputControl;
using UnityEngine;
using UnityEngine.XR;

public class Player : Singleton<Player> {

    [Header("Player Settings")]
    [SerializeField] private float speed = 5f;
    private Vector2 moveDirection;
    private Vector2 newMoveDirection;
    [SerializeField] private List<GameObject> playerMeshes = new List<GameObject>();

    [Header("References")]
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private LayerMask collectableLayer;
    [SerializeField] private Colour currentColour;
    
    private TrailRenderer[] trailRenderers;

    public event EventHandler<OnWinPointReachedEventArgs> OnWinPointReached;
    public class OnWinPointReachedEventArgs : EventArgs {
        public Vector3 position;
    }
    public event EventHandler<OnPlayerLoseEventArgs> OnPlayerLose;
    public class OnPlayerLoseEventArgs : EventArgs {
        public Vector3 position;
    }
    public event EventHandler<OnPlayerTeleportEventArgs> OnPlayerTeleport;
    public class OnPlayerTeleportEventArgs : EventArgs {
        public Vector3 position;
    }

    private bool playerMoving = false;
    private void Start() {
        currentColour = GetComponentInChildren<PlayerColour>().GetCurrentPlayerMeshColour();
        trailRenderers = GetComponentsInChildren<TrailRenderer>(true);
    }
    private void Update() {
        bool isObjectInPath = CheckForCollidersInPath();
        if (isObjectInPath && moveDirection != Vector2.zero) {
            MovePlayer();
            RotateInMoveDirection();
        } else {
            StopPlayer();
        }
    }
    private void OnEnable() {
        SwipeController.Instance.OnSwipeUp += SwipeController_OnSwipeUp;
        SwipeController.Instance.OnSwipeDown += SwipeController_OnSwipeDown;
        SwipeController.Instance.OnSwipeLeft += SwipeController_OnSwipeLeft;
        SwipeController.Instance.OnSwipeRight += SwipeController_OnSwipeRight;

        EventController.OnTeleport += EventController_OnTeleport;
    }

    private void EventController_OnTeleport(Vector2 vector) {
        PauseTrail();
        StopPlayer();
        this.moveDirection = vector;
        MovePlayer();
    }

    private void OnDisable() {
        SwipeController.Instance.OnSwipeUp -= SwipeController_OnSwipeUp;
        SwipeController.Instance.OnSwipeDown -= SwipeController_OnSwipeDown;
        SwipeController.Instance.OnSwipeLeft -= SwipeController_OnSwipeLeft;
        SwipeController.Instance.OnSwipeRight -= SwipeController_OnSwipeRight;
    }

    private void SwipeController_OnSwipeRight() {
        moveDirection = Vector2.right;
    }

    private void SwipeController_OnSwipeLeft() {
        moveDirection = Vector2.left;
    }

    private void SwipeController_OnSwipeDown() {
        moveDirection = Vector2.down;
    }

    private void SwipeController_OnSwipeUp() {
        moveDirection = Vector2.up;
    }



    private void MovePlayer() {
        playerMoving = true;
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);

    }

    private void StopPlayer() {
        playerMoving = false;
        moveDirection = Vector2.zero;
    }

    private bool CheckForCollidersInPath() {

        Vector2 size = new Vector2(0.3f, 0.3f);
        float length = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, size, 0, moveDirection, length, collisionLayer);
        RaycastHit2D collectableHit = Physics2D.BoxCast(transform.position, size, 0, moveDirection, length, collectableLayer);
        if (raycastHit) {
            CheckWallCollision(raycastHit);
            return false;
        }
        if (collectableHit) {
            CheckForCollectables(collectableHit);
            return true;
        }


        return CheckWinPointProximity();
    }

    private void CheckForCollectables(RaycastHit2D raycastHit) {
        if (raycastHit.collider.TryGetComponent<ICollectable>(out ICollectable collectable)) {
            collectable.Collect();
            if (collectable is TeleportPoint) {
                HandleTeleport((TeleportPoint) collectable);
            }
        }
    }

    private void CheckWallCollision(RaycastHit2D raycastHit) {
        if (raycastHit.collider.TryGetComponent<ICollidable>(out ICollidable collidable)) {
            collidable.Collide();
            RotateAwayFromCollision(raycastHit.point);
            StopPlayer();

            bool isColourDifferent = WallColourController.Instance.IsColourDifferent((Wall)collidable);

            if (!isColourDifferent) {
                // The player's color matches the wall's color; play particle effect
                if (moveDirection == Vector2.zero) {
                    PlayWallCollisionParticles();
                }

            } else {
                // Play lose sound before destroy
                OnPlayerLose?.Invoke(this, new OnPlayerLoseEventArgs {
                    position = transform.position
                });
                StopPlayer();
                HideSelf(); // Player loses when colliding to a wall
                GameManager.Instance.State = GameStates.Lose;
            }
        }
    }
    private void HandleTeleport(TeleportPoint teleportPoint) {
        if (!teleportPoint.Teleported) {
            // Teleport cooldown logic is implemented in TeleportPoint script
            StopPlayer();
            // Started teleport so set trail to false
            PauseTrail();

            teleportPoint.TeleportPlayer(this.transform, out newMoveDirection);
            OnPlayerTeleport?.Invoke(this, new OnPlayerTeleportEventArgs {
                position = transform.position
            });
            // Move player towards modifiedMoveDirection from TeleportPoint
            // Player teleported so set trail to true
            moveDirection = newMoveDirection;
            MovePlayer();
        }
    }


    private bool CheckWinPointProximity() {
        // Winpoint detection is not implemented by raycast but by distance
        // Singleton reference is taken as there will be only one winpoint per scene
        float distanceToWinPoint = Vector2.Distance(transform.position, WinPoint.Instance.transform.position);

        if (distanceToWinPoint <= 0.5f) {
            GameManager.Instance.State = GameStates.Win;
            OnWinPointReached?.Invoke(this, new OnWinPointReachedEventArgs {
                position = transform.position
            });
            StopPlayer();
            HideSelf();
            return false; // Return false as the player should stop when it reaches win point   
        }

        return true;
    }

    private void PlayWallCollisionParticles() {
        // Ensure particles are played only during actual collision
        ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
        particles.Play();
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

    public void HideMeshWithColour(Colour hideColour) {
        // All the required meshes are a child of the player
        // Cycles through the children and hides all children
        foreach (GameObject playerMesh in playerMeshes) {
            if (playerMesh.gameObject.GetComponent<PlayerColour>().GetCurrentPlayerMeshColour() == hideColour) {
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
    public void ShowSelf() {
        this.gameObject.SetActive(true);
    }


    private void RotateInMoveDirection() {
        float _angle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -_angle));
    }
    public void RotateAwayFromCollision(Vector2 collisionPoint) {
        Vector2 directionAwayFromCollision = (Vector2)transform.position - collisionPoint;
        float angle = Mathf.Atan2(directionAwayFromCollision.x, directionAwayFromCollision.y) * Mathf.Rad2Deg;
        float snappedAngle = Mathf.Round(angle / 90) * 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -snappedAngle));
    }

    public Colour GetColour() {
        return currentColour;
    }

    public bool CanPlayerMove() {
        return playerMoving;
    }
    public void SetCurrentPlayerColour(Colour setColour) {
        currentColour = setColour;
    }
}
