using System;
using System.Collections.Generic;
using SimpleEventBus.Disposables;
using Tools;
using Tools.SimpleEventBus;
using UnityEngine;
using Zenject;

public class MergeController : MonoBehaviour
{
    public MergeObject CurrentSelectObject => _selectSlot.MergeObject;

    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Vector3 _offSet;

    private MergeObjectProvider _mergeObjectProvider;
    private SlotManager _slotManager;
    private ParticleSpawner _particleSpawner;

    private Slot _selectSlot;

    private Vector3 _startPosisition;
    private Vector3 _distance;
    private Dictionary<int, List<Slot>> _dictionary = new Dictionary<int, List<Slot>>();

    private CompositeDisposable _subscription;
        
    [Inject]
    public void Initialize(MergeObjectProvider mergeObjectProvider, SlotManager slotManager, ParticleSpawner particleSpawner)
    {
        _particleSpawner = particleSpawner;
        _mergeObjectProvider = mergeObjectProvider;
        _slotManager = slotManager;

        for (var i = 0; i < _mergeObjectProvider.MaxLevel; i++)
        {
            _dictionary.Add(i, new List<Slot>());
        }

        _subscription = new CompositeDisposable
        {
            EventStreams.UserInterface.Subscribe<EventAutoMerge>(AutoMerge),
            EventStreams.UserInterface.Subscribe<EventSpawn>(AddMergeObject)
        };
    }
    
    private void OnPointerDownSlot(Slot slot)
    {
        if (slot.MergeObject == null)
        {
            return;
        }
            
        _selectSlot = slot;
        _selectSlot.HideText();
        _startPosisition = Camera.main.WorldToScreenPoint(_selectSlot.MergeObject.transform.position);
        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _startPosisition.z);
        _distance = _selectSlot.MergeObject.transform.position - Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void Merge(Slot slotMerge)
    {
        var level = _selectSlot.MergeObject.ID;
        if (!_mergeObjectProvider.HasNextLevel(level))
        {
            ResetSelectSlot();
            return;
        }
        
        _mergeObjectProvider.ReleaseModel(_selectSlot.MergeObject);
        _mergeObjectProvider.ReleaseModel(slotMerge.MergeObject);
        
        _dictionary[level].Remove(slotMerge);
        _dictionary[level].Remove(_selectSlot);
            
        var mergeObject = _mergeObjectProvider.GetGameObjectByLevel(++level);
        slotMerge.SetMergeObject(mergeObject);

        _dictionary[level].Add(slotMerge);

        _selectSlot.SetMergeObject();
        _selectSlot = null;
        _particleSpawner.Show(slotMerge.PositionFx.position);
    }
        
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var slot = RayCastSlot();
            if (slot != null)
            {
                OnPointerDownSlot(slot);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            SetNearestAvailableSlot();
        }
            
        if (_selectSlot != null && _selectSlot.MergeObject != null)
        {
            Vector3 lastPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _startPosisition.z);
            _selectSlot.MergeObject.transform.position = Camera.main.ScreenToWorldPoint(lastPos) + _distance + _offSet;
        }
    }

    private Slot RayCastSlot()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo) && hitInfo.collider.TryGetComponent(out Slot slot))
        {
            return slot;
        }

        return null;
    }
    
    private void ResetSelectSlot()
    {
        if (_selectSlot != null)
        {
            _selectSlot.SetMergeObject(_selectSlot.MergeObject, false);
            _selectSlot = null;
        }
    }
    
    private void AutoMerge(EventAutoMerge eventAutoMerge)
    {
        foreach (var keyValuePair in _dictionary)
        {
            if (keyValuePair.Value.Count >= 2)
            {
                ResetSelectSlot();

                Slot firstSlot = null;
                Slot secondSlot = null;
                    
                foreach (var slot in keyValuePair.Value)
                {
                    if (firstSlot == null)
                    {
                        firstSlot = slot;
                        continue;
                    }

                    secondSlot = slot;
                    break;
                    
                }

                if (firstSlot == null || secondSlot == null)
                {
                    continue;
                }
                    
                _selectSlot = keyValuePair.Value[0];
                Merge(keyValuePair.Value[1]);
                return;
            }
        }
    }

    private void SetNearestAvailableSlot()
    {
        if (_selectSlot == null || _selectSlot.MergeObject == null)
        {
            return;
        }
            
        var position = _selectSlot.MergeObject.transform.position;
            
        var minDistance = float.MaxValue;
        var minIndex = 0;

        for (var index = 0; index < _slotManager.Slots.Count; index++)
        {
            var target = _slotManager.Slots[index];

            var distanceToTarget = Vector3.Distance(target.transform.position, position);

            if (minDistance > distanceToTarget)
            {
                minDistance = distanceToTarget;
                minIndex = index;
            }
        }

        var slot = _slotManager.Slots[minIndex];
        if (slot.Equals(_selectSlot))
        {
            _selectSlot.SetMergeObject(_selectSlot.MergeObject, false);
            _selectSlot = null;
            return;
        }

        if (slot.MergeObject == null)
        {
            slot.SetMergeObject(_selectSlot.MergeObject, false);
                
            _dictionary[_selectSlot.MergeObject.ID].Add(slot);
            _dictionary[_selectSlot.MergeObject.ID].Remove(_selectSlot);
                
            _selectSlot.SetMergeObject();
            _selectSlot = null;
        }
        else if (slot.MergeObject.ID == _selectSlot.MergeObject.ID && 
                 _mergeObjectProvider.HasNextLevel(slot.MergeObject.ID))
        {
            Merge(slot);
        }
        else
        {
            ResetSelectSlot();
        }
    }

    private void AddMergeObject(EventSpawn eventSpawn)
    {
        _dictionary[eventSpawn.Slot.MergeObject.ID].Add(eventSpawn.Slot);
    }

    private void OnDestroy()
    {
        _subscription?.Dispose();
    }
}