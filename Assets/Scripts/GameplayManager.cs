using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private int _enemyAmount;

    [SerializeField]
    private Transform _spawnPointsParent;

    [SerializeField]
    private List<EnemyCharacter> _enemyCharacterPrefabss;

    private EnemyCharacter _activeEnemy;

    [SerializeField]
    private GameOverScreen _gameOverScreen;

    void Start()
    {
        StartCoroutine(EnemySpawnerCoroutine());
    }

    void Update()
    {
        if(MainCharacter.Instance == null)
        {
            _gameOverScreen.GetComponent<CanvasGroup>().alpha = 1.0f;
            StartCoroutine(OnGameOver());
        }
    }

    private void Reload()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private IEnumerator OnGameOver()
    {
        yield return new WaitForSeconds(5f);
        Reload();
    }

    private IEnumerator EnemySpawnerCoroutine()
    {
        int spawnedEnemyCount = 0;

        while(spawnedEnemyCount < _enemyAmount)
        {
            if(_activeEnemy == null)
            {
                yield return new WaitForSeconds(0.8f);
                SpawnEnemy();
                spawnedEnemyCount++;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void SpawnEnemy()
    {
        if(_spawnPointsParent.childCount == 0)
        {
            Debug.Log("No spawn point left can't spawn");
            return;
        }

        var spawnPoint = _spawnPointsParent.GetChild(0);

        var enemyIndex = Random.Range(0, _enemyCharacterPrefabss.Count);
        var enemyPrefab = _enemyCharacterPrefabss[enemyIndex];
        var enemy = GameObject.Instantiate(enemyPrefab.gameObject).GetComponent<EnemyCharacter>();

        enemy.transform.position = spawnPoint.position;

        Destroy(spawnPoint.gameObject);

        _activeEnemy = enemy;
    }
}
