namespace uTrans.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using SQLite4Unity3d;
    using UnityEngine;
#if !UNITY_EDITOR
    using System.Collections;
    using System.IO;
#endif

    public class DBConnection
    {

        private SQLiteConnection _connection;

        public DBConnection(string DatabaseName)
        {

#if UNITY_EDITOR
            var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
            // check if file exists in Application.persistentDataPath
            var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
        while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
        // then save to Application.persistentDataPath
        File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
            var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
       // then save to Application.persistentDataPath
       File.Copy(loadDb, filepath);
#elif UNITY_WP8
            var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
        // then save to Application.persistentDataPath
        File.Copy(loadDb, filepath);

#elif UNITY_WINRT
            var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
        // then save to Application.persistentDataPath
        File.Copy(loadDb, filepath);

#elif UNITY_STANDALONE_OSX
            var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
        // then save to Application.persistentDataPath
        File.Copy(loadDb, filepath);
#else
            var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
            // then save to Application.persistentDataPath
            File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
    }

    var dbPath = filepath;
#endif
            _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            Debug.Log("Final PATH: " + dbPath);

        }

        public void CreateTable<T>()
        {
            _connection.DropTable<T>();
            _connection.CreateTable<T>();
        }

        public void CreateOrUpdateTable<T>()
        {
            _connection.CreateTable<T>();
        }

        public IEnumerable<T> GetAll<T>() where T : new()
        {
            return _connection.Table<T>();
        }

        public IEnumerable<T> Find<T>(Expression<Func<T, bool>> clause) where T : new()
        {
            return _connection.Table<T>().Where(clause);
        }

        public T GetById<T>(object key) where T : new()
        {
            return _connection.Get<T>(key);
        }

        public void Insert(BaseDTO dto)
        {
            _connection.Insert(dto);
        }

        public void Update(BaseDTO dto)
        {
            _connection.Update(dto);
        }

        public void Delete(BaseDTO dto)
        {
            _connection.Delete(dto);
        }

        public void Delete<T>(object key) where T : new()
        {
            _connection.Delete<T>(key);
        }
    }
}