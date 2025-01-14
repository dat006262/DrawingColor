using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
public enum PartClickAction
{
    OnPartFillComplete,
        
}
public struct PartClickActionEvent
{
    public PartClickAction PartClickAction;

    /// <summary>
    /// Initializes a new instance of the <see cref="MoreMountains.TopDownEngine.TopDownEngineEvent"/> struct.
    /// </summary>
    /// <param name="eventType">Event type.</param>
    public PartClickActionEvent(PartClickAction partClickAction)
    {
        PartClickAction = partClickAction;
    }

    static PartClickActionEvent e;
    public static void Trigger(PartClickAction partClickAction)
    {
        e.PartClickAction = partClickAction;
        MMEventManager.TriggerEvent(e);
    }
}     
public class PartClick : MonoBehaviour
{
  
    
    public Collider2D     collider2D;
    public SpriteRenderer FilledSprite;
    
    public void OnHighLight()
    {
        
    }

    [Button]
    public void OnNotColor()
    {
        collider2D.enabled = true;
        FilledSprite.gameObject.SetActive(false);
        FilledSprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }

    [Button]
    public void OnColoring()
    {
        collider2D.enabled = false;
        FilledSprite.gameObject.SetActive(true);
        FilledSprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }
    [Button]
    public void OnColored()
    {
        collider2D.enabled = false;
        FilledSprite.gameObject.SetActive(true);
        FilledSprite.maskInteraction = SpriteMaskInteraction.None;
    }

    public void OnHint()
    {
        
    }

    [Button]
    public void AutoGetRef()
    {
        collider2D   = this.GetComponent<Collider2D>();
        FilledSprite = this.transform.Find("FilledImage")?.GetComponent<SpriteRenderer>();
    }
}
