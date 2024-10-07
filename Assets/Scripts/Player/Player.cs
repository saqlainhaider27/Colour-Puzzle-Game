using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player> {

    [Header("Player Settings")]
    [SerializeField] private float moveSpeed;
    private Vector2 moveDirection;
    [SerializeField] private float raycastLength = 0.5f;
    [SerializeField] private List<GameObject> playerMeshes = new List<GameObject>();

    [Header("References")]
    [SerializeField] private SwipeDetection swipeDetection;
    [SerializeField] private LayerMask collisionLayer;


    private bool generated = false;
    private bool canMove;
    [SerializeField] private Colour currentColour;
    private bool teleported = false;
    private float lastTeleportTime;
    private float teleportCooldownTime = 0.1f;

    private void Start() {
        currentColour = GetComponentInChildren<PlayerColour>().GetCurrentPlayerMeshColour();
    }
    private void Update() {
        moveDirection = swipeDetection.GetSwipeDirection();
        canMove = IsPathClear() && moveDirection != Vector2.zero;
        if (canMove) {
            transform.position += (Vector3)moveDirection * Time.deltaTime * moveSpeed;
            RotateInMoveDirection(); // Rotate object in move direction
        }
        else {
            // Stop player movement if WinPoint reached or collision detected
            moveDirection = Vector2.zero;

        }

        if (Time.time >= lastTeleportTime + teleportCooldownTime) {
            lastTeleportTime = Time.time;
            teleported = false;
        }


    }

    private bool IsPathClear() {
        // Check for collision with anything on the collision layer
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, moveDirection, raycastLength, collisionLayer);
        if (raycastHit) {
            if (raycastHit.collider.GetComponent<Wall>() != null) {
                // Debug.Log("Collided Wall");

                RotateAwayFromCollision(raycastHit.point);
                Wall collidedWall = raycastHit.collider.GetComponent<Wall>();
                bool WallColourDifferent = WallColourController.Instance.IsColourDifferent(collidedWall);
                if (WallColourDifferent) {
                    DestroySelf();
                }
                return false; // Collision detected, cannot move

            }
            else if (raycastHit.collider.GetComponent<Paint>() != null) {
                // Switch colour if paint is different than current colour
                Paint collidedPaint = raycastHit.collider.GetComponent<Paint>();
                bool switchColour = ColourSwitcher.Instance.IsColourDifferent(collidedPaint); // Checks if colour is different from collided paint
                if (switchColour) {
                    generated = false;
                }
                if (!generated) {
                    // Switch colour if different
                    ColourSwitcher.Instance.SwitchColour(collidedPaint);
                    generated = true;
                }

                return true;
            }
            else if (raycastHit.collider.GetComponent<TeleportPoint>() != null) {
                if (!teleported) {
                    Vector2 newMoveDirection;
                    raycastHit.collider.GetComponent<TeleportPoint>().TeleportPlayer(this.transform, out newMoveDirection);
                    moveDirection = newMoveDirection;
                    teleported = true;
                }
                //TeleportController.Instance.TeleportPlayer(this.transform);
                return true;
            }
            return false;
        }


        // Check if close enough to WinPoint (using distance)
        float distanceToWinPoint = Vector2.Distance(transform.position, WinPoint.Instance.transform.position);
        if (distanceToWinPoint <= 0.1f) {
            UIController.Instance.ShowWinMenu();
            HideSelf();
            return false;
        }

        return true; // No collision and not close enough to WinPoint
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
    public void HideMeshWithColour(Colour hideColour) {
        foreach (GameObject playerMesh in playerMeshes) {
            if (playerMesh.gameObject.GetComponent<PlayerColour>().GetCurrentPlayerMeshColour() ==  hideColour) {
                playerMesh.gameObject.SetActive(false);
            }
        }
    }
    public void ShowMeshWithColour(Colour showColour) {
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
    private void RotateAwayFromCollision(Vector2 collisionPoint) {
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
}
