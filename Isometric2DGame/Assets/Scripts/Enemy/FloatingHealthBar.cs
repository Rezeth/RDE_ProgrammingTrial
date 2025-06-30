using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar: MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (slider != null)
        {
            slider.value = currentHealth / maxHealth;
        }
    }
}
