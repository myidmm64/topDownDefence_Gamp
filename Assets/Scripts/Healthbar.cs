using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    private Transform separatorContainer;

    Transform barTransform;

    private void Awake()
    {
        barTransform = transform.Find("bar");
    }

    private void Start()
    {
        separatorContainer = transform.Find("separatorContainer");
        ConstrucHealthBarSeparators();
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnHealthAmoundMaxChanged += HealthSystem_OnHealthAmoundMaxChanged;
        UpdateBar();
        UpdateBarVisible();
    }

    private void ConstrucHealthBarSeparators()
    {
        Transform separatorTemplate = separatorContainer.Find("separatorTemplate");
        separatorContainer.gameObject.SetActive(true);

        foreach(Transform separatorTransform in separatorContainer)
        {
            if (separatorTransform == separatorTemplate) continue;
            Destroy(separatorTransform.gameObject);
        }

        int healthAmountPerSeparator = 10;
        float barSize = 4f;
        float barOneHealthAmountSize = barSize / healthSystem.GetHealthAmountMax();
        int healthSeparatorCOunt = Mathf.FloorToInt(healthSystem.GetHealthAmountMax() / healthAmountPerSeparator);
        for (int i = 1; i < healthSeparatorCOunt; i++)
        {
            Transform separatorTransform = Instantiate(separatorTemplate, separatorContainer);
            separatorTransform.gameObject.SetActive(true);
            separatorTransform.localPosition = new Vector3(barOneHealthAmountSize * i * healthAmountPerSeparator, 0, 0);
        }
    }

    private void HealthSystem_OnHealthAmoundMaxChanged(object sender, System.EventArgs e)
    {
        ConstrucHealthBarSeparators();
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        UpdateBar();
        UpdateBarVisible();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        UpdateBar();
        UpdateBarVisible();
    }

    private void UpdateBar()
    {
        barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
    }

    private void UpdateBarVisible()
    {
        if (healthSystem.IsFullHealth())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
