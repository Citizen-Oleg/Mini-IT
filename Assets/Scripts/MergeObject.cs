using UnityEngine;

public class MergeObject : MonoBehaviour
{
    public int ID => _id;
        
    [SerializeField]
    private int _id;
    [SerializeField]
    private float _rotationSpeed = 1.0f;

    private bool _isRotate = true;
    private MergeObjectProvider _mergeObjectProvider;

    public void Initialize(MergeObjectProvider mergeObjectProvider)
    {
        _mergeObjectProvider = mergeObjectProvider;
    }

    public void Release()
    {
        _mergeObjectProvider.ReleaseModel(this);
    }

    public void StopRotate()
    {
        _isRotate = false;
    }
        
    private void FixedUpdate() 
    {
        if (_isRotate)
        {
            transform.Rotate(Vector3.up, _rotationSpeed);
        }
    }

    private void OnDisable()
    {
        _isRotate = true;
    }
}