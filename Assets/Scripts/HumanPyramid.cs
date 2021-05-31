using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HumanPyramid : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform _transform;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private GameObject pref_Human;
    [SerializeField] private int maxColumnCount = 15;
    [SerializeField] private Vector2 moveSpeed;
    [Header("Values")]
    [Range(0,10)][SerializeField] private float maxXPosition = 5;
    [SerializeField] private float horizontalDistance = 2F;
    [SerializeField] private float verticalDistance = 1F;
    [HideInInspector] public Vector3 centerPosition;
    [HideInInspector] public int totalBottomElementCount;

    [HideInInspector] public List<Pyramid> humanPyramid = new List<Pyramid>();
    private TouchControl touchControl = new TouchControl();
    private List<Human> humansInThePyramid = new List<Human>();

    public void InitializePyramid()
    {
        humanPyramid = new List<Pyramid>();

        for(int i = 0; i < maxColumnCount; i++)
        {
            humanPyramid.Add(new Pyramid());
            for(int j = 0; j < i + 1; j++)
            {
                humanPyramid[i].humans.Add(null);
            }
        }

        Human firstHuman = Instantiate(pref_Human, _transform.position, Quaternion.identity, _transform).GetComponent<Human>();
        AddHumanInToThePyramid(firstHuman);
    }
    public void UpdatePyramid()
    {
        UpdateCenterPosition();
        FillEmptySlotVertical();
        FillEmptySlotHorizontal();
        ControlHumanCount();
        touchControl.UpdateTouchSystem();
    }
    public void FixedUpdatePyramid()
    {
        Vector3 moveDirection = new Vector3(touchControl.joyStick.x * moveSpeed.x, 0, moveSpeed.y);
        Vector3 movePosition = _transform.position + (_transform.TransformDirection(moveDirection) * Time.fixedDeltaTime);
        movePosition.x = Mathf.Clamp(movePosition.x, -maxXPosition + (horizontalDistance * totalBottomElementCount), maxXPosition);
        _rigidBody.MovePosition(movePosition);
    }
    void FillEmptySlotVertical()
    {
        for(int i = 0; i < humanPyramid.Count; i++)
        {
            for(int j = 0; j < humanPyramid[i].humans.Count; j++)
            {
                int columnNo = i;
                int rowNo = j;

                Human currentHuman = humanPyramid[columnNo].humans[rowNo];

                if(currentHuman == null)
                {
                    if(TryFillTheSlot(columnNo, rowNo))
                    {
                        return;
                    }
                }
                else
                {
                    ArrangePosition(currentHuman, columnNo, rowNo);
                }
            }
        }
    }

    void FillEmptySlotHorizontal()
    {
        if(humanPyramid[0].humans[0] == null)
        {
            for(int i = 0; i < humanPyramid.Count; i++)
            {
                for(int j = 0; j < humanPyramid[i].humans.Count; j++)
                {
                    int columnNo = i;
                    int rowNo = j;

                    Debug.Log(" RR " + columnNo);

                    Human currentHuman = humanPyramid[columnNo].humans[rowNo];

                    if(currentHuman != null)
                    {
                        humanPyramid[columnNo - 1].humans[rowNo] = currentHuman;
                        humanPyramid[columnNo].humans[rowNo] = null;
                    }
                }
            }
        }
    }

    bool TryFillTheSlot(int columnNo, int rowNo)
    {
        if(IsTopRightSlotFull(columnNo, rowNo))
        {
            humanPyramid[columnNo].humans[rowNo] = humanPyramid[columnNo].humans[rowNo + 1];
            humanPyramid[columnNo].humans[rowNo + 1] = null;
            return true;
        }
        else if(IsTopLeftSlotFull(columnNo, rowNo))
        {
            humanPyramid[columnNo].humans[rowNo] = humanPyramid[columnNo + 1].humans[rowNo + 1];
            humanPyramid[columnNo + 1].humans[rowNo + 1] = null;
            return true;
        }
        return false;
    }
    void ArrangePosition(Human currentHuman, int columnNo, int rowNo)
    {
        float positionX = rowNo * horizontalDistance * 0.5f - columnNo * horizontalDistance;
        float positionY = rowNo * verticalDistance;
        currentHuman._transform.localPosition = Vector3.Lerp(currentHuman._transform.localPosition, new Vector3(positionX, positionY, 0), 5 * Time.deltaTime);
        currentHuman._transform.localEulerAngles = Vector3.Lerp(currentHuman._transform.localEulerAngles, new Vector3(0,0, 0), 5 * Time.deltaTime);
    }
    bool IsTopRightSlotFull(int columnNo, int rowNo)
    {
        if(rowNo + 1 < humanPyramid[columnNo].humans.Count)
        {
            if(humanPyramid[columnNo].humans[rowNo + 1])
            {
                return true;
            }
        }

        return false;
    }
    bool IsTopLeftSlotFull(int columnNo, int rowNo)
    {
        if(columnNo + 1 < humanPyramid.Count)
        {
            if(rowNo + 1 < humanPyramid[columnNo + 1].humans.Count)
            {
                if(humanPyramid[columnNo + 1].humans[rowNo + 1])
                {
                    return true;
                }
            }
        }
        return false;
    }
    void AddHumanInToThePyramid(Human human)
    {
        for(int i = 0; i < humanPyramid.Count; i++)
        {
            for(int j = 0; j < humanPyramid[i].humans.Count; j++)
            {
                if(humanPyramid[i].humans[j] == null)
                {
                    if(human)
                    {
                        human._transform.SetParent(_transform);
                        human.gameObject.tag = "HumanPyramid";
                        human.Jump();
                        humansInThePyramid.Add(human);
                    }
                    humanPyramid[i].humans[j] = human;
                    return;
                }
            }
        }
    }
    private void UpdateCenterPosition()
    {
        totalBottomElementCount = 0;

        for(int i = humanPyramid.Count - 1; i > 0; i--)
        {
            if(humanPyramid[i].humans[0] != null)
            {
                totalBottomElementCount = i;
                break;
            }
        }

        centerPosition = _transform.TransformPoint(new Vector3(-horizontalDistance * totalBottomElementCount * 0.5F, 0, 0));
    }
    void PlaceHumansToTheFinishPoint()
    {
        StartCoroutine(IE_PlaceHumansToTheFinishPoint());
    }
    IEnumerator IE_PlaceHumansToTheFinishPoint()
    {
        humansInThePyramid = humansInThePyramid.OrderByDescending(x => x._transform.position.y).ToList();

        Transform finishPointParent = GameManager.Instance.finishPointTransform;

        for(int i = 0; i < finishPointParent.childCount; i++)
        {
            for(int j = 0; j <= i; j++)
            {
                if(humansInThePyramid.Count > 0)
                {
                    humansInThePyramid[0].Jump();
                    float distanceBetween = 1 / (float)(i + 1);
                    float jumpPositionX = -0.5f + (j * distanceBetween) + (distanceBetween / 2);
                    Vector3 jumpPosition = finishPointParent.GetChild(i).TransformPoint(new Vector3(jumpPositionX, 0, 0));
                    StartCoroutine(IE_FinishJumpToThePosition(humansInThePyramid[0], jumpPosition));
                    humansInThePyramid.RemoveAt(0);
                    yield return new WaitForSeconds(0.2F);
                }
            }
        }

        GameManager.Instance.OnPlacingHumansCompleted();
    }
    IEnumerator IE_FinishJumpToThePosition(Human human, Vector3 position)
    {
        float timer = 0;
        float duration = 1;
        var wait = new WaitForEndOfFrame();
        float jumpHeight = 5;
        Vector3 startPos = human._transform.position;
        Vector3 startRot = human._transform.localEulerAngles;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            float lerpVal = timer / duration;
            Vector3 jumpFactor = new Vector3(0, Mathf.PingPong(2 * lerpVal, 1) * jumpHeight, 0);
            human._transform.position = Vector3.Lerp(startPos, position, lerpVal) + jumpFactor;
            human._transform.localEulerAngles = Vector3.Lerp(startRot, new Vector3(0,180,0), lerpVal);
            yield return wait;
        }
    }
    public void OnObstacleHitHuman(Human human)
    {
        humansInThePyramid.Remove(human);
        Destroy(human.gameObject);
    }

    void ControlHumanCount()
    {
        if(GameManager.Instance.GetCurrentState == GameManager.State.Playing)
        {
            if(humansInThePyramid.Count == 0)
            {
                GameManager.Instance.OnLostAllHumans();
            }
        }        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Human"))
        {
            Human touchedHuman = other.GetComponent<Human>();
            AddHumanInToThePyramid(touchedHuman);
        }
        if(other.CompareTag("FinishPoint"))
        {
            if(GameManager.Instance.GetCurrentState == GameManager.State.Playing)
            {
                GameManager.Instance.OnStartedPlacingHumans();
                PlaceHumansToTheFinishPoint();
            }
        }
    }
}


[System.Serializable]
public class Pyramid
{
    public List<Human> humans = new List<Human>();
}
