﻿using System;

namespace ET.Server
{
    [FriendOf(typeof(DBManagerComponent))]
    public static partial class DBManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this DBManagerComponent self)
        {
            DBManagerComponent.Instance = self;
        }

        [EntitySystem]
        private static void Destroy(this DBManagerComponent self)
        {
            DBManagerComponent.Instance = null;
        }
        
        public static DBComponent GetZoneDB(this DBManagerComponent self, int zone)
        {
            DBComponent dbComponent = self.DBComponents[zone];
            if (dbComponent != null)
            {
                return dbComponent;
            }

            StartZoneConfig startZoneConfig = StartZoneConfigCategory.Instance.Get(zone);
            if (startZoneConfig.DBConnection == "")
            {
                throw new Exception($"zone: {zone} not found mongo connect string");
            }

            dbComponent = self.AddChild<DBComponent, string, string, int>(startZoneConfig.DBConnection, startZoneConfig.DBName, zone);
            self.DBComponents[zone] = dbComponent;
            return dbComponent;
        }
    }
}