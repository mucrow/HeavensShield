using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class EnemyWaves: MonoBehaviour {
    EnemyWave[] _data;

    public int Length => _data.Length;
    public EnemyWave this[int index] => _data[index];

    void Awake() {
      int numWaves = transform.childCount;
      _data = new EnemyWave[numWaves];
      for (int i = 0; i < transform.childCount; ++i) {
        _data[i] = transform.GetChild(i).GetComponent<EnemyWave>();
      }
    }
  }
}
