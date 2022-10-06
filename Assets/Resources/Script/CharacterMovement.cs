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
    /// �W�����v�A�j���[�V�����X�^�[�g
    /// </summary>
    void StartVerticalMove()
    {
        character.SetBool("vertMove", true);
    }
    /// <summary>
    /// ���n�̊m�F
    /// </summary>
    void LandingOver()
    {
        character.SetBool("isLanding", false);
        animOver = true;
    }

 
    /// <summary>
    /// �G�_���[�W�󂯂�
    /// </summary>
    void EnemyTakeDamage()
    {
        enemy.TakeDamage();
    }
    #endregion

    #region public function

    /// <summary>
    /// �w�肵���ꏊ�ɃW�����v
    /// </summary>
    /// <param name="targetPos">�ڎ��ʒu</param>
    public void JumpTo(Transform targetPos)
    {
        character.SetTrigger("jump");
        movePos = targetPos;
    }
    /// <summary>
    /// ���S��Ԋm�F
    /// </summary>
    /// <returns>Animator_bool_dead</returns>
    public bool GetDead()
    {
        return character.GetBool("dead");
    }
    /// <summary>
    /// �����A�j���[�V����
    /// </summary>
    public void MoveHorizontal()
    {
        character.SetBool("walking", true);
    }
    /// <summary>
    /// �U���A�j���[�V����
    /// </summary>
    public async void Attack()
    {
        animOver = false;
        character.SetTrigger("attack");
        await UniTask.WaitUntil(WaitAnimation);
        animOver = true;
    }
    /// <summary>
    /// �o�g���̌��ʂ̌v�Z
    /// </summary>
    /// <returns>���s����</returns>
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
    /// //�_���[�W�A�j���[�V�������Đ�
    /// </summary>
    public async void TakeDamage()
    {
        character.SetBool("dead", true);
        await UniTask.WaitUntil(WaitAnimation);
        animOver = true;
        isDead = true;
    }
    /// <summary>
    /// Animator�̃A�j���[�V�������I��邩�ǂ����m�F
    /// </summary>
    /// <returns>�A�j���[�V�����̏��</returns>
    public bool WaitAnimation()
    {
        return character.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
    }


    /// <summary>
    /// �QD�L�����̌����ύX
    /// </summary>
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
