using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerData : MonoBehaviour
{
  [Header("UI Elements")]
  [SerializeField] TextMeshProUGUI infoTextObject;
  [SerializeField] Image toggle;

  [Header("Textures")]
  [SerializeField] Sprite uncheckedToggle;
  [SerializeField] Sprite checkedToggle;

  [Header("References")]
  [SerializeField] Q_GameEvents events;

  private RectTransform _rect;
  public RectTransform Rect
  {
    get
    {
      if (_rect == null)
      {
        // ?? -> returns the first if not null, otherwise reutrns the second
        _rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
      }
      return _rect;
    }
  }

  private int _answerIndex = -1;
  public int AnswerIndex { get { return _answerIndex; } }

  private bool Checked = false;

  public void UpdateData(string info, int index)
  {
    infoTextObject.text = info;
    _answerIndex = index;
  }

  public void Reset()
  {
    Checked = false;
    UpdateUI();
  }

  public void SwitchState()
  {
    Checked = !Checked;

    if (events.UpdateQuestionAnswer != null)
      events.UpdateQuestionAnswer(this);
  }

  void UpdateUI()
  {
    toggle.sprite = (Checked) ? checkedToggle : uncheckedToggle;
  }
}