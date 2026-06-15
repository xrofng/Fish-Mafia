using UnityEngine;

public class BulletMana : MonoBehaviour
{
    public int Min = 0;
    public int Max = 60;
    private int _prevMana;
    [SerializeField]
    private int _current;

    public int Current => _current;

    public bool HasEnough(int cost)
    {
        return _current >= cost;
    }

    public void Add(int amount)
    {
        if(amount > 0)
        {
            Set(_current + amount);
        }
        // Debug.Log($"Mana +{amount} → {_current}");
    }

    public bool Consume(int cost = 10)
    {
        if (_current < cost)
            return false;

        Set(_current - cost);
        // Debug.Log($"Mana -{cost} → {_current}");
        return true;
    }

    public void Set(int value)
    {
        _prevMana = _current;
        _current = Mathf.Clamp(value, Min, Max);
        EventBus.TriggerEvent(new EvsBulletManaChanged(_prevMana, _current));
    }

    public struct EvsBulletManaChanged
    {
        public int PrevMana;
        public int NewMana;

        public EvsBulletManaChanged(int prevMana, int newMana)
        {
            PrevMana = prevMana;
            NewMana = newMana;
        }
    }
}