using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    #region private field
    private List<List<Tower>> towerList = new List<List<Tower>>();
    [SerializeField] private int currentTowerPos = 0;
    List<Tower> currentTowerList;
    Tower currentTower;
    int currentMaxNumber;
    #endregion

    #region public field
    public CharacterMovement player, target;
    public List<Tower> tower1, tower2, tower3;
    public GameObject GameOverScreen;
    public bool isGameOVer = false;
    public Camera mainCamera;
    public TextMeshProUGUI clearText;
    #endregion

    #region private function
    /// <summary>
    /// ゲームクリア条件を満たすの確認
    /// </summary>
    private void CheckGameClear()
    {
        if (CheckTowerClear(towerList.Count - 1))
        {
            GameOver("Game Clear");
        };
    }
    /// <summary>
    /// ランダムリストを作る
    /// </summary>
    /// <param name="listCount">作りたいリストの長さ/param>
    /// <returns></returns>
    private List<int> CreateRandomNumber(int listCount)
    {
        List<int> randomNumberList = new List<int>();
        for (int j = 0; j < listCount; j++)
        {
            int randomNumber = Random.Range(currentMaxNumber / 2, currentMaxNumber);
            //Debug.Log("max=" + currentMaxNumber + "random list=" + randomNumber);
            currentMaxNumber += randomNumber;
            randomNumberList.Add(randomNumber);
        }

        return randomNumberList;
    }

    /// <summary>
    /// リストデータをシャッフル
    /// </summary>
    /// <param name="randomNumberList"></param>
    /// <returns></returns>
    private List<int> ShuffleList(List<int> randomNumberList)
    {
        for (int k = 0; k < randomNumberList.Count; k++)
        {
            int temp = randomNumberList[k];
            int randomIndex = Random.Range(k, randomNumberList.Count);
            randomNumberList[k] = randomNumberList[randomIndex];
            randomNumberList[randomIndex] = temp;
        }

        return randomNumberList;
    }
    #endregion

    #region public function
    /// <summary>
    /// 全タワーの番号をランダマイズ
    /// </summary>
    public void RandomizeTowerNumber()
    {
        currentMaxNumber = player.charStat.power;
        for (int i = 0; i < towerList.Count; i++)
        {
            List<Tower> currentList = towerList[i];
            List<int> randomNumberList = CreateRandomNumber(currentList.Count);

            randomNumberList = ShuffleList(randomNumberList);
            //敵キャラの番号を変更
            for (int l = 0; l < randomNumberList.Count; l++)
            {
                currentList[l].enemy.GetComponent<CharStat>().ChangePowerText(randomNumberList[l]);

            }

        }
    }



    /// <summary>
    ///  次のタワーに行けるかどうか確認
    /// </summary>
    /// <param name="towerPos">タワー番号</param>
    /// <returns></returns>
    public bool CheckTowerClear(int towerPos)
    {
        int clearNumber = towerList[towerPos].Count;
        int count = 0;
        foreach (Tower tower in towerList[towerPos])
        {
            if (tower.clear)
            {
                count++;
            }
        }
        if (count >= clearNumber)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 勝った時
    /// </summary>
    public void WinBattle()
    {
        currentTower.clear = true;
        player.charStat.ChangePowerText(player.charStat.power + player.enemy.charStat.power);
        if (CheckTowerClear(currentTowerPos))
        {
            currentTowerPos++;
            CheckGameClear();
        }

    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    /// <param name="gameoverText">ゲームオーバーのテキスト</param>
    public void GameOver(string gameoverText)
    {
        isGameOVer = true;
        clearText.text = gameoverText;
        GameOverScreen.SetActive(true);
    }

    #endregion

    #region coroutine
    /// <summary>
    /// プレイヤーと敵のアニメーションが両方終わるまで待つ
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAnimation()
    {
        while (!player.animOver)
        {
            yield return new WaitForEndOfFrame();
        }
        player.animOver = false;
        player.enemy.animOver = false;
        if (player.WinBattle())
        {
            while (!player.animOver)
            {
                yield return new WaitForEndOfFrame();
            }
            WinBattle();
        }
        else
        {
            while (!player.enemy.animOver)
            {
                yield return new WaitForEndOfFrame();
            }
            GameOver("GameOver");
        }
    }
    #endregion

    #region Unity Callback
    private void Start()
    {
        towerList.Add(tower1);
        towerList.Add(tower2);
        towerList.Add(tower3);
        currentTowerList = new List<Tower>();
        RandomizeTowerNumber();
    }

    void Update()
    {
        mainCamera.transform.position = new Vector3(player.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
        }
        if (Input.GetMouseButton(0))
        {
            if (player.GetDead() || isGameOVer || !player.animOver)
            {
                return;
            }
          
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Tower")
                {
                   
                    Tower targetTower = hit.collider.gameObject.GetComponent<Tower>();
                    if (currentTowerPos != targetTower.towerNumber)
                    {
                        return;
                    }
                    target = targetTower.enemy;
                    player.movePos = hit.collider.gameObject.GetComponent<Tower>().stopPos;
                    currentTower = player.GetComponent<CharacterMovement>().currentTower;
                    
                    if ( currentTower!= targetTower)
                    {
                        currentTowerList.Clear();
                        player.GetComponent<CharacterMovement>().AssignTower(targetTower,player);
                        foreach (Tower tower in towerList[currentTowerPos])
                        {
                            currentTowerList.Add(tower);
                        }
                        currentTower = targetTower;
                        player.animOver = false;
                        player.JumpTo (targetTower.stopPos);
                        StartCoroutine(WaitAnimation());
                    }
                }
            }
        
        }
    }
    #endregion
}

