using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Bixos
{
    Capivara,
    Mico,
    Onca,
    Tatu
}

public class SpriteRotation : MonoBehaviour
{
    private int enumCount;

    private static Bixos _player;
    private static Bixos _enemy;
    
    [SerializeField] private Sprite[] enemySprites;
    public static Sprite enemySprite;
    public static Bixos Player => _player;

    private void Awake()
    {
        enumCount = Enum.GetNames(typeof(Bixos)).Length;
        
        // gets a random element from Enum Bixos not considering the last one used
        _player = (Bixos)(Random.Range((int)_player + 1, (int)_player + enumCount) % enumCount);
        // gets a random enemy from Enum Bixos not considering the chosen for the player
        _enemy = (Bixos)(Random.Range((int)_player + 1, (int)_player + enumCount) % enumCount);
        enemySprite = enemySprites[(int)_enemy];
    }
    
    // Called when a goal has been spawned so the next enemies are different
    public void NextEnemy()
    {
        _enemy = (Bixos)(((int)_enemy + 1) % enumCount);
        if (_enemy == _player)
            _enemy = (Bixos)(((int)_enemy + 1) % enumCount);
        enemySprite = enemySprites[(int)_enemy];
    }
}

