using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCategory : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI _categoryName;
    [SerializeField] private Transform _viewParent;

    private float _sizeY;

    public Transform ViewParent => _viewParent;
    public float SizeY => _sizeY;

    public void Init()
    {
        SetSizeContent();
    }

    private void SetSizeContent()
    {
        GridLayoutGroup grid = _viewParent.GetComponent<GridLayoutGroup>();
        RectTransform rect = transform.GetComponent<RectTransform>();

        int childCount = _viewParent.childCount;

        int rows = 1;
        int columns = 3;

        if (grid.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            columns = grid.constraintCount;
            rows = Mathf.CeilToInt((float)childCount / columns);
        }

        float height = grid.padding.top + grid.padding.bottom
                     + rows * grid.cellSize.y
                     + (rows - 1) * grid.spacing.y + 160;

        _sizeY = height;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
    }
}
