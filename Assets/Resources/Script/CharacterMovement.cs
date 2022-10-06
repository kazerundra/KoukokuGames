using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

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
    public bool isDead = false;
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
    /// <summary>
    /// ジャンプアニメーションスタート
    /// </summary>
    void StartVerticalMove()
    {
        character.SetBool("vertMove", true);
    }
    /// <summary>
    /// 着地の確認
    /// </summary>
    void LandingOver()
    {
        character.SetBool("isLanding", false);
        animOver = true;
    }

 
    /// <summary>
    /// 敵ダメージ受ける
    /// </summary>
    void EnemyTakeDamage()
    {
        enemy.TakeDamage();
    }
    #endregion

    #region public function

    /// <summary>
    /// 指定した場所にジャンプ
    /// </summary>
    /// <param name="targetPos">目次位置</param>
    public void JumpTo(Transform targetPos)
    {
        character.SetTrigger("jump");
        movePos = targetPos;
    }
    /// <summary>
    /// 死亡状態確認
    /// </summary>
    /// <returns>Animator_bool_dead</returns>
    public bool GetDead()
    {
        return character.GetBool("dead");
    }
    /// <summary>
    /// 歩きアニメーション
    /// </summary>
    public void MoveHorizontal()
    {
        character.SetBool("walking", true);
    }
    /// <summary>
    /// 攻撃アニメーション
    /// </summary>
    public async void Attack()
    {
        animOver = false;
        character.SetTrigger("attack");
        await UniTask.WaitUntil(WaitAnimation);
        animOver = true;
    }
    /// <summary>
    /// バトルの結果の計算
    /// </summary>
    /// <returns>勝敗結果</returns>
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
    /// <summary>
    /// //ダメージアニメーションを再生
    /// </summary>
    public async void TakeDamage()
    {
        character.SetBool("dead", true);
        await UniTask.WaitUntil(WaitAnimation);
        animOver = true;
        isDead = true;
    }
    /// <summary>
    /// Animatorのアニメーションが終わるかどうか確認
    /// </summary>
    /// <returns>アニメーションの状態</returns>
    public bool WaitAnimation()
    {
        return character.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
    }


    /// <summary>
    /// ２Dキャラの向き変更
    /// </summary>
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
