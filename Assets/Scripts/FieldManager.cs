using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField] private GameObject fieldPrefab;
    [SerializeField] private bool canSpawnField; // variavel necessaria para que o campo nao seja spawnado mais de uma vez
    
    private GameObject _currentFieldGameObject;
    private GameObject _nextFieldGameObject;
    private Field _currentField;
    private Field _nextField;

    private Vector3 offset = new Vector3(0f, 8.78f, 0f);
    
    private void Awake()
    {
        _currentFieldGameObject = Instantiate(fieldPrefab, Vector3.zero, Quaternion.identity);
        _currentField = _currentFieldGameObject.GetComponent<Field>();
    }
    
    private void Update()
    {
        // isso sera movido para uma funçao que verifica se o player fez gol
        if (!canSpawnField || _currentField.state != Field.States.OnTarget) return;
        ChangeField();
        canSpawnField = false;
    }

    public void ChangeField()
    {
        _nextFieldGameObject = Instantiate(fieldPrefab, _currentFieldGameObject.transform.position + offset, Quaternion.identity);
        _currentField.state = Field.States.Leaving;
        _nextField = _nextFieldGameObject.GetComponent<Field>(); // nao sera executado varias vezes por frame
        _nextField.state = Field.States.Reaching;
        Destroy(_currentFieldGameObject, 5f);
        _currentFieldGameObject = _nextFieldGameObject;
        _currentField = _nextField;
    }
}
