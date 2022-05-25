using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterAnim : MonoBehaviour
{
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] FacingDirection defaultDirection = FacingDirection.right;
    [SerializeField] bool flip;

    //Parameters.
    public float MoveX { get; set; }
    public bool IsMoving { get; set; }

    [SerializeField] int idleFrame = 1;
    SpriteAnimator currentAnim;

    bool wasPreviouslyMoving;

    //States.
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;

    public List<Sprite> WalkRightSprites
    {
        get
        {
            return walkRightSprites;
        }
        set
        {
            walkRightSprites = value;
        }
    }

    public List<Sprite> WalkLeftSprites
    {
        get
        {
            return walkLeftSprites;
        }
        set
        {
            walkLeftSprites = value;
        }
    }

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer, 0.21f);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer, 0.21f);

        SetFacingDirection(defaultDirection);
    }

    private void Update()
    {
        //Store current anim.
        var prevAnim = currentAnim;

        if (MoveX > 0)
        {
            currentAnim = walkRightAnim;
        }
        else if (MoveX < 0)
        {
            currentAnim = walkLeftAnim;
        }

        //Check if the animation changes.
        if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
            currentAnim.Start();

        if (IsMoving)
            currentAnim.HandleUpdate();
        //else
        //    spriteRenderer.sprite = currentAnim.Frames[idleFrame];

        wasPreviouslyMoving = IsMoving;
        spriteRenderer.flipX = flip;
    }

    public void SetFacingDirection(FacingDirection dir)
    {
        if (dir == FacingDirection.right)
        {
            MoveX = 1;
        }
        else if (dir == FacingDirection.left)
        {
            MoveX = -1;
        }
    }

    public FacingDirection DefaultDirection { get => defaultDirection; }

    public bool FlipSprite(bool isFlipped = false)
    {
        flip = isFlipped;

        return flip;
    }
}

public enum FacingDirection { left, right }

