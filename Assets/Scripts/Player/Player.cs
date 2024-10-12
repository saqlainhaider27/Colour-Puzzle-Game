using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player> {

    [Header("Player Settings")]
    [SerializeField] private float moveSpeed;
    private Vector2 moveDirection;
    private Vector2 newMoveDirection;

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
    private float teleportCooldownTime = 0.25f;

    private void Start() {
        currentColour = GetComponentInChildren<PlayerColour>().GetCurrentPlayerMeshColour();
    }
    private void Update() {
        HandleTeleportCooldown();
        UpdateMoveDirection();
        canMove = CanMove();

        if (canMove) {
            MovePlayer();
            RotateInMoveDirection(); // Rotate object in move direction
        }
        else {
            StopPlayer();
        }
    }

    private void HandleTeleportCooldown() {
        if (Time.time >= lastTeleportTime + teleportCooldownTime) {
            lastTeleportTime = Time.time;
            teleported = false;
        }
    }

    private void UpdateMoveDirection() {
        if (swipeDetection.GetSwipeDirection() != Vector2.zero) {
            moveDirection = swipeDetection.GetSwipeDirection();
        }
        else if (newMoveDirection != Vector2.zero && newMoveDirection != moveDirection) {
            moveDirection = newMoveDirection;
        }
    }
    private bool CanMove() {
        return CheckForCollidersInPath() && moveDirection != Vector2.zero;
    }
    private void MovePlayer() {
        transform.position += (Vector3)moveDirection * Time.deltaTime * moveSpeed;
    }
    private void StopPlayer() {
        moveDirection = Vector2.zero; // Stop player movement
    }
    private bool CheckForCollidersInPath() {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, moveDirection, raycastLength, collisionLayer);

        if (raycastHit) {
            if (CheckWallCollision(raycastHit))
                return false;
            if (CheckPaintCollision(raycastHit))
                return true;
            if (CheckTeleportPointCollision(raycastHit))
                return true;

            return false;
        }

        return CheckWinPointProximity();
    }
    private bool CheckWallCollision(RaycastHit2D raycastHit) {
        if (raycastHit.collider.GetComponent<Wall>() != null) {
            RotateAwayFromCollision(raycastHit.point);

            Wall collidedWall = raycastHit.collider.GetComponent<Wall>();
            bool WallColourDifferent = WallColourController.Instance.IsColourDifferent(collidedWall);

            if (WallColourDifferent) {
                DestroySelf();
                UIController.Instance.ShowLoseMenu();
            }

            return true; // Collision with wall detected
        }

        return false;
    }
    private bool CheckPaintCollision(RaycastHit2D raycastHit) {
        if (raycastHit.collider.GetComponent<Paint>() != null) {
            Paint collidedPaint = raycastHit.collider.GetComponent<Paint>();
            bool switchColour = ColourSwitcher.Instance.IsColourDifferent(collidedPaint);

            if (switchColour) {
                generated = false;
            }

            if (!generated) {
                ColourSwitcher.Instance.SwitchColour(collidedPaint);
                generated = true;
            }

            return true;
        }

        return false;
    }
    private bool CheckTeleportPointCollision(RaycastHit2D raycastHit) {
        if (raycastHit.collider.GetComponent<TeleportPoint>() != null) {
            if (!teleported) {
                teleported = true;
                raycastHit.collider.GetComponent<TeleportPoint>().TeleportPlayer(this.transform, out newMoveDirection);
            }

            return true; // Teleportation happened
        }

        return false;
    }
    private bool CheckWinPointProximity() {
        float distanceToWinPoint = Vector2.Distance(transform.position, WinPoint.Instance.transform.position);

        if (distanceToWinPoint <= 0.5f) {
            UIController.Instance.ShowWinMenu();
            HideSelf();
            return false;
        }

        return true;
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
