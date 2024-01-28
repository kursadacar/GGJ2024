using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterMouthController : MonoBehaviour
{
    public enum ExpressionType
    {
        Frown,
        Smile1,
        Smile2,
        TalkHappy1,
        TalkHappy2,
        TalkHappy3,
        TalkHappy4,
        TalkHappy5,
        TalkUpset,
        TalkUpset2,
    }

    [Serializable]
    public class ExpressionInfo
    {
        public ExpressionType Type;
        public List<Sprite> Sprites = new List<Sprite>();
        public float UpdateInterval;
    }

    public List<ExpressionInfo> Expressions = new List<ExpressionInfo>();
    public ExpressionInfo DefaultExpression;

    private ExpressionInfo _currentExpression;
    private float _expressionTimer;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if(DefaultExpression == null)
        {
            DefaultExpression = GetExpression(ExpressionType.Smile1);
        }

        _currentExpression = DefaultExpression;
    }
    void Start()
    {
        
    }

    void Update()
    {
        UpdateExpression(Time.deltaTime);
    }

    public void StartExpression(ExpressionType info)
    {
        _currentExpression = GetExpression(info);
        if(_currentExpression != null && _currentExpression.Sprites.Count > 0)
        {
            _spriteRenderer.sprite = _currentExpression.Sprites[0];
        }
    }

    public void StopExpression(ExpressionType idleExpression = ExpressionType.Smile1)
    {
        _currentExpression = GetExpression(idleExpression);
    }

    private void UpdateExpression(float dt)
    {
        if(_currentExpression != null && _currentExpression.Sprites.Count > 0 && _currentExpression.UpdateInterval > 0)
        {
            if (_expressionTimer > _currentExpression.UpdateInterval)
            {
                int currentSpriteIndex = 0;
                if (_spriteRenderer.sprite != null)
                {
                    currentSpriteIndex = _currentExpression.Sprites.IndexOf(_spriteRenderer.sprite);
                    currentSpriteIndex = (currentSpriteIndex + 1) % _currentExpression.Sprites.Count;
                }

                _spriteRenderer.sprite = _currentExpression.Sprites[currentSpriteIndex];

                _expressionTimer = 0f;
            }

            _expressionTimer += dt;
        }
    }

    private ExpressionInfo GetExpression(ExpressionType type)
    {
        for (int i = 0; i < Expressions.Count; i++)
        {
            if (Expressions[i].Type == type)
            {
                return Expressions[i];
            }
        }

        return null;
    }
}
