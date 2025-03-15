using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public MergeObject MergeObject => _mergeObject;
    public Transform PositionFx => _positionFX;
    public Text Text => _text;

    [SerializeField]
    private Transform _positionObject;
    [SerializeField]
    private Transform _positionFX;
    [SerializeField]
    private Text _text;

    private MergeObject _mergeObject;

    public void HideText()
    {
        _text.text = "";
    }

    public void SetMergeObject(MergeObject mergeObject = null, bool resetRotaion = true)
    {
        _mergeObject = mergeObject;
            
        if (_mergeObject != null)
        {
            _mergeObject.transform.parent = _positionObject;
            _mergeObject.transform.localPosition = Vector3.zero;
            _mergeObject.transform.localScale = Vector3.one;
            _text.text = (_mergeObject.ID + 1).ToString();
                
            if (resetRotaion)
            {
                _mergeObject.transform.localRotation = Quaternion.identity;
            }
            
            _mergeObject.gameObject.SetActive(true);
        }
        else
        {
            _text.text = "";
        }
    }
}