using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MoodBox : MonoBehaviour {

    //void OnDrawGizmos()
    //{
    //    Gizmos.matrix = this.transform.localToWorldMatrix;
    //    Gizmos.color = new Color(0.5f, 0.9f, 1.0f, 0.35f);
    //    Gizmos.DrawCube(Vector3.zero, Vector3.one);
    //}

    void OnDrawGizmosSelected()
    {
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.color = new Color(0.5f, 0.9f, 1.0f, 0.35f);
        Gizmos.DrawCube(GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size);//measured in the object's local space.
        //Gizmos.DrawCube(GetComponent<Collider>().bounds.center, GetComponent<Collider>().bounds.size);//The center of the bounding box. world space

        /**
         *  1、Collider.bounds.center
            2、Collider.rigidbody.worldCenterOfMass
            3、Collider.attachedRigidbody.worldCenterOfMass
            第一种方式其实是认为碰撞体是均匀几何体，所以取碰撞盒中心作为质心。注意，这里的Collider并不是collider属性，而是表示一个Collider引用，所以不必考虑collider属性访问引起的性能开销（其实在处理碰撞时，Collider引用是能够直接得到的，也不需要做collider属性访问）。

            设计实验——针对同一个碰撞体，分别调用以上三种方式各8×1024×1024次
            实验结果——第一种约4500ms，第二种约2500ms，第三种约2000ms。

            在碰撞不涉及刚体的情况下，要获取质心只能使用性能最差的第一种方式。而有刚体的情况下，自然推荐使用第三种方式。
         */
    }
}
