using Helpers;
using UnityEngine;


public class FieldManager : MonoBehaviour
{
    [SerializeField] private GameObject fieldPrefab;
    [SerializeField] private float moveSpeed = 5f;

    public bool canSpawnField;
    public bool canMoveFields;

    private readonly MyQueue<GameObject> _fieldGameObjectsQueue = new MyQueue<GameObject>();
    private readonly Vector3 _offset = new Vector3(0f, 8.78f, 0f);
    private readonly Vector3 _movePosition = new Vector3(0f, -4f, 0f);

    private void Awake()
    {
        _fieldGameObjectsQueue.Enqueue(Instantiate(fieldPrefab, Vector3.zero, Quaternion.identity));
    }

    private void Update()
    {
        if (_fieldGameObjectsQueue.Count > 1)
        {
            if (_fieldGameObjectsQueue.Peek().transform.position.y < -30f)
            {
                DestroyField();
            }
        }

        if (canMoveFields) MoveFields();
    
        // isso sera movido para uma funçao que verifica se o player fez gol
        if (!canSpawnField) return;
        InstantiateNextField();

        canSpawnField = false;
    }

    private void InstantiateNextField()
    {
        _fieldGameObjectsQueue.Enqueue(Instantiate(fieldPrefab, 
            _fieldGameObjectsQueue.Last.transform.position + _offset, Quaternion.identity));
    }

    private void MoveFields()
    {
        foreach (GameObject field in _fieldGameObjectsQueue)
        {
            Transform fieldTransform = field.transform;
            var position = fieldTransform.position;
            position = Vector3.MoveTowards(position, position +
                                                     _movePosition, Time.deltaTime * moveSpeed);
            fieldTransform.position = position;
        }
    }

    private void DestroyField()
    {
        Destroy(_fieldGameObjectsQueue.Dequeue());
    }
}

