using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float positionY = 0;
    [SerializeField]
    private float division = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �}�E�X�̃X�N���[�����W���擾
        Vector3 mousePosition = Input.mousePosition;

        // �X�N���[�����W�����[���h���W�ɕϊ�
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // �I�u�W�F�N�g���}�E�X�ʒu�Ɉړ�
        transform.position = new Vector3(mousePosition.x/division,positionY,0);
    }
}
