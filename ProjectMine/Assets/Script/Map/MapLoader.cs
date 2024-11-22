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
    // �� ���� ����
    public Object[,,] wallArray = new Object[100, 100, 6];

    // �� ���� ��ġ
    public GameObject plane;

    // ������ ������ ����
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

        // �� �迭 ����
        roomArray[0] = startMonsterRoom;
        roomArray[1] = monsterRoom;
        roomArray[2] = upgradeRoom;
        roomArray[3] = gemRoom;

        for (int i = 1; i < 99; i++)
        {
            for (int j = 1; j < 99; j++)
            {
                // ���� ���� Ȯ��
                if (Random.Range(1, 11) < 8)
                {
                    CreateWall(i, j, 4, wall1);
                }
            }
        }

        // �׵θ��� ����
        CreateOutLine();

        // ���� ������ ����� ����
        BossRoomCreate();

        // �� 4�� ����
        for (int i = 0; i < 4; i++)
        {
            RoomCreate();
        }

        CreateCeil();
    }

    // ������ �����
    void BossRoomCreate()
    {
        // ������ ���������
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
        // (������ ��)���۹� ���������
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

        // ��������
        Instantiate(bossRoom, new Vector3(plane.gameObject.transform.position.x + width*3/4-0.5f, plane.gameObject.transform.position.y + 2, plane.gameObject.transform.position.z + height*3/4-0.5f), Quaternion.identity);
        Instantiate(upgradeRoom, new Vector3(plane.gameObject.transform.position.x + 8 +roomSizeX/2, plane.gameObject.transform.position.y + 2, plane.gameObject.transform.position.z + 5+roomSizeZ/2-1f), Quaternion.identity);

        // ������Ʈ ����
        //Instantiate(bossMonster, new Vector3(plane.gameObject.transform.position.x + width*3/4-0.5f +9, plane.gameObject.transform.position.y+0.5f, plane.gameObject.transform.position.z + height*3/4-0.5f), Quaternion.identity);
        bossMonster.transform.Translate(new Vector3(-40, plane.gameObject.transform.position.y, -40));

        //Instantiate(upgradeSlotMachine, new Vector3(plane.gameObject.transform.position.x + 8 +roomSizeX/2, plane.gameObject.transform.position.y+0.5f, plane.gameObject.transform.position.z + 5+roomSizeZ/2-1f), Quaternion.identity);
        upgradeSlotMachine.transform.Translate(new Vector3(12, plane.gameObject.transform.position.y, 17));
        upgradeRoomObject.transform.Translate(new Vector3(27, plane.gameObject.transform.position.y, 2));


        // �׵θ� �� ����(������)
        for (int i = 0; i < height/2; i++)
        {
            CreateWall(width*3/2-1, height*3/2+i-1, 5, hardWall);
        }
        for (int i = 0; i < width/2 -4; i++)
        {
            CreateWall(width*3/2+3+i, height*3/2-1, 5, hardWall);
        }
        // �׵θ� �� ����(���۹�)
        int randomNum = Random.Range(1, 3);
        if (randomNum == 1)
        {
            /// �� ����� case1
            // �� �����(���� ����)
            for (int i = 0; i < roomSizeX/4; i++)
            {
                // ��
                CreateWall(i+width + 8, height + 4+roomSizeZ-1, 5, hardWall);
                // �Ʒ�
                CreateWall(i+width + 8+roomSizeX/4*3, height + 4, 5, hardWall);
            }
            for (int i = 0; i < roomSizeX - roomSizeX/4; i++)
            {
                // ��
                CreateWall(i+width + 8+roomSizeX/4, height + 4+roomSizeZ-1, 5, wall1);
                // �Ʒ�
                CreateWall(i+width + 8, height + 4, 5, wall1);
            }

            // �� �����(���� ����)
            for (int i = 1; i < roomSizeZ/4; i++)
            {
                // ����
                CreateWall(width + 8, height + 4 + i +roomSizeZ/4*3-1, 5, hardWall);
                // ������
                CreateWall(width + 8+roomSizeX-1, height + 4 + i, 5, hardWall);
            }
            for (int i = 1; i < roomSizeZ - roomSizeZ/4; i++)
            {
                // ����
                CreateWall(width + 8, height + 4 + i, 5, wall1);
                // ������
                CreateWall(width + 8+roomSizeX-1, height + 4 + i +roomSizeZ/4-1, 5, wall1);
            }
        }
        else if (randomNum == 2)
        {
            /// �� ����� case2
            // �� �����(���� ����)
            for (int i = 0; i < roomSizeX/4; i++)
            {
                // ��
                CreateWall(width + 8+i, height + 4+roomSizeZ-1, 5, hardWall);
                CreateWall(width + 8+i+roomSizeX/2, height + 4+roomSizeZ-1, 5, hardWall);
                // �Ʒ�
                CreateWall(width + 8+i, height + 4, 5, hardWall);
                CreateWall(width + 8+i + roomSizeX/4*3, height + 4, 5, hardWall);
            }
            for (int i = 0; i < roomSizeX/4; i++)
            {
                // ��
                CreateWall(width + 8+i+roomSizeX/4, height + 4+roomSizeZ-1, 5, wall1);
                CreateWall(width + 8+i+roomSizeX/4*3, height + 4+roomSizeZ-1, 5, wall1);
                // �Ʒ�
                CreateWall(width + 8+i+roomSizeX/4, height + 4, 5, wall1);
                CreateWall(width + 8+i+roomSizeX/2, height + 4, 5, wall1);
            }

            // �� �����(���� ����)
            for (int i = 1; i < roomSizeZ/4; i++)
            {
                // ����
                CreateWall(width + 8, height + 4 + i +roomSizeZ/4*3-1, 5, hardWall);
                CreateWall(width + 8, height + 4 + i, 5, hardWall);
                // ������
                CreateWall(width + 8+roomSizeX-1, height + 4 + i, 5, hardWall);
            }
            for (int i = 1; i < roomSizeZ/4+1; i++)
            {
                // ����
                CreateWall(width + 8, height + 4 + i-1+roomSizeX/2, 5, wall1);
                CreateWall(width + 8, height + 4 + i +roomSizeZ/4-1, 5, wall1);
                // ������            
                CreateWall(width + 8+roomSizeX-1, height + 4 + i -1+roomSizeZ/2, 5, wall1);
                CreateWall(width + 8+roomSizeX-1, height + 4 + i +roomSizeZ/4-1, 5, wall1);
            }
            for (int i = 1; i < roomSizeZ/4; i++)
            {
                CreateWall(width + 8+roomSizeX-1, height + 4 + i +roomSizeZ/4*3-1, 5, wall1);
            }
        }
    }

    // ���� �游���
    void RoomCreate()
    {
        // �� ��ǥ ���ϱ�
        RoomXZCreate();

        // �� ���� ���� 
        Instantiate(roomArray[roomCreateCount], new Vector3(plane.gameObject.transform.position.x - width +roomX, plane.gameObject.transform.position.y + 2, plane.gameObject.transform.position.z - height+roomZ), Quaternion.identity);
        // ������Ʈ ����
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

        // ��ĭ�����
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

        // ������� 2�� ����� ��
        if (randomNum == 1)
        {
            /// �� ����� case1
            // �� �����(���� ����)
            for (int i = 0; i < roomSizeX/4; i++)
            {
                // ��
                CreateWall(roomX+i-roomSizeX/2, roomZ+roomSizeZ/2-1, 4, hardWall);
                // �Ʒ�
                CreateWall(roomX+i+roomSizeX/4, roomZ-roomSizeZ/2, 4, hardWall);
            }
            for (int i = 0; i < roomSizeX - roomSizeX/4; i++)
            {
                // ��
                CreateWall(roomX+i-roomSizeX/4, roomZ+roomSizeZ/2-1, 4, wall1);
                // �Ʒ�
                CreateWall(roomX+i-roomSizeX/2, roomZ-roomSizeZ/2, 4, wall1);
            }

            // �� �����(���� ����)
            for (int i = 1; i < roomSizeZ/4; i++)
            {
                // ����
                CreateWall(roomX-roomSizeX/2, roomZ + i +roomSizeZ/4-1, 4, hardWall);
                // ������
                CreateWall(roomX+roomSizeX/2-1, roomZ + i -roomSizeZ/2, 4, hardWall);
            }
            for (int i = 1; i < roomSizeZ - roomSizeZ/4; i++)
            {
                // ����
                CreateWall(roomX-roomSizeX/2, roomZ + i -roomSizeZ/2, 4, wall1);
                // ������
                CreateWall(roomX+roomSizeX/2-1, roomZ + i -roomSizeZ/4-1, 4, wall1);
            }
        }
        else if (randomNum == 2)
        {
            /// �� ����� case2
            // �� �����(���� ����)
            for (int i = 0; i < roomSizeX/4; i++)
            {
                // ��
                CreateWall(roomX+i-roomSizeX/2, roomZ+roomSizeZ/2-1, 4, hardWall);
                CreateWall(roomX+i, roomZ+roomSizeZ/2-1, 4, hardWall);
                // �Ʒ�
                CreateWall(roomX+i-roomSizeX/2, roomZ-roomSizeZ/2, 4, hardWall);
                CreateWall(roomX+i + roomSizeX/4, roomZ-roomSizeZ/2, 4, hardWall);
            }
            for (int i = 0; i < roomSizeX/4; i++)
            {
                // ��
                CreateWall(roomX+i-roomSizeX/4, roomZ+roomSizeZ/2-1, 4, wall1);
                CreateWall(roomX+i+roomSizeX/4, roomZ+roomSizeZ/2-1, 4, wall1);
                // �Ʒ�
                CreateWall(roomX+i-roomSizeX/4, roomZ-roomSizeZ/2, 4, wall1);
                CreateWall(roomX+i, roomZ-roomSizeZ/2, 4, wall1);
            }

            // �� �����(���� ����)
            for (int i = 1; i < roomSizeZ/4; i++)
            {
                // ����
                CreateWall(roomX-roomSizeX/2, roomZ + i +roomSizeZ/4-1, 4, hardWall);
                CreateWall(roomX-roomSizeX/2, roomZ + i -roomSizeZ/2, 4, hardWall);
                // ������
                CreateWall(roomX+roomSizeX/2-1, roomZ + i -roomSizeZ/2, 4, hardWall);
            }
            for (int i = 1; i < roomSizeZ/4+1; i++)
            {
                // ����
                CreateWall(roomX-roomSizeX/2, roomZ + i-1, 4, wall1);
                CreateWall(roomX-roomSizeX/2, roomZ + i -roomSizeZ/4-1, 4, wall1);
                // ������            
                CreateWall(roomX+roomSizeX/2-1, roomZ + i -1, 4, wall1);
                CreateWall(roomX+roomSizeX/2-1, roomZ + i -roomSizeZ/4-1, 4, wall1);
            }
            for (int i = 1; i < roomSizeZ/4; i++)
            {
                CreateWall(roomX+roomSizeX/2-1, roomZ + i +roomSizeZ/4-1, 4, wall1);
            }
        }


    }

    // �� ��ǥ ���ϴ� �Լ�
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

    // ���� 3�� ����� �⺻ �Լ�
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

    // ��ü���� �׵θ� �κ� �����
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

    // õ�� �����
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
