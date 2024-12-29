using UnityEngine;

public class UnitSelectionManagerUI : MonoBehaviour
{
    [SerializeField] private RectTransform _selectionAreaRectTransform;
    [SerializeField] private Canvas _canvas;

    private void Start()
    {
        UnitSelectionManager.Instance.OnSelectinoAreaStart += UnitSelectionManager_OnSelectinoAreaStart;
        UnitSelectionManager.Instance.OnSelectinoAreaEnd += UnitSelectionManager_OnSelectinoAreaEnd;

        _selectionAreaRectTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(_selectionAreaRectTransform.gameObject.activeSelf)
        {
            UpdateVisual();
        }
    }
    private void UnitSelectionManager_OnSelectinoAreaStart(object sender, System.EventArgs e)
    {
        _selectionAreaRectTransform.gameObject.SetActive(true);
        UpdateVisual();
    }
    private void UnitSelectionManager_OnSelectinoAreaEnd(object sender, System.EventArgs e)
    {
        _selectionAreaRectTransform.gameObject.SetActive(false );
        
    }
    private void UpdateVisual()
    {
        Rect selectionAreaRect = UnitSelectionManager.Instance.GetSelectionArea();
        float canvasScale = _canvas.transform.localScale.x;
        _selectionAreaRectTransform.anchoredPosition = new Vector2(selectionAreaRect.x, selectionAreaRect.y) / canvasScale;
        _selectionAreaRectTransform.sizeDelta = new Vector2(selectionAreaRect.width, selectionAreaRect.height) / canvasScale;
    }



}
