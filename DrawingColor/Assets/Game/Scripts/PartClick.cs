using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public int             id;
    public int             idColor;
    /// <summary>
    /// Initializes a new instance of the <see cref="MoreMountains.TopDownEngine.TopDownEngineEvent"/> struct.
    /// </summary>
    /// <param name="eventType">Event type.</param>
    public PartClickActionEvent(PartClickAction partClickAction,int _id, int _idColor)
    {
        PartClickAction = partClickAction;
        id = _id;
        idColor = _idColor;
    }

    static PartClickActionEvent e;
    public static void Trigger(PartClickAction partClickAction,int id,int idColor)
    {
        e.PartClickAction = partClickAction;
        e.id = id;
        e.idColor = idColor;
        MMEventManager.TriggerEvent(e);
    }
}     
public class PartClick : MonoBehaviour
{

    #region Public Variables

    public Collider2D     collider2D;
    public SpriteRenderer whiteSprite;

    #endregion

    #region Private Variables

    [ReadOnly][ShowInInspector] private int id;
    [ReadOnly][ShowInInspector] private int idColor;

    #endregion

    public void SetID(int _id)
    {
        id = _id;
    }
    public void SetIDColor(int _id)
    {
        idColor = _id;
    }
    public void OnHighLight()
    {
        
    }

    [Button]
    public void OnNotColor()
    {
        collider2D.enabled = true;
        whiteSprite.gameObject.SetActive(true);
        whiteSprite.maskInteraction = SpriteMaskInteraction.None;
    }

    [Button]
    public void OnColoring()
    {
        collider2D.enabled = false;
        whiteSprite.gameObject.SetActive(true);
        whiteSprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
    }
    [Button]
    public void OnColored()
    {
        PartClickActionEvent.Trigger( PartClickAction.OnPartFillComplete,id,idColor);
        collider2D.enabled = false;
        whiteSprite.gameObject.SetActive(false);
        whiteSprite.maskInteraction = SpriteMaskInteraction.None;
    }

    public void OnHint()
    {
        
    }

    [Button]
    public void AutoGetRef()
    {
        collider2D   = this.GetComponent<Collider2D>();
        whiteSprite = this.transform.GetComponent<SpriteRenderer>();
    }
}
