using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
    public class NpcView : CharacterView
    {
        private NpcInfo m_Npc = null;

        private void Release()
        {
            if (ObjectInfo.SummonOwnerActorId > 0)
            {
                CharacterInfo owner = WorldSystem.Instance.GetCharacterById(m_Npc.SummonOwnerId);
                if (owner != null)
                {
                    owner.GetSkillStateInfo().RecyleSummonObject(m_Npc);
                }
                CharacterView owner_view = EntityManager.Instance.GetCharacterViewById(m_Npc.SummonOwnerId);
                if (owner_view != null)
                {
                    owner_view.ObjectInfo.Summons.Remove(Actor);
                }
            }
        }

        public void Destroy()
        {
            Release();
            DestroyActor();
        }
    }
}
