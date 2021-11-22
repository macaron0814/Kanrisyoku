using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScore : MonoBehaviour
{
    [SerializeField] private Text timeText;
    [SerializeField] private Text avoidText;
    [SerializeField] private Text jumpText;
    [SerializeField] private Text damageText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text hpText;

    public struct B_Point
    {
        public float time;
        public int   avoid;
        public int   jump;
        public int   damage;
        public bool  penalty;
        public int   score;
        public int   bossHp;
    }

    // 構造体のリストの定義
    public static B_Point b_point;

    // Start is called before the first frame update
    void Start()
    {
        b_point.time = 0;
        b_point.avoid = 0;
        b_point.jump = 0;
        b_point.damage = 0;
        b_point.penalty = false;
        b_point.score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        //条件を満たす場合はスコアが0になる
        if (b_point.time < 40.0f || b_point.avoid > 100) b_point.avoid = 100;
        if (b_point.penalty || b_point.time  > 1000)     b_point.time  = 1000;

        //各スコアの計算
        b_point.time   = (1000000 - (int)(b_point.time * 1000.0f));
        b_point.avoid  = (1000000 - (int)(b_point.avoid * 10000.0f));
        b_point.jump   = ((int)(b_point.jump * 10000.0f));
        b_point.damage = ((int)(b_point.damage * 20000.0f));

        //UIに反映
        timeText.text   = b_point.time.ToString();
        avoidText.text  = b_point.avoid.ToString();
        jumpText.text   = b_point.jump.ToString();
        damageText.text = b_point.damage.ToString();

        //合計スコアを算出
        scoreText.text = "Score " + (b_point.time + b_point.avoid + b_point.jump + b_point.damage).ToString();

        b_point.bossHp = (int)((100 / b_point.bossHp) * Boss.hp);
        hpText.text = b_point.bossHp.ToString();
    }
}
