using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCooldownIconView : MonoBehaviour
{
    [System.Serializable]
    private class SkillCooldownSlot
    {
        public SkillId skillId;
        public Image cooldownOverlay;
        public TMP_Text cooldownText;
    }

    [Header("Cooldown Source")]
    [SerializeField] private SkillCooldownManager cooldownManager;

    [Header("Slots")]
    [SerializeField] private SkillCooldownSlot[] slots;

    private void Update()
    {
        if (cooldownManager == null || slots == null)
            return;

        foreach (SkillCooldownSlot slot in slots)
        {
            UpdateSlot(slot);
        }
    }

    private void UpdateSlot(SkillCooldownSlot slot)
    {
        if (slot == null)
            return;

        float remainingTime = cooldownManager.GetRemainingTime(slot.skillId);
        bool isCoolingDown = remainingTime > 0f;

        if (slot.cooldownOverlay != null)
        {
            slot.cooldownOverlay.gameObject.SetActive(isCoolingDown);
            slot.cooldownOverlay.fillAmount = GetFillAmount(remainingTime);
        }

        if (slot.cooldownText != null)
        {
            slot.cooldownText.gameObject.SetActive(isCoolingDown);
            slot.cooldownText.text = Mathf.CeilToInt(remainingTime).ToString();
        }
    }

    private float GetFillAmount(float remainingTime)
    {
        float duration = cooldownManager.GetCooldownDuration();

        if (duration <= 0f)
            return 0f;

        return Mathf.Clamp01(remainingTime / duration);
    }
}