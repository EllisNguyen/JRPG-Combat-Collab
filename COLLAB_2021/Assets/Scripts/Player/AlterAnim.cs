using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterAnim : MonoBehaviour
{
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] FacingDirection defaultDirection = FacingDirection.down;
    [SerializeField] bool flip;

    //Parameters.
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }
    public bool Flip { get; set; }

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

        //Check if the animation changes.
        if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
            currentAnim.Start();

        if (IsMoving)
            currentAnim.HandleUpdate();
        else
            spriteRenderer.sprite = currentAnim.Frames[idleFrame];

        wasPreviouslyMoving = IsMoving;
    }

    public void SetFacingDirection(FacingDirection dir)
    {
        if (dir == FacingDirection.right)
        {
            flip = false;
            MoveX = 1;
        }
        else if (dir == FacingDirection.left)
        {
            flip = true;
            MoveX = -1;
        }
    }

    public FacingDirection DefaultDirection { get => defaultDirection; }
}

public enum FacingDirection { up, down, left, right }