using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public sealed class SharedGameObjectInfo
    {
        public int SummonOwnerActorId = -1;

        public List<int> Summons = new List<int>();

        //渲染层缓存
        public List<object> m_SkinedOriginalMaterials = new List<object>();
        public bool m_SkinedMaterialChanged;
        public List<object> m_MeshOriginalMaterials = new List<object>();
        public bool m_MeshMaterialChanged;
    }
}
