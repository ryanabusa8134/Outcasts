using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int chestID;
    private Animator animator;
    private bool isOpen = false;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        if (ChestTracker.Instance.IsChestFound(chestID))
        {
            animator.Play("ChestOpen");
            isOpen = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen && collision.GetComponent<Pawn>())
        {
            animator.Play("ChestOpen");
            ChestTracker.Instance.FoundNewChest(chestID);
            isOpen = true;
        }
    }
    #if UNITY_EDITOR
    protected void OnDrawGizmos()
    {
        Handles.Label(transform.position, "CHEST " + chestID, EditorStyles.boldLabel);
    }
    #endif
}
