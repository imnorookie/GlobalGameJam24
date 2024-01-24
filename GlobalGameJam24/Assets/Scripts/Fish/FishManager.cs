using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static FishManager Instance;

    [Header("Fish Prefabs")]
    public FishController FishRegularPrefabs;

    [Header("Fish Spawn Settings")]
    public float SpawnInitialDelay;
    public float SpawnInterval = 1f;
    public float SpawnIntervalRandomRange = 0.5f;
    public Vector3 SpawnPositionLeft = new Vector3(-10f, 0f, 0f);
    public Vector3 SpawnPositionRight = new Vector3(10f, 0f, 0f);
	public Vector3 SurfacePositionLeft = new Vector3(-10, 0, 0);
	public Vector3 SurfacePositionRight = new Vector3(10, 0, 0);


	protected float _spawnTime;
    protected bool _isLeft;
    protected List<FishController> _fishesRegular = new List<FishController>(); // object pool for regular fish


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

		_spawnTime = Time.time + SpawnInitialDelay;

		for (int i = 0; i < 16; i++)
        {
			var fish = Instantiate(FishRegularPrefabs);
			fish.gameObject.SetActive(false);
			_fishesRegular.Add(fish);
		}
	}

	private void Update()
	{
		if (Time.time <= _spawnTime)
			return;

		PoolFishRegular();
		_spawnTime = Time.time + SpawnInterval + Random.Range(-SpawnIntervalRandomRange, SpawnIntervalRandomRange);
	}

	/// <summary>
	/// Pools a fish from the object pool.
	/// </summary>
	public void PoolFishRegular()
    {
        var fish = _fishesRegular.FirstOrDefault();
        if (fish == null)
			return;
		_fishesRegular.RemoveAt(0);

		fish.gameObject.SetActive(true);
		_isLeft = Random.Range(0, 2) == 0;
		fish.transform.position = _isLeft ? SpawnPositionLeft : SpawnPositionRight;
		fish.Initialize(_isLeft);
	}

	/// <summary>
	/// Recycle the fish back into the pool.
	/// </summary>
	/// <param name="fish"></param>
    public void RecycleFish(FishController fish)
    {
        fish.gameObject.SetActive(false);
		if (fish.FishType == FishController.FishTypeEnum.Regular)
        {
			_fishesRegular.Add(fish);
		}
	}

	private void OnDrawGizmosSelected()
	{
		float size = .2f;
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(SpawnPositionLeft, size);
		Gizmos.DrawSphere(SpawnPositionRight, size);
		Gizmos.DrawSphere(SurfacePositionLeft, size);
		Gizmos.DrawSphere(SurfacePositionRight, size);
	}
}
