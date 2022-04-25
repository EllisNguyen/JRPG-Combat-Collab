using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] FaceDirection defaultDirection = FaceDirection.down;

    //Parameters.
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }

    [SerializeField] int idleFrame = 1;
    SpriteAnimator currentAnim;

    bool wasPreviouslyMoving;

    //States.
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);

        SetFacingDirection(defaultDirection);
    }

    private void Update()
    {
        //Store current anim.
        var prevAnim = currentAnim;

        if (MoveX == 1)
            currentAnim = walkRightAnim;
        else if (MoveX == -1)
            currentAnim = walkLeftAnim;
        else if (MoveY == 1)
            currentAnim = walkUpAnim;
        else if (MoveY == -1)
            currentAnim = walkDownAnim;

        //Check if the animation changes.
        if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
            currentAnim.Start();

        if (IsMoving)
            currentAnim.HandleUpdate();
        else
            spriteRenderer.sprite = currentAnim.Frames[idleFrame];

        wasPreviouslyMoving = IsMoving;
    }

    public void SetFacingDirection(FaceDirection dir)
    {
        if (dir == FaceDirection.right)
            MoveX = 1;
        else if (dir == FaceDirection.left)
            MoveX = -1;
        else if (dir == FaceDirection.up)
            MoveY = 1;
        else if (dir == FaceDirection.down)
            MoveY = -1;
    }

    public FaceDirection DefaultDirection { get => defaultDirection; }
}

public enum FaceDirection { up, down, left, right }