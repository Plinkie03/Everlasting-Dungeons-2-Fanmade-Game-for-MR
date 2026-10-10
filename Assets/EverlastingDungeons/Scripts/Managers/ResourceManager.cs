using Assets.EverlastingDungeons.Scripts.ScriptableObjects;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Managers
{
    public class ResourceHolder<T> where T : BaseSO<T>
    {
        private T[] raw;
        private Dictionary<string, T> elements;
        public void Load()
        {
            raw = Resources.LoadAll<T>("");
            elements = raw.ToDictionary(x => x.uuid);
            Debug.Log($"Loaded {raw.Length} resources of type {typeof(T).Name}");
        }

        public T Get(string uuid)
        {
            if (elements.TryGetValue(uuid, out var element))
            {
                return element;
            }

            Debug.LogError($"Resource with uuid {uuid} not found");
            return null;
        }

        public T Random => raw[UnityEngine.Random.Range(0, raw.Length)];
        public T[] Raw => raw;
    }

    public class ResourceManager : BaseManager<ResourceManager>
    {
        public readonly ResourceHolder<BaseItemSO> items = new();
        public readonly ResourceHolder<DungeonSpritesSO> dungeonSprites = new();
        public readonly ResourceHolder<SpellSO> spells = new();
        public readonly ResourceHolder<DungeonGenerationOptionsSO> dungeonGenerationOptions = new();
        public readonly ResourceHolder<MonsterSO> monsters = new();

        public override void Init()
        {
            dungeonGenerationOptions.Load();
            monsters.Load();
            spells.Load();
            dungeonSprites.Load();
            items.Load();
        }
    }
}
