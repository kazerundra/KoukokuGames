using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    #region private field
    private Animator character;
    [SerializeField] private float speed = 5f;
    #endregion

    #region public field
    public Tower currentTower;
    public CharStat charStat;
    public Transform movePos;
    public CharacterMovement enemy;
    public bool animOver = true;
    #endregion

    #region UnityCallBack
    void Awake()
    {
        character = GetComponent<Animator>();
        charStat = GetComponent<CharStat>();
    }
    void Update()
    {
        if (character.GetBool("vertMove"))
        {
            var step = speed * Time.deltaTime;
            Debug.Log(movePos);
            transform.position = Vector3.MoveTowards(transform.position, movePos.position, step);
            if (Vector3.Distance(transform.position, movePos.position) < 0.001f)
            {

                character.SetBool("vertMove", false);
                character.SetBool("isLanding", true);
            }
        }

        if (character.GetBool("walking"))
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, movePos.position, step);
            if (Vector3.Distance(transform.position, movePos.position) < 0.001f)
            {
                character.SetBool("walking", false);
            }
        }
    }
    #endregion

    #region private function
    //ジャンプアニメーションスタート
    void StartVerticalMove()
    {
        character.SetBool("vertMove", true);
    }
    //着地の確認
    void LandingOver()
    {
        character.SetBool("isLanding", false);
        animOver = true;
    }
    //アニメーション終了の呼び出す
    void AnimOver()
    {
        animOver = true;
    }
    //敵ダメージ受ける
    void EnemyTakeDamage()
    {
        enemy.TakeDamage();
    }
    #endregion

    #region public function
    //指定した場所にジャンプ
    public void JumpTo(Transform targetPos)
    {
        character.SetTrigger("jump");
        movePos = targetPos;
    }
    //死亡状態確認
    public bool GetDead()
    {
        return character.GetBool("dead");
    }
    //歩きアニメーション
    public void MoveHorizontal()
    {
        character.SetBool("walking", true);
    }
    //攻撃アニメーション
    public void Attack()
    {
        animOver = false;
        character.SetTrigger("attack");
    }
    //バトルの結果の計算
    public bool WinBattle()
    {
        if (GetComponent<CharStat>().power > enemy.GetComponent<CharStat>().power)
        {
            Attack();
            return true;
        }
        else
        {
            enemy.Attack();
            return false;
        }
    }
    //ダメージアニメーションを再生
    public void TakeDamage()
    {
        character.SetBool("dead", true);
    }

    //２Dキャラの向き変更
    public void FlipChar()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        Transform charStat = GetComponent<CharStat>().powerText.transform;
        charStat.transform.localScale = new Vector3(-1, 1, 1);
    }
    /// <summary>
    /// 現在のタワーを設定する
    /// </summary>
    /// <param name="tower">タワー</param>
    /// <param name="player">プレイヤー</param>
    public void AssignTower(Tower tower, CharacterMovement player)
    {
        currentTower = tower;
        enemy = tower.enemy;
        enemy.enemy = player;
    }
    #endregion


}
