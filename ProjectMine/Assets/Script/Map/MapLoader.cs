using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
// using UnityEngine.UIElements;
// using System.Threading;
// using Unity.VisualScripting;

public class MapLoader : MonoBehaviour
{
    // 벽 생성 관리
    public Object[,,] wallArray = new Object[100, 100, 6];

    // 벽 생성 위치
    public GameObject plane;

    // 프리팹 저장할 변수
    public GameObject wall1;
    public GameObject wall2;
    public GameObject hardWall;
    public GameObject ceilBlock;

    public GameObject startMonsterRoom;
    public GameObject monsterRoom;
    public GameObject upgradeRoom;
    public GameObject gemRoom;
    public GameObject bossRoom;
    public GameObject gemRoomObject1;
    public GameObject gemRoomObject2;
    public GameObject upgradeRoomObject;

    public GameObject mobMonster1;
    public GameObject mobMonster2;
    public GameObject upgradeSlotMachine;
    public GameObject gemS;
    public GameObject gemA;
    public GameObject gemB;
    public GameObject bossMonster;
    public GameObject gamePlayer;
    private Transform playerTransform;
    private Transform mobTransform1;
    private Transform mobTransform2;

    const int width = 50;
    const int height = 50;
    int roomX;
    int roomZ;
    int[] roomXArray = new int[4];
    int[] roomZArray = new int[4];
    int roomCreateCount = 0;
    int roomSizeX = 20;
    int roomSizeZ = 20;
    GameObject[] roomArray = new GameObject[4];

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mobTransform1 = GameObject.Find("EnemyGroup_1").GetComponent<Transform>();
        mobTransform2 = GameObject.Find("EnemyGroup_2").GetComponent<Transform>();

        // 방 배열 설정
        roomArray[0] = startMonsterRoom;
        roomArray[1] = monsterRoom;
        roomArray[2] = upgradeRoom;
        roomArray[3] = gemRoom;

        for (int i = 1; i < 99; i++)
        {
            for (int j = 1; j < 99; j++)
            {
                // 벽돌 만들 확률
                if (Random.Range(1, 11) < 8)
                {
                    CreateWall(i, j, 4, wall1);
                }
            }
        }

        // 테두리벽 생성
        CreateOutLine();

        // 우상단 보스룸 빈공간 생성
        BossRoomCreate();

        // 방 4개 생성
        for (int i = 0; i < 4; i++)
        {
            RoomCreate();
        }

        CreateCeil();
    }

    // 보스방 만들기
    void BossRoomCreate()
    {
        // 보스방 공간만들기
        for (int i = 0; i < width/2; i++)
        {
            for (int j = 0; j < height/2; j++)
            {
                if (wallArray[i+width*3/2-1, j+height*3/2-1, 0] != null)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        Destroy(wallArray[i+width * 3 / 2 - 1, j+height * 3 / 2 - 1, k]);
                    }
                }
            }
        }
        // (보스방 앞)업글방 공간만들기
        for (int i = 0; i < roomSizeX; i++)
        {
            for (int j = 0; j < roomSizeZ; j++)
            {
                if (wallArray[i+width + 8, j+height + 4, 0] != null)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        Destroy(wallArray[i+width + 8, j+height + 4, k]);
                    }
                }
            }
        }

        // 구역설정
        Instantiate(bossRoom, new Vector3(plane.gameObject.transform.position.x + width*3/4-0.5f, plane.gameObject.transform.position.y + 2, plane.gameObject.transform.position.z + height*3/4-0.5f), Quaternion.identity);
        Instantiate(upgradeRoom, new Vector3(plane.gameObject.transform.position.x + 8 +roomSizeX/2, plane.gameObject.transform.position.y + 2, plane.gameObject.transform.position.z + 5+roomSizeZ/2-1f), Quaternion.identity);

        // 오브젝트 생성
        //Instantiate(bossMonster, new Vector3(plane.gameObject.transform.position.x + width*3/4-0.5f +9, plane.gameObject.transform.position.y+0.5f, plane.gameObject.transform.position.z + height*3/4-0.5f), Quaternion.identity);
        bossMonster.transform.Translate(new Vector3(-40, plane.gameObject.transform.position.y, -40));

        //Instantiate(upgradeSlotMachine, new Vector3(plane.gameObject.transform.position.x + 8 +roomSizeX/2, plane.gameObject.transform.position.y+0.5f, plane.gameObject.transform.position.z + 5+roomSizeZ/2-1f), Quaternion.identity);
        upgradeSlotMachine.transform.Translate(new Vector3(12, plane.gameObject.transform.position.y, 17));
        upgradeRoomObject.transform.Translate(new Vector3(27, plane.gameObject.transform.position.y, 2));


        // 테두리 벽 생성(보스방)
        for (int i = 0; i < height/2; i++)
        {
            CreateWall(width*3/2-1, height*3/2+i-1, 5, hardWall);
        }
        for (int i = 0; i < width/2 -4; i++)
        {
            CreateWall(width*3/2+3+i, height*3/2-1, 5, hardWall);
        }
        // 테두리 벽 생성(업글방)
        int randomNum = Random.Range(1, 3);
        if (randomNum == 1)
        {
            /// 벽 만들기 case1
            // 벽 만들기(가로 두줄)
            for (int i = 0; i < roomSizeX/4; i++)
            {
                // 위
                CreateWall(i+width + 8, height + 4+roomSizeZ-1, 5, hardWall);
                // 아래
                CreateWall(i+width + 8+roomSizeX/4*3, height + 4, 5, hardWall);
            }
            for (int i = 0; i < roomSizeX - roomSizeX/4; i++)
            {
                // 위
                CreateWall(i+width + 8+roomSizeX/4, height + 4+roomSizeZ-1, 5, wall1);
                // 아래
                CreateWall(i+width + 8, height + 4, 5, wall1);
            }

            // 벽 만들기(세로 두줄)
            for (int i = 1; i < roomSizeZ/4; i++)
            {
                // 왼쪽
                CreateWall(width + 8, height + 4 + i +roomSizeZ/4*3-1, 5, hardWall);
                // 오른쪽
                CreateWall(width + 8+roomSizeX-1, height + 4 + i, 5, hardWall);
            }
            for (int i = 1; i < roomSizeZ - roomSizeZ/4; i++)
            {
                // 왼쪽
                CreateWall(width + 8, height + 4 + i, 5, wall1);
                // 오른쪽
                CreateWall(width + 8+roomSizeX-1, height + 4 + i +roomSizeZ/4-1, 5, wall1);
            }
        }
        else if (randomNum == 2)
        {
            /// 벽 만들기 case2
            // 벽 만들기(가로 두줄)
            for (int i = 0; i < roomSizeX/4; i++)
            {
                // 위
                CreateWall(width + 8+i, height + 4+roomSizeZ-1, 5, hardWall);
                CreateWall(width + 8+i+roomSizeX/2, height + 4+roomSizeZ-1, 5, hardWall);
                // 아래
                CreateWall(width + 8+i, height + 4, 5, hardWall);
                CreateWall(width + 8+i + roomSizeX/4*3, height + 4, 5, hardWall);
            }
            for (int i = 0; i < roomSizeX/4; i++)
            {
                // 위
                CreateWall(width + 8+i+roomSizeX/4, height + 4+roomSizeZ-1, 5, wall1);
                CreateWall(width + 8+i+roomSizeX/4*3, height + 4+roomSizeZ-1, 5, wall1);
                // 아래
                CreateWall(width + 8+i+roomSizeX/4, height + 4, 5, wall1);
                CreateWall(width + 8+i+roomSizeX/2, height + 4, 5, wall1);
            }

            // 벽 만들기(세로 두줄)
            for (int i = 1; i < roomSizeZ/4; i++)
            {
                // 왼쪽
                CreateWall(width + 8, height + 4 + i +roomSizeZ/4*3-1, 5, hardWall);
                CreateWall(width + 8, height + 4 + i, 5, hardWall);
                // 오른쪽
                CreateWall(width + 8+roomSizeX-1, height + 4 + i, 5, hardWall);
            }
            for (int i = 1; i < roomSizeZ/4+1; i++)
            {
                // 왼쪽
                CreateWall(width + 8, height + 4 + i-1+roomSizeX/2, 5, wall1);
                CreateWall(width + 8, height + 4 + i +roomSizeZ/4-1, 5, wall1);
                // 오른쪽            
                CreateWall(width + 8+roomSizeX-1, height + 4 + i -1+roomSizeZ/2, 5, wall1);
                CreateWall(width + 8+roomSizeX-1, height + 4 + i +roomSizeZ/4-1, 5, wall1);
            }
            for (int i = 1; i < roomSizeZ/4; i++)
            {
                CreateWall(width + 8+roomSizeX-1, height + 4 + i +roomSizeZ/4*3-1, 5, wall1);
            }
        }
    }

    // 랜덤 방만들기
    void RoomCreate()
    {
        // 방 좌표 구하기
        RoomXZCreate();

        // 방 구역 설정 
        Instantiate(roomArray[roomCreateCount], new Vector3(plane.gameObject.transform.position.x - width +roomX, plane.gameObject.transform.position.y + 2, plane.gameObject.transform.position.z - height+roomZ), Quaternion.identity);
        // 오브젝트 생성
        if (roomArray[roomCreateCount] == startMonsterRoom)
        {
            mobTransform1.transform.Translate(new Vector3(plane.gameObject.transform.position.x - width*3/2 +roomX-roomSizeX+1, plane.gameObject.transform.position.y+1, plane.gameObject.transform.position.z - height*3/2+roomZ-roomSizeZ-4));
            //playerTransform.transform.Translate(new Vector3(plane.gameObject.transform.position.x - width*3/2 +roomX-roomSizeX+1, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height*3/2+roomZ-roomSizeZ-4));

        }
        else if (roomArray[roomCreateCount] == monsterRoom)
        {
            mobTransform2.transform.Translate(new Vector3(plane.gameObject.transform.position.x - width*3/2 +roomX-roomSizeX+3, plane.gameObject.transform.position.y+1, plane.gameObject.transform.position.z - height*3/2+roomZ-roomSizeZ-5));
        }
        else if (roomArray[roomCreateCount] == upgradeRoom)
        {
            Instantiate(gemS, new Vector3(plane.gameObject.transform.position.x - width +roomX, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height+roomZ+3), Quaternion.identity);
            Instantiate(gemA, new Vector3(plane.gameObject.transform.position.x - width +roomX+7, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height+roomZ+2), Quaternion.identity);
            Instantiate(gemA, new Vector3(plane.gameObject.transform.position.x - width +roomX+1, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height+roomZ-3), Quaternion.identity);
            Instantiate(gemB, new Vector3(plane.gameObject.transform.position.x - width +roomX-3, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height+roomZ+6), Quaternion.identity);
            Instantiate(gemB, new Vector3(plane.gameObject.transform.position.x - width +roomX-6, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height+roomZ-5), Quaternion.identity);
            Instantiate(gemB, new Vector3(plane.gameObject.transform.position.x - width +roomX+7, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height+roomZ+7), Quaternion.identity);
            gemRoomObject1.transform.Translate(new Vector3(plane.gameObject.transform.position.x - width*3/2 +roomX-roomSizeX-2, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height*3/2+roomZ-roomSizeZ+1));
        }
        else if (roomArray[roomCreateCount] == gemRoom)
        {
            Instantiate(gemS, new Vector3(plane.gameObject.transform.position.x - width +roomX, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height+roomZ+3), Quaternion.identity);
            Instantiate(gemA, new Vector3(plane.gameObject.transform.position.x - width +roomX+7, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height+roomZ+2), Quaternion.identity);
            Instantiate(gemA, new Vector3(plane.gameObject.transform.position.x - width +roomX+1, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height+roomZ-3), Quaternion.identity);
            Instantiate(gemB, new Vector3(plane.gameObject.transform.position.x - width +roomX-3, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height+roomZ+6), Quaternion.identity);
            Instantiate(gemB, new Vector3(plane.gameObject.transform.position.x - width +roomX-6, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height+roomZ-5), Quaternion.identity);
            Instantiate(gemB, new Vector3(plane.gameObject.transform.position.x - width +roomX+7, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height+roomZ+7), Quaternion.identity);
            gemRoomObject2.transform.Translate(new Vector3(plane.gameObject.transform.position.x - width*3/2 +roomX-roomSizeX-2, plane.gameObject.transform.position.y, plane.gameObject.transform.position.z - height*3/2+roomZ-roomSizeZ+1));
        }


        roomCreateCount++;
        int randomNum = Random.Range(1, 3);

        // 빈칸만들기
        for (int i = 0; i < roomSizeX; i++)
        {
            for (int j = 0; j < roomSizeZ; j++)
            {
                if (wallArray[roomX+i-roomSizeX/2, roomZ+j-roomSizeZ/2, 0] != null)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        Destroy(wallArray[roomX+i-roomSizeX/2, roomZ+j-roomSizeZ/2, k]);
                    }
                }
            }
        }

        // 블럭만들기 2개 경우의 수
        if (randomNum == 1)
        {
            /// 벽 만들기 case1
            // 벽 만들기(가로 두줄)
            for (int i = 0; i < roomSizeX/4; i++)
            {
                // 위
                CreateWall(roomX+i-roomSizeX/2, roomZ+roomSizeZ/2-1, 4, hardWall);
                // 아래
                CreateWall(roomX+i+roomSizeX/4, roomZ-roomSizeZ/2, 4, hardWall);
            }
            for (int i = 0; i < roomSizeX - roomSizeX/4; i++)
            {
                // 위
                CreateWall(roomX+i-roomSizeX/4, roomZ+roomSizeZ/2-1, 4, wall1);
                // 아래
                CreateWall(roomX+i-roomSizeX/2, roomZ-roomSizeZ/2, 4, wall1);
            }

            // 벽 만들기(세로 두줄)
            for (int i = 1; i < roomSizeZ/4; i++)
            {
                // 왼쪽
                CreateWall(roomX-roomSizeX/2, roomZ + i +roomSizeZ/4-1, 4, hardWall);
                // 오른쪽
                CreateWall(roomX+roomSizeX/2-1, roomZ + i -roomSizeZ/2, 4, hardWall);
            }
            for (int i = 1; i < roomSizeZ - roomSizeZ/4; i++)
            {
                // 왼쪽
                CreateWall(roomX-roomSizeX/2, roomZ + i -roomSizeZ/2, 4, wall1);
                // 오른쪽
                CreateWall(roomX+roomSizeX/2-1, roomZ + i -roomSizeZ/4-1, 4, wall1);
            }
        }
        else if (randomNum == 2)
        {
            /// 벽 만들기 case2
            // 벽 만들기(가로 두줄)
            for (int i = 0; i < roomSizeX/4; i++)
            {
                // 위
                CreateWall(roomX+i-roomSizeX/2, roomZ+roomSizeZ/2-1, 4, hardWall);
                CreateWall(roomX+i, roomZ+roomSizeZ/2-1, 4, hardWall);
                // 아래
                CreateWall(roomX+i-roomSizeX/2, roomZ-roomSizeZ/2, 4, hardWall);
                CreateWall(roomX+i + roomSizeX/4, roomZ-roomSizeZ/2, 4, hardWall);
            }
            for (int i = 0; i < roomSizeX/4; i++)
            {
                // 위
                CreateWall(roomX+i-roomSizeX/4, roomZ+roomSizeZ/2-1, 4, wall1);
                CreateWall(roomX+i+roomSizeX/4, roomZ+roomSizeZ/2-1, 4, wall1);
                // 아래
                CreateWall(roomX+i-roomSizeX/4, roomZ-roomSizeZ/2, 4, wall1);
                CreateWall(roomX+i, roomZ-roomSizeZ/2, 4, wall1);
            }

            // 벽 만들기(세로 두줄)
            for (int i = 1; i < roomSizeZ/4; i++)
            {
                // 왼쪽
                CreateWall(roomX-roomSizeX/2, roomZ + i +roomSizeZ/4-1, 4, hardWall);
                CreateWall(roomX-roomSizeX/2, roomZ + i -roomSizeZ/2, 4, hardWall);
                // 오른쪽
                CreateWall(roomX+roomSizeX/2-1, roomZ + i -roomSizeZ/2, 4, hardWall);
            }
            for (int i = 1; i < roomSizeZ/4+1; i++)
            {
                // 왼쪽
                CreateWall(roomX-roomSizeX/2, roomZ + i-1, 4, wall1);
                CreateWall(roomX-roomSizeX/2, roomZ + i -roomSizeZ/4-1, 4, wall1);
                // 오른쪽            
                CreateWall(roomX+roomSizeX/2-1, roomZ + i -1, 4, wall1);
                CreateWall(roomX+roomSizeX/2-1, roomZ + i -roomSizeZ/4-1, 4, wall1);
            }
            for (int i = 1; i < roomSizeZ/4; i++)
            {
                CreateWall(roomX+roomSizeX/2-1, roomZ + i +roomSizeZ/4-1, 4, wall1);
            }
        }


    }

    // 방 좌표 구하는 함수
    void RoomXZCreate()
    {
        roomX = Random.Range(roomSizeX/2 + 1, 99-roomSizeX/2);
        roomZ = Random.Range(roomSizeZ/2 + 1, 99-roomSizeZ/2);

        if (roomX > width - roomSizeX/2 && roomZ > height - roomSizeZ/2)
        {
            RoomXZCreate();
        }
        if (roomCreateCount>0)
        {
            for (int i = 0; i < roomCreateCount; i++)
            {
                if (roomX<roomXArray[i]+roomSizeX && roomX>roomXArray[i]-roomSizeX)
                {
                    if (roomZ<roomZArray[i]+roomSizeZ && roomZ>roomZArray[i]-roomSizeZ)
                    {
                        RoomXZCreate();
                    }
                }
            }
        }
        roomXArray[roomCreateCount] = roomX;
        roomZArray[roomCreateCount] = roomZ;
    }

    // 벽돌 3개 만드는 기본 함수
    void CreateWall(int x, int z, int n, Object objWall)
    {
        for (int i = 1; i < n+1; i++)
        {
            int randomNum = Random.Range(1, 3);
            if (randomNum == 1 && objWall == wall1)
            {
                wallArray[x, z, i-1] = Instantiate(wall1, new Vector3(plane.gameObject.transform.position.x - width+0.5f +x, plane.gameObject.transform.position.y +i-0.5f, plane.gameObject.transform.position.z - height+0.5f+z), Quaternion.identity);

            }
            else if (randomNum > 1 && objWall == wall1)
            {
                wallArray[x, z, i-1] = Instantiate(wall2, new Vector3(plane.gameObject.transform.position.x - width+0.5f +x, plane.gameObject.transform.position.y +i-0.5f, plane.gameObject.transform.position.z - height+0.5f+z), Quaternion.identity);

            }
            else if (objWall != wall1)
            {
                wallArray[x, z, i-1] = Instantiate(objWall, new Vector3(plane.gameObject.transform.position.x - width+0.5f +x, plane.gameObject.transform.position.y +i-0.5f, plane.gameObject.transform.position.z - height+0.5f+z), Quaternion.identity);

            }
        }
    }

    // 전체맵의 테두리 부분 만들기
    void CreateOutLine()
    {
        for (int i = 0; i < width*2; i++)
        {
            CreateWall(i, 0, 5, hardWall);
            CreateWall(i, width*2-1, 5, hardWall);
        }
        for (int i = 1; i < height*2-1; i++)
        {
            CreateWall(0, i, 5, hardWall);
            CreateWall(width*2-1, i, 5, hardWall);
        }
    }

    // 천장 만들기
    void CreateCeil()
    {
        for (int i = 0; i < width*2; i++)
        {
            for (int j = 0; j < height*2; j++)
            {
                if (i<60 || j<60)
                {
                    wallArray[i, j, 4] = Instantiate(ceilBlock, new Vector3(plane.gameObject.transform.position.x - width+0.5f +i, plane.gameObject.transform.position.y + 4.5f, plane.gameObject.transform.position.z - height+0.5f+j), Quaternion.identity);
                }
                wallArray[i, j, 5] = Instantiate(ceilBlock, new Vector3(plane.gameObject.transform.position.x - width+0.5f +i, plane.gameObject.transform.position.y + 5.5f, plane.gameObject.transform.position.z - height+0.5f+j), Quaternion.identity);

            }
        }
    }

}
