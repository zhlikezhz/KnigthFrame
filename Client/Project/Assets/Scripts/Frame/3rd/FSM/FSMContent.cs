using UnityEngine;
using Huge;

namespace Huge.FSM
{
    public class FSMContent     
    {
        private int m_iTickID = -1;
        private FSMState m_State;
        private System.Type m_StateType;

        public FSMContent()
        {
            Huge.TickManager.RegisterUpdateTick((delta, tickID) => { m_iTickID = tickID; Update(delta); }, Huge.TickType.Loop);
        }

        public void Destroy()
        {
            if (m_State != null)
            {
                m_State.OnLeave(this);
                m_State.OnDestroy(this);
            }

            if (m_iTickID != -1)
            {
                Huge.TickManager.RemoveUpdateTick(m_iTickID);
                m_iTickID = -1;
            }
        }

        public void ChangeState<T>() where T : FSMState, new()
        {
            System.Type stateType = typeof(T);
            if (m_StateType == stateType)
            {
                m_State.OnLeave(this);
                m_State.OnEnter(this);
            }
            else
            {
                T state = new T();
                if (m_State != null)
                {
                    m_State.OnLeave(this);
                    m_State.OnDestroy(this);
                }
                state.OnInit(this);
                state.OnEnter(this);
                m_State = state;
            }
        }

        void Update(uint delta)
        {
            if (m_State != null)
            {
                m_State.OnUpdate(this);
            }
        }
    }
}
