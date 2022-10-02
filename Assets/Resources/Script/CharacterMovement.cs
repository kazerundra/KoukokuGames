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
    //�W�����v�A�j���[�V�����X�^�[�g
    void StartVerticalMove()
    {
        character.SetBool("vertMove", true);
    }
    //���n�̊m�F
    void LandingOver()
    {
        character.SetBool("isLanding", false);
        animOver = true;
    }
    //�A�j���[�V�����I���̌Ăяo��
    void AnimOver()
    {
        animOver = true;
    }
    //�G�_���[�W�󂯂�
    void EnemyTakeDamage()
    {
        enemy.TakeDamage();
    }
    #endregion

    #region public function
    //�w�肵���ꏊ�ɃW�����v
    public void JumpTo(Transform targetPos)
    {
        character.SetTrigger("jump");
        movePos = targetPos;
    }
    //���S��Ԋm�F
    public bool GetDead()
    {
        return character.GetBool("dead");
    }
    //�����A�j���[�V����
    public void MoveHorizontal()
    {
        character.SetBool("walking", true);
    }
    //�U���A�j���[�V����
    public void Attack()
    {
        animOver = false;
        character.SetTrigger("attack");
    }
    //�o�g���̌��ʂ̌v�Z
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
    //�_���[�W�A�j���[�V�������Đ�
    public void TakeDamage()
    {
        character.SetBool("dead", true);
    }

    //�QD�L�����̌����ύX
    public void FlipChar()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        Transform charStat = GetComponent<CharStat>().powerText.transform;
        charStat.transform.localScale = new Vector3(-1, 1, 1);
    }
    /// <summary>
    /// ���݂̃^���[��ݒ肷��
    /// </summary>
    /// <param name="tower">�^���[</param>
    /// <param name="player">�v���C���[</param>
    public void AssignTower(Tower tower, CharacterMovement player)
    {
        currentTower = tower;
        enemy = tower.enemy;
        enemy.enemy = player;
    }
    #endregion


}
