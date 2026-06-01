using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildItem : MonoBehaviour
{
    [SerializeField]
    private Image Icon;
    [SerializeField]
    private TextMeshProUGUI NameText;
    [SerializeField]
    private Button Button;
    
    private BaseBuildSO _buildSo;
    private BuildWindow _window;
    
    public void Init(BaseBuildSO build, BuildWindow window)
    {
        _buildSo = build;
        Icon.sprite = build.Icon;
        NameText.text = build.ID;
        _window = window;
    }

    private void OnEnable()
    {
        Button.onClick.AddListener(ForceSelect);
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(ForceSelect);
    }

    private void ForceSelect()
    {
        _window.Select(_buildSo);
    }
}
