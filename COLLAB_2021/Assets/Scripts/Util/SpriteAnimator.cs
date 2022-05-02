using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator
{

    [SerializeField] SpriteRenderer spriteRenderer;
    List<Sprite> frames;
    float frameRate;

    int currentFrame;
    float timer;

    /// <summary>
    /// Contrusctor to init the class.
    /// </summary>
    /// <param name="frames">List of sprites.</param>
    /// <param name="spriteRenderer">The graphic of the character.</param>
    /// <param name="frameRate">Frame rate that the animation will run at, default frame rate is 16.</param>
    public SpriteAnimator(List<Sprite> frames, SpriteRenderer spriteRenderer, float frameRate = .16f)
    {
        this.frames = frames;
        this.spriteRenderer = spriteRenderer;
        this.frameRate = frameRate;
    }

    /// <summary>
    /// Reset the frame and timer. Set the refereced sprite renderer as the first frame in the list.
    /// </summary>
    public void Start()
    {
        currentFrame = 0;
        timer = 0;

        spriteRenderer.sprite = frames[0];
    }

    /// <summary>
    /// Run by Update method in player entity or enemy entity to update their animation when moving.
    /// </summary>
    public void HandleUpdate()
    {
        //Increment of timer.
        timer += Time.deltaTime;

        //Animate the character.
        if(timer > frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Count;
            spriteRenderer.sprite = frames[currentFrame];
            timer -= frameRate;
        }
    }

    public List<Sprite> Frames { get { return frames; } }

}
