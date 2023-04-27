using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectButton : MonoBehaviour
{
    [SerializeField]
    private HeroQualityType _myType = HeroQualityType.None;

    public void HeroTypeSelect()
    {
        HeroSelectManager.Instance.QualityTypeSet(_myType);
    }

    private void Start()
    {
        transform.Find("background").GetComponent<Button>().onClick.AddListener(() =>
        {
            HeroTypeSelect();
        });
    }
}
