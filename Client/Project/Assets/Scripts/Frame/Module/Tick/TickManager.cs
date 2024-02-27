using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Huge
{
    public class Tick
    {
        private const int TICK_STEP = 66;
        private const int TICK_ITEM_GROUP_NUM = 60;

        private static ulong s_Now;
        private static int s_TickIndex = 1;
        private static bool s_Inited = false;
        private static uint s_DefaultInterval;

        // ontick调度中分散tick遍历压力的容器列表
        private static List<TickItem> s_TickItemBack = new List<TickItem>();
        private static List<TickItem>[] s_TickItemGroup = new List<TickItem>[TICK_ITEM_GROUP_NUM];

        // ontick中调度updateTick的容器列表
        private static List<UpdateTickItem> s_UpdateTickList1 = new List<UpdateTickItem>();
        private static List<UpdateTickItem> s_UpdateTickList2 = new List<UpdateTickItem>();

        // tickItem存储容器
        private static Dictionary<int, TickItemBase> s_TickItems = new Dictionary<int, TickItemBase>();

        private Tick()
        {

        }

        public static void Init(uint interval)
        {
            if (s_Inited)
            {
                return;
            }

            s_Now = 0;
            s_Inited = true;
            s_DefaultInterval = interval;
            for (int i = 0; i < s_TickItemGroup.Length; i++)
            {
                s_TickItemGroup[i] = new List<TickItem>(4);
            }
        }

        #region Tick机制实现
        private static int GetTickID()
        {
            s_TickIndex++;
            if (s_TickIndex >= int.MaxValue)
            {
                s_TickIndex = 1;
            }
            return s_TickIndex;
        }

        public static void OnTick(uint deltaTime)
        {
            ulong tickEnd = s_Now + deltaTime;
            List<UpdateTickItem> list = s_UpdateTickList1;
            s_UpdateTickList1 = s_UpdateTickList2;
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                UpdateTickItem tick = list[i];
                if (tick.Valid)
                {
                    tick.OnTick(deltaTime, tickEnd);
                    if (tick.Valid)
                    {
                        s_UpdateTickList1.Add(tick);
                    }
                }
            }
            list.Clear();
            s_UpdateTickList2 = list;

            int nowIndex = -1;
            for (uint i = 1; i <= deltaTime; i++)
            {
                // 按间隔累进触发
                int index = GetTickGroupIdx(s_Now + i);
                if (nowIndex == index)
                    continue;
                nowIndex = index;
                TickStep(nowIndex, tickEnd, deltaTime);
            }

            s_Now = tickEnd;
        }

        private static int GetTickGroupIdx(ulong tick)
        {
            return (int)((tick / TICK_STEP) % TICK_ITEM_GROUP_NUM);
        }

        private static int Cascade(List<TickItem>[] vec, int index)
        {
            List<TickItem> list = vec[index];
            vec[index] = new List<TickItem>(4);
            foreach (TickItem tick in list)
            {
                if (tick.Valid)
                {
                    RegisterInternal(tick);
                }
            }
            return index;
        }

        private static void TickStep(int index, ulong tickEnd, uint deltaTime)
        {
            List<TickItem> list = s_TickItemGroup[index];
            if (list.Count == 0)
            {
                return;
            }
            s_TickItemGroup[index] = s_TickItemBack;

            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                TickItem tick = list[i];
                if (tick.Valid)
                {
                    if (tickEnd >= tick.NextTickTime)
                    {
                        tick.OnTick(deltaTime, tickEnd);
                    }

                    if (tick.Valid)
                    {
                        RegisterInternal(tick);
                    }
                }
            }
            list.Clear();
            s_TickItemBack = list;
        }

        private static void RegisterInternal(TickItem tick)
        {
            int index = GetTickGroupIdx(tick.NextTickTime);
            List<TickItem> list = s_TickItemGroup[index];
            list.Add(tick);
        }
        #endregion

        /// <summary>
        /// 注销固定间隔的tick
        /// </summary>
        /// <param name="tickID"></param>
        public static void RemoveFixedTimeTick(int tickID)
        {
            s_TickItems.TryGetValue(tickID, out var tick);
            if (!(tick is TickItem))
            {
                Huge.Debug.LogError($"RemoveTick Error: cant get tickItem [{tickID}]");
                return;
            }

            var tickItem = (TickItem)tick;
            tickItem.TickAction = null;
            tickItem.Valid = false;
            s_TickItems.Remove(tickID);
        }

        /// <summary>
        /// 注册默认间隔的Tick
        /// </summary>
        /// <param name="tickFun">tick处理函数</param>
        /// <param name="nType">tick类型</param>
        /// <returns>tickId</returns>
        public static int RegisterFixedTimeTick(FixedTimeTickHandler tickFun, TickType nType = TickType.Loop)
        {
            return RegisterFixedTimeTick(tickFun, s_DefaultInterval, nType);
        }

        /// <summary>
        /// 注册固定间隔的Tick
        /// </summary>
        /// <param name="tickFun">tick处理函数</param>
        /// <param name="interval">间隔</param>
        /// <param name="nType">tick类型</param>
        /// <returns>tickId</returns>
        public static int RegisterFixedTimeTick(FixedTimeTickHandler tickFun, uint interval, TickType nType = TickType.Loop)
        {
            int tickID = GetTickID();
            TickItem tick = new TickItem(tickID, tickFun, interval, s_Now + interval, nType);
            s_TickItems[tickID] = tick;
            RegisterInternal(tick);
            return tickID;
        }

        /// <summary>
        /// 注销每帧调用的tick
        /// </summary>
        /// <param name="tickID"></param>
        public static void RemoveUpdateTick(int tickID)
        {
            s_TickItems.TryGetValue(tickID, out var tick);
            if (!(tick is UpdateTickItem))
            {
                Huge.Debug.LogError($"RemoveUpdateTick Error: cant get tickItem [{tickID}]");
                return;
            }

            var updateTickItem = (UpdateTickItem)tick;
            updateTickItem.TickAction = null;
            updateTickItem.Valid = false;
            s_TickItems.Remove(tickID);
        }

        /// <summary>
        /// 注册每帧调用的tick
        /// </summary>
        /// <param name="tickAction">tick处理函数</param>
        /// <param name="nType">tick类型</param>
        /// <returns>tickId</returns>
        public static int RegisterUpdateTick(UpdateTickHandler tickAction, TickType nType)
        {
            int tickID = GetTickID();
            var tick = new UpdateTickItem(tickID, tickAction, nType);
            s_UpdateTickList1.Add(tick);
            s_TickItems[tickID] = tick;
            return tickID;
        }
    }
}
