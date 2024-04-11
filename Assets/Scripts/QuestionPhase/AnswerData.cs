using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerData : MonoBehaviour 
{
  [Header("UI Elements")]
  [SerializeField] TextMeshProUGUI infoTextObject = null;
  [SerializeField] TextMeshProUGUI letterTextObject = null;
  [SerializeField] Image toggle = null;

  [Header("Textures")]
  [SerializeField] Color  uncheckedToggle;
  [SerializeField] Color checkedToggle;

  [Header("References")]
  [SerializeField] Q_GameEvents events = null;

  private RectTransform _rect = null;
  public  RectTransform Rect
  {
    get
    {
      if (_rect == null)
      {
        _rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
      }
      return _rect;
    }
  }

  private int _answerIndex = -1;
  public int AnswerIndex { get { return _answerIndex; } }

  private bool Checked = false;

  public void UpdateData (string info, int index)
  {
    infoTextObject.text = info;
    letterTextObject.text = ".";
    _answerIndex = index;
  }

  public void Reset ()
  {
    Checked = false;
    UpdateUI();
  }

  public void SwitchState ()
  {
    Checked = !Checked;
    UpdateUI();

    if (events.UpdateQuestionAnswer != null)
    {
      events.UpdateQuestionAnswer(this);
    }
  }

  void UpdateUI ()
  {
    if (toggle == null) return;

    toggle.color = (Checked) ? checkedToggle : uncheckedToggle;
  }
}