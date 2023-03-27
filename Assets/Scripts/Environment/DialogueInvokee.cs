using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInvokee : Invokee
{
    [SerializeField] private DialogueObject m_dialogueObject;
    [SerializeField] private bool m_playOnAwake = false;

    private void Start()
    {
        if (m_playOnAwake) DialogueManager.Instance.DisplayDialogue(m_dialogueObject);
    }

    protected override void OnActivate()
    {
        DialogueManager.Instance.DisplayDialogue(m_dialogueObject);
    }

    protected override void OnDeactivate()
    {
        
    }



}