using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerData : MonoBehaviour
{
  [Header("UI Elements")]
  [SerializeField] TextMeshProUGUI infoTextObject;
  [SerializeField] Image toogle;

  [Header("Textures")]
  [SerializeField] Sprite uncheckedToggle;
  [SerializeField] Sprite checkedToggle;

  private int _answerIndex = -1;
  public int AnswerIndex { get { return _answerIndex; } }
}