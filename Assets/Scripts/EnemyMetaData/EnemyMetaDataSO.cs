using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyMetaDataSO : ScriptableObject
{

    public List<EnemyMetaData> enemyMetaData;
    [HideInInspector]
    public List<string> keys = new List<string>();

    public List<EnemyMetaData> GetEnemysByFirstLvl (int firstLvl)
    {
        IEnumerable<EnemyMetaData> query =
                    from enemy in enemyMetaData
                    where enemy.firstLvl == firstLvl
                    select enemy;

        return query.ToList<EnemyMetaData>();
    }

    public List<EnemyMetaData> GetEnemysByFirstLvlRange (int start, int end)
    {
        IEnumerable<EnemyMetaData> query =
                    from enemy in enemyMetaData
                    where enemy.firstLvl >= start && enemy.firstLvl <= end
                    select enemy;

        return query.ToList<EnemyMetaData>();
    }
}
