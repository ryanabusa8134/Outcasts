using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 
/// </summary>
public class DialogueManager : MonoBehaviour
{
    #region Singleton
    private static DialogueManager instance; 
    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogueManager>().GetComponent<DialogueManager>();
            }

            return instance;
        }
    }
    #endregion

    [SerializeField] private TextProducer m_dialogueProducer;
    [SerializeField] private Image profile;
    [SerializeField] private GameObject dialogueBox;
    private bool inProduction;

    private void Start()
    {
        HideDialogue();
    }

    private void Update()
    {
        
    }

    public void DisplayDialogue(DialogueObject a_dialogueObject)
    {
        if (inProduction) return;
        inProduction = true;
        dialogueBox.GetComponent<Animator>().Play("Appear");
        StartCoroutine(RunThroughDialogue(a_dialogueObject));
    }

    public void HideDialogue()
    {
        dialogueBox.GetComponent<Animator>().Play("Disappear");
        inProduction = false;
    }

    private IEnumerator RunThroughDialogue(DialogueObject a_dialogueObject)
    {
        foreach (Dialogue dialogue in a_dialogueObject.Dialogue)
        {
            AdjustProfileSegment(dialogue.Profile);
            m_dialogueProducer.TypeSound = dialogue.TypeSound;
            yield return m_dialogueProducer.ReplaceTextWith(dialogue.Text, ProduceEffect.Typewriter, dialogue.Speed, dialogue.Delay);
        }

        HideDialogue();
        yield return null;
    }

    private void AdjustProfileSegment(Sprite a_profile)
    {
        if (profile == null) return;
        profile.gameObject.SetActive(true);
        profile.sprite = null;
        profile.CrossFadeAlpha(0f, 0f, true);
        m_dialogueProducer.TMP_access.margin = new Vector4(0, 0, 0, 0);
        m_dialogueProducer.TMP_access.alignment = TextAlignmentOptions.Center;
        profile.sprite = a_profile;
        if (profile.sprite != null)
        {
            m_dialogueProducer.TMP_access.margin = new Vector4(250f, 0, 0, 0);
            m_dialogueProducer.TMP_access.alignment = TextAlignmentOptions.Left;
            profile.CrossFadeAlpha(1f, 0f, true);
        }
    }
}
